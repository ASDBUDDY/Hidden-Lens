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
        public static string RESPAWN_POINT = "RespawnPoint";
        public static string ABILITY_UNLOCK = "AbilityUnlocked";
        public static string GAUGE_UNLOCK = "GaugeUnlocked";
    }

    public static class ControlSchemes
    {
        public const string KEYBOARD = "Keyboard";
        public const string GAMEPAD = "Gamepad";
    }

    public static class Interactables
    {
        public static string CHEST_ID = "ChestUnlock";
        
    }
}
