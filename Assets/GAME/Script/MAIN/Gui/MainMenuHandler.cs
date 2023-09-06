using MultiCoreLibCSE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {

    public MenuPage menuState = MenuPage.MAIN;

    [Header("Main Screen")]
    public GameObject MainScreen;

    [Header("Loading World Screen")]
    public GameObject LoadWorldScreen;

    [Header("Worldgen Screen")]
    public GameObject NewWorldScreen;
    public TMP_InputField WorldNameInputField;
    public TMP_InputField WorldSeedInputField;
    public TMP_Dropdown ChunkSizeDropdownInput;

    [Header("Settings Screen")]
    public GameObject SettingsScreen;

    void Start(){
        
    }

    void Update(){

        switch (menuState) {
            case MenuPage.MAIN:
                    MenuStateMachine(true, false, false, false);
                break;
            case MenuPage.PLAY_LOAD:
                    MenuStateMachine(false, true, false, false);
                break;
            case MenuPage.PLAY_NEW:
                    MenuStateMachine(false, false, true, false);
                break;
            case MenuPage.SETTINGS:
                    MenuStateMachine(false, false, false, true);
                break;
        }
    }

    public void MenuStateMachine(bool main, bool loadW, bool newW, bool settings) {
        MainScreen.SetActive(main);
        LoadWorldScreen.SetActive(loadW);
        NewWorldScreen.SetActive(newW);
        SettingsScreen.SetActive(settings);
    }


    public void SwitchToMenu(int page) {
        this.menuState = (MenuPage)page;
    }

    public void GenerateWorld() {
        //steps:
        //read settings from gui - check
        //create world gen config object from said settings - check
        //save config to file - check
        //async load JWE scene - check

        bool genRandomSeed = true;
        if (WorldSeedInputField.text.Length > 0) {
            genRandomSeed = false;
        }
        WorldConfig config = new WorldConfig(false, WorldNameInputField.text, genRandomSeed, WorldSeedInputField.text.GetHashCode(), 1024, Int32.Parse(ChunkSizeDropdownInput.options[ChunkSizeDropdownInput.value].text), LandmassGenerator.Perlin, BiomeGenerator.HeatWetMap, 10, 4);

        Debug.Log("Generating new world with settings:"); 
        config.debugThis();

        FileHelperUtil.SaveFile(config, GameLocationUtil.gameSavesLoc + "/worldconfig.jasgcfg");
        if (File.Exists(GameLocationUtil.gameSavesLoc + "/worldconfig.jasgcfg")) {
            Debug.Log("Saved config [" + GameLocationUtil.gameSavesLoc + "/worldconfig.jasgcfg" + "]");
        } else {
            Debug.Log("Faled to save world config");
        }

        StartCoroutine(GenericUtils.LoadSceneAsync("JWE"));
        //SceneManager.LoadScene("JWE");
    }

    public void LoadWorld() {
        Debug.Log("Loading a world");
        //steps:
        //i have no idea, make saving worlds first, so that you have something to load
    }
}

public enum MenuPage {
    MAIN,
    PLAY_LOAD,
    PLAY_NEW,
    SETTINGS
}