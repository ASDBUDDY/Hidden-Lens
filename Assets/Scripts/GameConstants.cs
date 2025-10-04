using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants : MonoBehaviour
{
    public static class Layers
    {
        public static int PLAYER_LAYER = 6;
        public static int ENEMY_LAYER = 8;
    }


    public static class PlayerPrefConstants
    {
        public static string BGM_VOLUME = "BGMVolume";
        public static string INGAME_VOLUME = "IngameVolume";
        public static string UI_VOLUME = "UIVolume";
    }
}
