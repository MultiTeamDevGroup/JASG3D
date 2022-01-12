using UnityEngine;

public class ObjectRegistry {
    public static JasgObject GrassBlock = new JasgObject(JASGMain.GameId,"grass_block", new JasgObject.Properties().BlockMapColor(new Color(0,1,0)));

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Register() {
        JASGMain.Registry.register(GrassBlock);
    }

    public class JasgObject : JASGMain.JasgRegisterable {
        public int hardness;
        public Color mapColor;
        public JASGMain.ResourceLocation model;

        public JasgObject(string nameSpace, string id, JasgObject.Properties properties) : base(id, nameSpace) {
            hardness = properties.harndessLevel.Equals(null) ? 1 : properties.harndessLevel;
            mapColor = properties.mapColor.Equals(null) ? new Color(0,0,0) : properties.mapColor;
            //model = new JASGMain.ResourceLocation(nameSpace + "/models/block/", id+".ply", JASGMain.ModelType.BLOCK);
        }
    
        public class Properties {
            public int harndessLevel;
            public Color mapColor;

            public JasgObject.Properties Hardness(int hardnessLevel) {
                this.harndessLevel = hardnessLevel;
                return this;
            }
        
            public JasgObject.Properties BlockMapColor(Color mapColor) {
                this.mapColor = mapColor;
                return this;
            }
        
        }
    }
}