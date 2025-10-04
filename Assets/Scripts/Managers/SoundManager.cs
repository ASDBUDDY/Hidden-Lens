using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public BoolEventChannelSO SoundChange;

   private float BGMusicVolume = 100f;
    private float UISoundVolume = 100f;
    private float IngameSFXVolume = 100f;


    public float GetBGMVol => BGMusicVolume;
    public float GetIngameVol => IngameSFXVolume;
    public float GetUIVol => UISoundVolume;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    private void Start()
    {
        SetupVolume();
    }
    private void SetupVolume()
    {
        if (PlayerPrefs.HasKey(GameConstants.PlayerPrefConstants.BGM_VOLUME))
        {
            BGMusicVolume = PlayerPrefs.GetFloat(GameConstants.PlayerPrefConstants.BGM_VOLUME);
        }
        

        if (PlayerPrefs.HasKey(GameConstants.PlayerPrefConstants.INGAME_VOLUME))
        {
            IngameSFXVolume = PlayerPrefs.GetFloat(GameConstants.PlayerPrefConstants.INGAME_VOLUME);
        }
        

        if (PlayerPrefs.HasKey(GameConstants.PlayerPrefConstants.UI_VOLUME))
        {
            UISoundVolume = PlayerPrefs.GetFloat(GameConstants.PlayerPrefConstants.UI_VOLUME);
        }
        
        UpdateSoundPrefs();

    }

    public void SetVolume(string VolumeType, float Volume)
    {
        
        if(string.Equals(VolumeType, GameConstants.PlayerPrefConstants.INGAME_VOLUME))
        {
            IngameSFXVolume = Volume;
        }
        else if (string.Equals(VolumeType, GameConstants.PlayerPrefConstants.BGM_VOLUME))
        {
            BGMusicVolume = Volume;
        }
        else if (string.Equals(VolumeType, GameConstants.PlayerPrefConstants.UI_VOLUME))
        {
            UISoundVolume = Volume;
        }
        else
        {

        }

        SoundChange.RaiseEvent(true);
        UpdateSoundPrefs();
    }

    private void UpdateSoundPrefs()
    {
        PlayerPrefs.SetFloat(GameConstants.PlayerPrefConstants.BGM_VOLUME, BGMusicVolume);
        PlayerPrefs.SetFloat(GameConstants.PlayerPrefConstants.INGAME_VOLUME, IngameSFXVolume);
        PlayerPrefs.SetFloat(GameConstants.PlayerPrefConstants.UI_VOLUME, UISoundVolume);
    }

}
