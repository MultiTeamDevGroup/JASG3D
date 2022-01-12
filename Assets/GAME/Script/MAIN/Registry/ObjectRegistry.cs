using System.Collections.Generic;
using UnityEngine;

public class ObjectRegistry {
    //public static JasgObject GrassBlock = new JasgObject(JASGMain.GameId,"grass_block", new JasgObject.Properties().BlockMapColor(new Color(0,1,0)));

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Register() {
        //JASGMain.Registry.register(GrassBlock);
    }

    public class JasgObject : JASGMain.JasgRegisterable {
        public Vector3Int size;
        public int hardness;
        public ItemRegistry.ToolType requiredTool;
        public Color mapColor;
        public JASGMain.ResourceLocation model;

        public JasgObject(string nameSpace, string id, JasgObject.Properties properties) : base(id, nameSpace) {
            this.size = properties.size;
            this.hardness = properties.hardness;
            this.requiredTool = properties.requiredTool;
            this.mapColor = properties.mapColor;
            //model = new JASGMain.ResourceLocation(nameSpace + "/models/block/", id+".ply", JASGMain.ModelType.BLOCK);
        }
    
        public class Properties {
            public Vector3Int size;
            public int hardness;
            public ItemRegistry.ToolType requiredTool;
            public Color mapColor;

            public JasgObject.Properties Size(Vector3Int size) {
                this.size = size;
                return this;
            }
            
            public JasgObject.Properties Hardness(int hardnessLevel) {
                this.hardness = hardnessLevel;
                return this;
            }
            
            public JasgObject.Properties ToolType(ItemRegistry.ToolType toolType) {
                this.requiredTool = toolType;
                return this;
            }

            public JasgObject.Properties MapColor(Color mapColor) {
                this.mapColor = mapColor;
                return this;
            }
        
        }
    }
}