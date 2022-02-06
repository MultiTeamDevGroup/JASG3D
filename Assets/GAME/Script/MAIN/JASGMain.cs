using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            biomeRegistry.Add(registerable);
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

    public class ResourceLocation {
        public Mesh model;

        public ResourceLocation(ObjectRegistry.JasgObject jasgObject, string location) {
            string fileLocation = jasgObject.nameSpace + "/models/" + location + jasgObject.id;
            this.model = PLYVoxelParser.parse(Resources.Load<TextAsset>(fileLocation).text).generateMesh();
        }
        
        public ResourceLocation(BlockRegistry.JasgBlock jasgBlock, string location) {
            string fileLocation = jasgBlock.nameSpace + "/textures/" + location + jasgBlock.id;

            Texture2D texture = Resources.Load<Texture2D>(fileLocation);
            
            
            List<Vector3> vertices_ = new List<Vector3>();
            List<Color> vertexColor = new List<Color>();
            List<int> tris = new List<int>();
            
            vertices_.Add(new Vector3(0, 0, 0));
            vertices_.Add(new Vector3(0, 0, 1));
            vertices_.Add(new Vector3(1, 0, 0));
            vertices_.Add(new Vector3(1, 0, 1));
            
            vertices_.Add(new Vector3(0, 1, 0));
            vertices_.Add(new Vector3(0, 1, 1));
            vertices_.Add(new Vector3(1, 1, 0));
            vertices_.Add(new Vector3(1, 1, 1));

            Color vertexColor_ = new Color32();
            vertexColor.Add(vertexColor_);
            vertexColor.Add(vertexColor_);
            vertexColor.Add(vertexColor_);
            vertexColor.Add(vertexColor_);
            vertexColor.Add(vertexColor_);
            vertexColor.Add(vertexColor_);
            vertexColor.Add(vertexColor_);
            vertexColor.Add(vertexColor_);

            int[] triangles_ = new int[] {
                //bottom
                 + 0,  + 2,  + 1,
                 + 1,  + 2,  + 3,
                //top
                 + 4,  + 5,  + 6,
                 + 5,  + 7,  + 6,
                //front
                 + 0,  + 4,  + 2,
                 + 4,  + 6,  + 2,
                //left
                 + 1,  + 5,  + 4,
                 + 4,  + 0,  + 1,
                //right
                 + 2,  + 6,  + 7,
                 + 7,  + 3,  + 2,
                //back
                 + 5,  + 1,  + 3,
                 + 3,  + 7,  + 5
            };
            
            tris.AddRange(triangles_);

            Mesh ret = new Mesh();
            ret.Clear();
            ret.vertices = vertices_.ToArray();
            ret.triangles = tris.ToArray();
            ret.colors = vertexColor.ToArray();
            
            
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
