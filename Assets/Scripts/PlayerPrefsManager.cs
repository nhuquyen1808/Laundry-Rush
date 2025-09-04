using System.Collections;
using System.Collections.Generic;
using ntDev;
using UnityEngine;

namespace DevDuck
{
    public  class PlayerPrefsManager 
    {
       public const string Coin = "COIN";
       public const string AREA_UNLOCK = "AREAUNLOCK";
       public const string SHOPPING_DONE = "SHOPPING_DONE";
       public const string CoinTest  = "COINTEST";
       public const string FIRST_TIME_DOWNLOAD = "FIRST_TIME_DOWNLOAD";
       
       public const string TUT_MAKEUP = "TUT_MAKEUP"; 
       public const string TUT_FASHION = "TUT_FASHION";
       
       //Mission Data
       public const string THE_LAST_DAY_SAVED = "THE_LAST_DAY_SAVED";
       public const string TUTORIAL_COMPLETED = "TUTORIAL_COMPLETED";
       
       public const string A_GAME_COMEPLETED = "A_GAME_COMEPLETED";
       public const string PREVIEW_A_DRESS = "PREVIEW_A_DRESS";
       public const string BUY_A_DRESS = "BUY_A_DRESS";
       public const string GET_3_STARS_A_GAME = "GET_3_STARS_A_GAME";
       public const string UNLOCK_NEW_THEME = "UNLOCK_NEW_THEME";
       public const string COMPLETE_3_TASK = "COMPLETE_3_TASK";
       public const string COMPLETE_ALL_THEME = "COMPLETE_ALL_THEME";
       
       
       
       
    }

    public static class GlobalData
    {
        public static bool isFashion = false;
        public static string currentSceneMiniGame = "";
        public static string currentTheme;
        public static int currentStory;
        public static bool isInGame = false;
        public static bool isReplay;
    }

    public static class THEME
    {
        public const string ARCADE = "ARCADE";
        public const string DEFAULT = "DEFAULT";
        public const string FASHION = "FASHION";
        public const string RESTAURANT = "RESTAURANT";
        public const string SCHOOL = "SCHOOL";
        public const string PARK = "PARK";
        public const string BAR = "BAR";
    }

    public class SceneGame
    {
        public const string sceneFashion = "SceneFashion";
        public const string sceneShop = "SceneShop";
        public const string sceneMakeup = "SceneMakeup";
        public const string Game1 = "Game1";
        public const string Game2 = "Game2";
        public const string Game3 = "Game3";
        public const string Game4 = "Game4";
        public const string Game5 = "Game5";
        public const string Game6 = "Game6";
        public const string Game8 = "Game8";
        public const string Game20 = "Game20";
        public const string Game15 = "Game15";
        public const string Game16 = "Game16";
        public const string Game18 = "Game18";
        public const string Game10 = "Game10EasyMode";
      //  public const string Game6 = "Game6";
        
    }
}
