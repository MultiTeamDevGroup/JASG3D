using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using MultiCoreLibCSE;

public class JASGMain {

    public static string GameId = "jasg";
    
    public static List<BiomeRegistry.JasgBiome> biomeRegistry = new List<BiomeRegistry.JasgBiome>();
    //public static List<RegistryObject<JasgItem>> itemRegistry = new List<RegistryObject<JasgItem>>();
    public static List<BlockRegistry.JasgBlock> blockRegistry = new List<BlockRegistry.JasgBlock>();
    public static List<ObjectRegistry.JasgObject> objectRegistry = new List<ObjectRegistry.JasgObject>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod() {
        GameLocationUtil.CheckGameLocationsIntegrity();

        Debug.Log("Initializing JASG - welcome from registry block");
        
        foreach (BlockRegistry.JasgBlock block in blockRegistry) {
            Debug.Log("Registered block [" + block.nameSpace+":"+block.id+"]");
        }
        
        foreach (BiomeRegistry.JasgBiome biome in biomeRegistry) {
            Debug.Log("Registered biome [" + biome.nameSpace+":"+biome.id+"]");
        }
        
        foreach (ObjectRegistry.JasgObject objec in objectRegistry) {
            Debug.Log("Registered biome [" + objec.nameSpace+":"+objec.id+"]");
        }
        

    }

    public class JasgRegisterable {
        public string nameSpace;
        public string id;

        public JasgRegisterable(string id, string nameSpace) {
            this.id = id;
            this.nameSpace = nameSpace;
        }
    }

    public static class Registry {

        public static JasgRegisterable register(BiomeRegistry.JasgBiome registerable) {
            biomeRegistry.Add((BiomeRegistry.JasgBiome)registerable);
            return registerable;
        }
        
        public static JasgRegisterable register(BlockRegistry.JasgBlock registerable) {
            blockRegistry.Add(registerable);
            return registerable;
        }
        
        public static JasgRegisterable register(ObjectRegistry.JasgObject registerable) {
            objectRegistry.Add(registerable);
            return registerable;
        }
        //Debug.LogException(new Exception(registerable.nameSpace+":"+registerable.id+" had failed to register correctly do tue its type setting!"));
        
        
    }
    
    public class JasgWorldObject {
        
    }
    
    public class ResourceLocation {
        public string fileLocation;
        public string fileName;
        public JasgModelFile modelFile;
        
        public ResourceLocation(string location, string fileName, ModelType modelType) {
            this.fileName = fileName;
            this.fileLocation = location;
            this.modelFile = new JasgModelFile(PLYVoxelParser.parse(Resources.Load<TextAsset>(this.fileLocation+fileName).text), modelType);
        }
    }

    public enum ModelType {
        BLOCK,
        ITEM,
        ENTITY,
        OBJECT
    }
    
    public class JasgModelFile {
        public PLYVoxelParser.PLYFile plyFile;
        public ModelType modelType;

        public JasgModelFile(PLYVoxelParser.PLYFile plyFile, ModelType modelType) {
            this.plyFile = plyFile;
            this.modelType = modelType;
        }
    }

}
