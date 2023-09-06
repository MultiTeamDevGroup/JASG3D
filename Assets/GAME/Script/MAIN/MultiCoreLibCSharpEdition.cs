using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace MultiCoreLibCSE {
    
    public static class GameLocationUtil {
        
        public static String gameSRCLoc = Application.persistentDataPath;
        public static String gameSettingsLoc = gameSRCLoc + "/settings/";
        public static String gameModsLoc = gameSRCLoc + "/mods/";
        public static String gameSavesLoc = gameSRCLoc + "/saves/";
        
        public static void CheckGameLocationsIntegrity(String gameSourcesLoc_, String gameSettingsLoc_, String gameModsLoc_, String gameSavesLoc_) {
            if (!Directory.Exists(gameSettingsLoc_)) {
                Directory.CreateDirectory(gameSettingsLoc_);
            }
            if (!Directory.Exists(gameModsLoc_)) {
                Directory.CreateDirectory(gameModsLoc_);
            }
            if (!Directory.Exists(gameSavesLoc_)) {
                Directory.CreateDirectory(gameSavesLoc_);
            }
        }

        public static void CheckGameLocationsIntegrity() {
            if (!Directory.Exists(gameSettingsLoc)) {
                Directory.CreateDirectory(gameSettingsLoc);
            }
            if (!Directory.Exists(gameModsLoc)) {
                Directory.CreateDirectory(gameModsLoc);
            }
            if (!Directory.Exists(gameSavesLoc)) {
                Directory.CreateDirectory(gameSavesLoc);
            }
        }

    }
    
    public static class FileHelperUtil {

        public static void SaveFile(Object saveObject, String location) {
            string jsonString = JsonUtility.ToJson(saveObject, true);
            File.WriteAllText(location, jsonString);

        }
        
    }

    public static class MathHelperUtil {
        public static float scale(float FromMin, float FromMax, float ToMin, float ToMax, float value) {

            float OldRange = (FromMax - FromMin);
            float NewRange = (ToMax - ToMin);
            float NewValue = (((value - FromMin) * NewRange) / OldRange) + ToMin;

            return NewValue;
        }
        
        public static Color scaleColor(int r, int g, int b) {
            return new Color(scale(0, 255, 0, 1, r), scale(0, 255, 0, 1, g), scale(0, 255, 0, 1, b));
        }

        public static Vector3Int scaleBiomeTempHumProperty(int temperature, int humidity) {
            return new Vector3Int(Mathf.RoundToInt(scale(-100, 100, 0, 255, temperature)), Mathf.RoundToInt(scale(0, 100, 0, 255, humidity)), 255);
        }

        public static int closestNumber(int input, int dividableBy) {
            int q = input / dividableBy;
      
            int n1 = dividableBy * q;
      
            int n2 = (input * dividableBy) > 0 ? (dividableBy * (q + 1)) : (dividableBy * (q - 1));

            if (Mathf.Abs(input - n1) < Mathf.Abs(input - n2)) {
                return n1;
            }
        
            return n2;     
        }
        
        public static float ConvertToRadians(float angle)
        {
            return (float)(Math.PI / 180) * angle;
        }
        
    }

    public static class GenericUtils {
        
        public class Map<T, K> {
            public T Key { get; set; }
            public K Value { get; set; }
        }

        public static IEnumerator LoadSceneAsync(string sceneToLoad)
        {

            AsyncOperation SceneLoadOp = SceneManager.LoadSceneAsync(sceneToLoad);

            while (!SceneLoadOp.isDone)
            {
                yield return null;
            }

        }

    }
}
