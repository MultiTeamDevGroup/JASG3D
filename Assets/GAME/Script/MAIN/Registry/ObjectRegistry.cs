using System.Collections.Generic;
using MultiCoreLibCSE;
using UnityEngine;

public class ObjectRegistry {
    
    public static JasgObject DefaultCactus1 = new JasgObject(JASGMain.GameId,"default_cactus_1", new JasgObject.Properties().Hardness(0).Size(1,1,1).MapColor(Color.red).ToolType(ItemRegistry.ToolType.Hand));
    public static JasgObject AlphaBush = new JasgObject(JASGMain.GameId,"alpha_bush", new JasgObject.Properties().Hardness(0).Size(1,1,1).MapColor(Color.red).ToolType(ItemRegistry.ToolType.Hand));
    public static JasgObject DefaultStone1 = new JasgObject(JASGMain.GameId,"default_stone_1", new JasgObject.Properties().Hardness(0).Size(1,1,1).MapColor(Color.red).ToolType(ItemRegistry.ToolType.Hand));
    public static JasgObject AlphaStick = new JasgObject(JASGMain.GameId,"alpha_stick", new JasgObject.Properties().Hardness(0).Size(1,1,1).MapColor(Color.red).ToolType(ItemRegistry.ToolType.Hand));

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Register() {}

    public class JasgObject : JASGMain.JasgRegisterable {
        public Vector3Int size;
        public int hardness;
        public ItemRegistry.ToolType requiredTool;
        public Color mapColor;
        public JASGMain.ResourceLocation resourceLocation;

        public JasgObject(string nameSpace, string id, JasgObject.Properties properties) : base(id, nameSpace) {
            this.size = properties.size;
            this.hardness = properties.hardness;
            this.requiredTool = properties.requiredTool;
            this.mapColor = properties.mapColor;
            JASGMain.Registry.register(this);
            this.resourceLocation = new JASGMain.ResourceLocation(this, "objects/");
        }
    
        public class Properties {
            public Vector3Int size;
            public int hardness;
            public ItemRegistry.ToolType requiredTool;
            public Color mapColor;

            public JasgObject.Properties Size(int x, int y, int z) {
                this.size = new Vector3Int(x, y, z);
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