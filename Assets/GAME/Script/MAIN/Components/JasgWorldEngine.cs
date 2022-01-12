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

    [Header("Main World Config")]
    public bool bypassLoadConfig = false;
    public WorldConfig worldConfig;

    [Header("Landmass Generator")]
    public bool[,] landmassArray;
    [Header("Perlin Landmass")]
    public float perlinScale;
    public int perlinOctaves;
    public float perlinPersistance;
    public float perlinLacunarity;
    public Vector2 perlinOffset;
    public float perlinDivision;
    [Header("Hex Landmass")]
    public int hexagonSize;

    [Header("Biome Generator")]
    public Texture2D biomeMapping;
    public BiomeRegistry.JasgBiome[,] biomeArray;
    public int himidityNodeLimit;
    public int humidityFilteringCycles;
    public int humidityFilteringAmount;
    
    
    
    public async void Start() {
	    if (!bypassLoadConfig) {
		    worldConfig = LoadWorldConfig();
	    }

        SetupWorld();

        switch (worldConfig.landmassGeneratorType) {
            case LandmassGenerator.Perlin:
	            landmassArray = await GenPerlinLandmass();
	            break;
            case LandmassGenerator.Hex:
	            landmassArray = await GenHexLandmass();
	            break;
            case LandmassGenerator.JasgCustom:
	            break;
        }

        switch (worldConfig.biomeGeneratorType) {
	        case BiomeGenerator.HeatWetMap:
		        biomeArray = await GenBiomesHeatWetMap();
		        break;
	        case BiomeGenerator.RulePuzzle:
		        break;
        }
        
        Texture2D displayMap;
        displayMap = await CompileBiomesToTexture();

        displayMap.Apply();
        displayMap.filterMode = FilterMode.Point;
                
        uiMapDisplay.GetComponent<SpriteRenderer>().sprite = Sprite.Create(displayMap, new Rect(0.0f, 0.0f, worldConfig.worldSize, worldConfig.worldSize), new Vector2(0.5f, 0.5f), 100.0f);

    }

    public WorldConfig LoadWorldConfig() {
        return new WorldConfig();
    }

    public void SetupWorld() {
	    
    }

    public async Task<bool[,]> GenPerlinLandmass() {
	    float[,] noiseMap = new float[worldConfig.worldSize,worldConfig.worldSize];
    
	    System.Random prng = new System.Random (worldConfig.worldSeed);
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
	    
		bool[,] boolArray = new bool[worldConfig.worldSize, worldConfig.worldSize];
	    for (int x = 0; x < worldConfig.worldSize; x++) {
		    for (int y = 0; y < worldConfig.worldSize; y++) {
			    if (noiseMap[x, y] >= perlinDivision) {
				    boolArray[x, y] = true;
			    }else {
				    boolArray[x, y] = false;
			    }
		    }   
	    }

	    return await Task.FromResult(boolArray);
    }

    public async Task<bool[,]> GenHexLandmass() {

	    bool[,] boolArray = new bool[worldConfig.worldSize, worldConfig.worldSize];

	    int a = 0;
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
			    if (landmassArray[x, y] == true) {
				    texture.SetPixel(x,y,new Color(1,1,1));
			    }else {
				    texture.SetPixel(x,y,new Color(0,0,0));
			    }
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
    void Update() {
        
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

[System.Serializable]
    public class WorldConfig {
        public bool loadExisting;
        public string worldName;
        public bool genRandomSeed;
        public int worldSeed;
        public int worldSize;
        public int chunkSize;
        public LandmassGenerator landmassGeneratorType;
        public BiomeGenerator biomeGeneratorType;
    }
