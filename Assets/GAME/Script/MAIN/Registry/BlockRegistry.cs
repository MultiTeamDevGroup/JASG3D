using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRegistry {
    public static JasgBlock GrassBlock = new JasgBlock(JASGMain.GameId,"grass_block", new JasgBlock.Properties().BlockMapColor(new Color(0,1,0)));
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Register() {
    }

    public class JasgBlock : JASGMain.JasgRegisterable {
        public int hardness;
        public Color mapColor;
        public JASGMain.ResourceLocation model;

        public JasgBlock(string nameSpace, string id, JasgBlock.Properties properties) : base(id, nameSpace) {
            hardness = properties.harndessLevel.Equals(null) ? 1 : properties.harndessLevel;
            mapColor = properties.mapColor.Equals(null) ? new Color(0,0,0) : properties.mapColor;
            JASGMain.Registry.register(this);
            //model = new JASGMain.ResourceLocation(nameSpace + "/models/block/", id+".ply", JASGMain.ModelType.BLOCK);
        }
        
        public class Properties {
            public int harndessLevel;
            public Color mapColor;

            public JasgBlock.Properties Hardness(int hardnessLevel) {
                this.harndessLevel = hardnessLevel;
                return this;
            }
            
            public JasgBlock.Properties BlockMapColor(Color mapColor) {
                this.mapColor = mapColor;
                return this;
            }
            
        }
    }
}
