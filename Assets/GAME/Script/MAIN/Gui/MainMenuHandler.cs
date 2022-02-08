using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour {

    public MenuPage menuState = MenuPage.MAIN;

    public GameObject MainScreen;
    public GameObject LoadWorldScreen;
    public GameObject NewWorldScreen;
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

}

public enum MenuPage {
    MAIN,
    PLAY_LOAD,
    PLAY_NEW,
    SETTINGS
}