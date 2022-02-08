using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using UnityEngine;
using MultiCoreLibCSE;
using TMPro;
using UnityEngine.SceneManagement;

public class JasgWorldEngine : MonoBehaviour {
    [Header("References")]
    public GameObject pauseMenu;
    public Renderer uiMapDisplay;
    public PlayerController player;

    [Header("Main World Config")]
    public bool bypassLoadConfig = false;
    public WorldConfig worldConfig;

    [Header("Landmass Generator")]
    public int[,] landmassArray;
    [Header("Perlin Landmass")]
    public float perlinScale;
    public int perlinOctaves;
    public float perlinPersistance;
    public float perlinLacunarity;
    public Vector2 perlinOffset;
    [Header("Hex Landmass")]
    public int hexagonSize;

    [Header("Biome Generator")]
    public Texture2D biomeMapping;
    public BiomeRegistry.JasgBiome[,] biomeArray;
    public int himidityNodeLimit;
    public int humidityFilteringCycles;
    public int humidityFilteringAmount;

    public ObjectRegistry.JasgObject[,] surfaceObjectArray;
    [Header("Surface Generator")]
    public int surfaceGenRandomRange;

    [Header("World Components")] 
    public List<WorldChunk> worldChunks;

    public async void Start() {
	    if (!bypassLoadConfig) {
		    worldConfig = LoadWorldConfig();
	    }

        SetupWorld();
        
        Task<int[,]> landmassTask = null;
        Task<BiomeRegistry.JasgBiome[,]> biomeMapTask = null;
        Task<ObjectRegistry.JasgObject[,]> surfaceObjectsTask = null;
        
        switch (worldConfig.landmassGeneratorType) {
            case LandmassGenerator.Perlin:
	            landmassTask = GenPerlinLandmass();
	            break;
            case LandmassGenerator.Hex:
	            //TODO make hex landmass actually gen hexagons
	            //landmassTask = GenHexLandmass();
	            landmassTask = GenPerlinLandmass();
	            break;
            case LandmassGenerator.JasgCustom:
	            //TODO make custom landmass gen
	            landmassTask = GenPerlinLandmass();
	            break;
        }

        switch (worldConfig.biomeGeneratorType) {
	        case BiomeGenerator.HeatWetMap:
		        biomeMapTask = GenBiomesHeatWetMap();
		        break;
	        case BiomeGenerator.RulePuzzle:
		        //TODO make RulePuzzle Biome Map Generator
		        biomeMapTask = GenBiomesHeatWetMap();
		        break;
        }

        await Task.WhenAll(landmassTask);
        await Task.WhenAll(biomeMapTask);
        
        landmassArray = landmassTask.Result;
        biomeArray = biomeMapTask.Result;

        surfaceObjectsTask = GenSurfaceObjects();
        await Task.WhenAll(surfaceObjectsTask);
        surfaceObjectArray = surfaceObjectsTask.Result;
        
        
        Texture2D displayMap;
        displayMap = await CompileObjectsToTexture();

        displayMap.Apply();
        displayMap.filterMode = FilterMode.Point;
                
        uiMapDisplay.GetComponent<SpriteRenderer>().sprite = Sprite.Create(displayMap, new Rect(0.0f, 0.0f, worldConfig.worldSize, worldConfig.worldSize), new Vector2(0.5f, 0.5f), 100.0f);
        
        FinalizeWorldGeneration();
        
    }

    public WorldConfig LoadWorldConfig() {
        return new WorldConfig();
    }

    public void SetupWorld() {
	    
    }

    public async Task<int[,]> GenPerlinLandmass() {
	    float[,] noiseMap = new float[worldConfig.worldSize,worldConfig.worldSize];
    
	    System.Random prng = new System.Random(worldConfig.worldSeed);
	    Vector2[] octaveOffsets = new Vector2[perlinOctaves];
	    for (int i = 0; i < perlinOctaves; i++) {
		    float offsetX = prng.Next (-100000, 100000) + perlinOffset.x;
		    float offsetY = prng.Next (-100000, 100000) + perlinOffset.y;
		    octaveOffsets [i] = new Vector2 (offsetX, offsetY);
	    }
    
	    if (perlinScale <= 0) {
		    perlinScale = 0.0001f;
	    }
    
	    float maxNoiseHeight = float.MinValue;
	    float minNoiseHeight = float.MaxValue;
    
	    float mapHalf = worldConfig.worldSize / 2f;
    
    
	    for (int y = 0; y < worldConfig.worldSize; y++) {
		    for (int x = 0; x < worldConfig.worldSize; x++) {
    		
			    float amplitude = 1;
			    float frequency = 1;
			    float noiseHeight = 0;
    
			    for (int i = 0; i < perlinOctaves; i++) {
				    float sampleX = (x-mapHalf) / perlinScale * frequency + octaveOffsets[i].x;
				    float sampleY = (y-mapHalf) / perlinScale * frequency + octaveOffsets[i].y;
    
				    float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
				    noiseHeight += perlinValue * amplitude;
    
				    amplitude *= perlinPersistance;
				    frequency *= perlinLacunarity;
			    }
    
			    if (noiseHeight > maxNoiseHeight) {
				    maxNoiseHeight = noiseHeight;
			    } else if (noiseHeight < minNoiseHeight) {
				    minNoiseHeight = noiseHeight;
			    }
			    noiseMap [x, y] = noiseHeight;
		    }
	    }
    
	    for (int y = 0; y < worldConfig.worldSize; y++) {
		    for (int x = 0; x < worldConfig.worldSize; x++) {
			    noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
		    }
	    }
	    
		int[,] landmass = new int[worldConfig.worldSize, worldConfig.worldSize];
	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    landmass[x, y] = Mathf.RoundToInt(noiseMap[x, y] * worldConfig.worldHightModifier);
		    }   
	    }

	    return await Task.FromResult(landmass);
    }

    public async Task<bool[,]> GenHexLandmass() {

	    bool[,] boolArray = new bool[worldConfig.worldSize, worldConfig.worldSize];

	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    if (y >= (1 - 0.866f)/2 && y <= 0.866 + (1-0.866)/2 && y <= Mathf.Sqrt(3 * x) + 1/2 && y >= -Mathf.Sqrt(3*x) + 1/2 && y <= -Mathf.Sqrt(3 * x) + 2.2320508075 && y >= Mathf.Sqrt(3*x)-1.2320508075) {
				    boolArray[x, y] = true;
			    }
		    }
	    }

	    return await Task.FromResult(boolArray);
    }
    
    public async Task<Texture2D> CompileLandmassToTexture( ) {
	    Texture2D texture = new Texture2D(worldConfig.worldSize, worldConfig.worldSize);
	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    int a = landmassArray[x, y] / worldConfig.worldHightModifier;
			    texture.SetPixel(x,y,new Color(a,a,a));
		    }
	    }
	    return await Task.FromResult(texture);
    }

    public async Task<BiomeRegistry.JasgBiome[,]> GenBiomesHeatWetMap() {
	    
	    System.Random rand = new System.Random(worldConfig.worldSeed.GetHashCode());
	    
	    
	    Vector3Int[] humidityNodes = new Vector3Int[himidityNodeLimit];
	    
	    for (int i = 0; i < humidityNodes.Length; i++) {
		    humidityNodes[i] = new Vector3Int(rand.Next(0, worldConfig.worldSize), rand.Next(0, worldConfig.worldSize), rand.Next(0, 100));
	    }

	    for (int k = 0; k < humidityFilteringCycles; k++) {
		    for (int i = 0; i < humidityNodes.Length; i++) {
			    int closestNodeIndex = 0;
			    float closestDist = worldConfig.worldSize;
			    for (int j = 0; j < humidityNodes.Length; j++) {
				    float dist = Vector2.Distance(new Vector2Int(humidityNodes[i].x, humidityNodes[i].y), new Vector2Int(humidityNodes[j].x, humidityNodes[j].y));
				    if (dist < closestDist) {
					    closestDist = dist;
					    closestNodeIndex = j;
				    }
			    }

			    if (humidityNodes[closestNodeIndex].z > humidityNodes[i].z + humidityFilteringAmount) {
				    humidityNodes[i].z = Mathf.RoundToInt(humidityNodes[closestNodeIndex].z-humidityFilteringAmount/2);
			    }
		    }
	    }
	    

	    Vector3Int[,] valuesMap = new Vector3Int[worldConfig.worldSize, worldConfig.worldSize];

	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    float nearestNodeDist = worldConfig.worldSize;
			    int nearestNodeIndex = 0;
			    Vector2Int currentPoint = new Vector2Int(x, y);
			    
			    for (int i = 0; i < humidityNodes.Length; i++) {
				    Vector2Int point = new Vector2Int(humidityNodes[i].x, humidityNodes[i].y);
				    float dist = Vector2.Distance(point, currentPoint);
				    if (dist < nearestNodeDist) {
					    nearestNodeDist = dist;
					    nearestNodeIndex = i;
				    }
			    }
			    int humidity = humidityNodes[nearestNodeIndex].z;
			    int temperature = Mathf.RoundToInt(Mathf.Lerp(0, 255, Mathf.Abs(-MathHelperUtil.scale(worldConfig.worldSize, 0, -1, 1, y))));
			    
			    valuesMap[x, y] = new Vector3Int(temperature, humidity, 255);
		    }
	    }
	    
	    BiomeRegistry.JasgBiome[,] biomeArray_ = new BiomeRegistry.JasgBiome[worldConfig.worldSize,worldConfig.worldSize];

	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    int temperature = valuesMap[x, y].x;
			    int humidity = valuesMap[x, y].y;
			    
			    Color32 color = biomeMapping.GetPixel(temperature, humidity);
			    BiomeRegistry.JasgBiome biomeToPlace = null;
			    
			    for (int i = 0; i < JASGMain.biomeRegistry.Count; i++) {
				    if (JASGMain.biomeRegistry[i].biomeDisplayColor.Equals(color)) {
					    biomeToPlace = JASGMain.biomeRegistry[i];
				    }
			    }
			    
			    biomeArray_[x, y] = biomeToPlace;
		    }
	    }


	    return await Task.FromResult(biomeArray_);
    }

    public async Task<Texture2D> CompileBiomesToTexture() {
	    Texture2D texture = new Texture2D(worldConfig.worldSize, worldConfig.worldSize);
	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    if (biomeArray[x, y] != null) {
				    texture.SetPixel(x, y, biomeArray[x,y].biomeDisplayColor);
			    }
		    }
	    }
	    return await Task.FromResult(texture);
    }

    public async Task<ObjectRegistry.JasgObject[,]> GenSurfaceObjects() {
	    ObjectRegistry.JasgObject[,] surfaceObjects = new ObjectRegistry.JasgObject[worldConfig.worldSize, worldConfig.worldSize];

	    bool[,] potentialObjectSpots = new bool[worldConfig.worldSize, worldConfig.worldSize];

	    System.Random prng = new System.Random(worldConfig.worldSeed);
	    
	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    if (prng.Next(0, 100) > surfaceGenRandomRange && landmassArray[x, y] > worldConfig.worldSeaLevel) {
				    potentialObjectSpots[x, y] = true;
			    }else {
				    potentialObjectSpots[x, y] = false;
			    }
		    }
	    }

	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    if (potentialObjectSpots[x, y]) {
				    BiomeRegistry.BiomeSurfaceDecorator decorator = biomeArray[x, y].decorator;

				    int index = prng.Next(0, decorator.surfaceObjects.Count);
			    
				    if (decorator.surfaceObjects[index].Value < prng.Next(0, 100)) {
					    surfaceObjects[x, y] = decorator.surfaceObjects[index].Key;
				    }
			    }
		    }
	    }
	    
	    return await Task.FromResult(surfaceObjects);
    }
    
    public async Task<Texture2D> CompileObjectsToTexture() {
	    Texture2D texture = new Texture2D(worldConfig.worldSize, worldConfig.worldSize);
	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    if (surfaceObjectArray[x, y] != null) {
				    texture.SetPixel(x, y, surfaceObjectArray[x,y].mapColor);
			    }
		    }
	    }
	    return await Task.FromResult(texture);
    }

    public void FinalizeWorldGeneration() {
	    
	    System.Random prng = new System.Random(worldConfig.worldSeed);
	    
	    //Map out chunks
	    int numa = worldConfig.worldSize / worldConfig.chunkSize;
	    for (int chunkX = 0; chunkX < numa; chunkX++) {
		    for (int chunkY = 0; chunkY < numa; chunkY++) {
			    WorldChunk chunk = new WorldChunk(new Vector2Int(chunkX, chunkY), worldConfig.chunkSize);
			    
			    for (int x = 0; x < worldConfig.chunkSize; x++) {
				    for (int y = 0; y < worldConfig.chunkSize; y++) {
					    int realWorldX = chunkX * worldConfig.chunkSize + x;
					    int realWorldY = chunkY * worldConfig.chunkSize + y;

					    int blockAltitude = landmassArray[realWorldX, realWorldY];
					    
					    int maxRandLength = biomeArray[realWorldX, realWorldY].decorator.surfaceBlocks.Count;
					    chunk.SetBlockInChunk(biomeArray[realWorldX, realWorldY].decorator.surfaceBlocks[prng.Next(0, maxRandLength)], x, y, blockAltitude);
					    
					    if (surfaceObjectArray[realWorldX, realWorldY] != null) {
						    chunk.SetObjectInChunk(surfaceObjectArray[realWorldX, realWorldY], x, y, blockAltitude+1);
					    }
				    }
			    }
			    
			    worldChunks.Add(chunk);
		    }
	    }
	    
    }

    
    
    void Update() {
	    LoadChunksAroundPlayer();
    }


    public async void LoadChunksAroundPlayer() {
	    foreach (WorldChunk chunk in worldChunks) {
		    Vector2Int chunkPos = chunk.chunkPosition;
		    Vector3 playerPos = player.transform.position;
		    Vector2Int playerChunkPos = new Vector2Int(Mathf.RoundToInt(playerPos.x) / worldConfig.chunkSize, Mathf.RoundToInt(playerPos.z) / worldConfig.chunkSize);

		    if (Vector2.Distance(chunkPos, playerChunkPos) > player.chunkLoadingDistance) {
			    if (chunk.isLoaded) {
				    chunk.unload();
			    }
		    }else {
			    if (!chunk.isLoaded) {
				    chunk.load();
			    }
		    }
		    
		    await Task.Yield();
	    }
    }
}

[Serializable]
public class WorldChunk {
	public Vector2Int chunkPosition;
	public BlockRegistry.JasgBlock[,,] chunkBlocks;
	public ObjectRegistry.JasgObject[,,] chunkObjects;
	public bool isLoaded;

	public WorldChunk(Vector2Int chunkPosition, int chunkSize) {
		this.chunkPosition = chunkPosition;
		this.chunkBlocks = new BlockRegistry.JasgBlock[chunkSize, chunkSize, chunkSize];
		this.chunkObjects = new ObjectRegistry.JasgObject[chunkSize, chunkSize, chunkSize];
		this.isLoaded = false;
	}

	public WorldChunk load() {
		Debug.Log("Loaded chunk at " + chunkPosition);
		this.isLoaded = true;
		return this;
	}
	
	public WorldChunk unload() {
		Debug.Log("Unloaded chunk at " + chunkPosition);
		this.isLoaded = false;
		return this;
	}

	public void SetBlockInChunk(BlockRegistry.JasgBlock JBlock, int x, int y, int z) {
		
	}
	
	public void SetObjectInChunk(ObjectRegistry.JasgObject JObject, int x, int y, int z) {
		
	}
}

public enum LandmassGenerator {
    Hex,
    Perlin,
    JasgCustom
}

public enum BiomeGenerator {
    RulePuzzle,
    HeatWetMap
}

[Serializable]
    public class WorldConfig {
        public bool loadExisting;
        public string worldName;
        public bool genRandomSeed;
        public int worldSeed;
        public int worldSize;
        public int chunkSize;
        public LandmassGenerator landmassGeneratorType;
        public BiomeGenerator biomeGeneratorType;
        public int worldHightModifier;
        public int worldSeaLevel;
    }
