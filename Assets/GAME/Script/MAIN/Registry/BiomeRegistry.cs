using System.Collections;
using System.Collections.Generic;
using MultiCoreLibCSE;
using UnityEngine;

public class BiomeRegistry {

    public static JasgBiome AlphaValley = new JasgBiome(
        JASGMain.GameId,"alpha_valley",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
            BlockRegistry.GrassBlock
        },new List<BlockRegistry.JasgBlock>()
        ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(22, 50),
        new Color32(115, 197, 42, 255)
    );
    
    public static JasgBiome DarkForest = new JasgBiome(
        JASGMain.GameId,"dark_forest",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(20, 60),
        new Color32(36, 139, 70, 255)
    );
    
    public static JasgBiome EnchantedHights = new JasgBiome(
        JASGMain.GameId,"enchanted_hights",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(20, 60),
        new Color32(39, 182, 157, 255)
    );
    
    public static JasgBiome Murk = new JasgBiome(
        JASGMain.GameId,"murk",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(26, 75),
        new Color32(67, 117, 7, 255)
    );
    
    public static JasgBiome SereneOvergrowth = new JasgBiome(
        JASGMain.GameId,"serene_overgrowth",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(32, 100),
        new Color32(71, 174, 12, 255)
    );
    
    public static JasgBiome Grazenlands = new JasgBiome(
        JASGMain.GameId,"grazenlands",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(28, 33),
        new Color32(176, 202, 66, 255)
    );
    
    public static JasgBiome CrimsonDomain = new JasgBiome(
        JASGMain.GameId,"crimson_domain",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(15, 0),
        new Color32(163, 54, 28, 255)
    );
    
    public static JasgBiome FallenGrove = new JasgBiome(
        JASGMain.GameId,"fallen_grove",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(8, 40),
        new Color32(173, 198, 201, 255)
    );
    
    public static JasgBiome PermafrostBarren = new JasgBiome(
        JASGMain.GameId,"permafrost_barren",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(-10, 33),
        new Color32(173, 211, 246, 255)
    );
    
    public static JasgBiome Desert = new JasgBiome(
        JASGMain.GameId,"desert",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(45, 0),
        new Color32(228, 199, 137, 255)
    );
    
    public static JasgBiome GlazedDepression = new JasgBiome(
        JASGMain.GameId,"glazed_depression",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(14, 0),
        new Color32(52, 51, 84, 255)
    );
    
    public static JasgBiome HadenicWaste = new JasgBiome(
        JASGMain.GameId,"hadenic_waste",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(14, 40),
        new Color32(166, 79, 133, 255)
    );
    
    public static JasgBiome DriedDepths = new JasgBiome(
        JASGMain.GameId,"dried_depths",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(35, 0),
        new Color32(243, 223, 154, 255)
    );
    
    public static JasgBiome PetrifiedTaint = new JasgBiome(
        JASGMain.GameId,"petrified_taint",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(0, 33),
        new Color32(115, 89, 118, 255)
    );
    
    public static JasgBiome TwilightCrossing = new JasgBiome(
        JASGMain.GameId,"dried_depths",
        new BiomeSurfaceDecorator(new List<BlockRegistry.JasgBlock> {
                BlockRegistry.GrassBlock
            },new List<BlockRegistry.JasgBlock>()
            ,new List<JasgTree>()),
        MathHelperUtil.scaleBiomeTempHumProperty(-5, 33),
        new Color32(105, 151, 193, 255)
    );

    

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Register() {
        JASGMain.Registry.register(AlphaValley);
        JASGMain.Registry.register(DarkForest);
        JASGMain.Registry.register(EnchantedHights);
        JASGMain.Registry.register(Murk);
        JASGMain.Registry.register(SereneOvergrowth);
        JASGMain.Registry.register(Grazenlands);
        JASGMain.Registry.register(CrimsonDomain);
        JASGMain.Registry.register(FallenGrove);
        JASGMain.Registry.register(PermafrostBarren);
        JASGMain.Registry.register(Desert);
        JASGMain.Registry.register(GlazedDepression);
        JASGMain.Registry.register(HadenicWaste);
        JASGMain.Registry.register(DriedDepths);
        JASGMain.Registry.register(PetrifiedTaint);
        JASGMain.Registry.register(TwilightCrossing);
    }

    public class JasgBiome : JASGMain.JasgRegisterable {
        public BiomeSurfaceDecorator decorator;
        public Vector3Int heatWetProperties; // x = temperature, y = humidity, 
        public Color32 biomeDisplayColor;
        public JasgBiome(string nameSpace, string id, BiomeSurfaceDecorator surfaceDecorator, Vector3Int heatWetProperties, Color32 biomeDisplayColor) : base(id, nameSpace) {
            this.decorator = surfaceDecorator;
            this.heatWetProperties = heatWetProperties;
            this.biomeDisplayColor = biomeDisplayColor;
        }
    }
    
    public class BiomeSurfaceDecorator {
        public List<BlockRegistry.JasgBlock> surfaceBlocks;
        public List<BlockRegistry.JasgBlock> foliageBlocks;
        public List<JasgTree> treeTypes;

        public BiomeSurfaceDecorator(List<BlockRegistry.JasgBlock> listOfSurfaceBlocks, List<BlockRegistry.JasgBlock> listOfFoliageBlocks, List<JasgTree> listOfTreeTypes) {
            this.surfaceBlocks = listOfSurfaceBlocks;
            this.foliageBlocks = listOfFoliageBlocks;
            this.treeTypes = listOfTreeTypes;
        }
    }
    
    public class JasgTree : JASGMain.JasgWorldObject {
        
    }
    
}
