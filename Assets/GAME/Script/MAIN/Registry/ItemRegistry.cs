using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRegistry {
    //public static JasgBlock GrassBlock = new JasgBlock(JASGMain.GameId,"grass_block", new JasgBlock.Properties().BlockMapColor(new Color(0,1,0)));
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Register() {
        //JASGMain.Registry.register(GrassBlock);
    }

    public enum ToolType {
        Hand,
        Pickaxe,
        Axe,
        Sword,
        Hoe
    }

}