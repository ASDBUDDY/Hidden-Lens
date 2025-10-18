using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerMainScript PlayerObj;
    private PlayerInput CurrentPlayerInput;
    public PlayerControlScheme CurrentPlayerControlScheme = PlayerControlScheme.Keyboard;

    public bool IsLensUnlocked => PlayerObj.hiddenLensUnlocked;

    public void ActivateLens(bool firstTime = true) 
    {
        if (firstTime)
        {
            PlayerPrefs.SetInt(GameConstants.PlayerPrefConstants.ABILITY_UNLOCK + 1, 1);
            LensManager.Instance.SetupLensMana(1);
        }
        PlayerObj.hiddenLensUnlocked = true; 
    }
    public void ActivateDash(bool firstTime = true)
    {
        if(firstTime)
            PlayerPrefs.SetInt(GameConstants.PlayerPrefConstants.ABILITY_UNLOCK + 2, 1);
        PlayerObj.dashUnlocked = true;
    }
    public void ActivateWallSlide(bool firstTime = true)
    {
        if (firstTime)
            PlayerPrefs.SetInt(GameConstants.PlayerPrefConstants.ABILITY_UNLOCK + 3, 1);
        PlayerObj.wallSlideUnlocked = true;
    }

    public void HealPlayer(float health) => PlayerObj.OnHeal(health);
    public enum PlayerControlScheme
    {
        Keyboard=0,
        Gamepad=1
    }

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        CurrentPlayerInput = PlayerObj.gameObject.GetComponent<PlayerInput>();

    }
    // Start is called before the first frame update
    void Start()
    {
        CheckForAbilities();
        SpawnPlayerOnStart();
    }

    private void CheckForAbilities()
    {
        if (PlayerPrefs.HasKey(GameConstants.PlayerPrefConstants.ABILITY_UNLOCK + 1))
            ActivateLens(false);
        if (PlayerPrefs.HasKey(GameConstants.PlayerPrefConstants.ABILITY_UNLOCK + 2))
            ActivateDash(false);
        if (PlayerPrefs.HasKey(GameConstants.PlayerPrefConstants.ABILITY_UNLOCK + 3))
            ActivateWallSlide(false);

    }
    public void OnSchemeChange()
    {
        if (!CurrentPlayerInput)
            return;

        CurrentPlayerControlScheme = CurrentPlayerInput.currentControlScheme switch
        {
            GameConstants.ControlSchemes.KEYBOARD => PlayerControlScheme.Keyboard,
            GameConstants.ControlSchemes.GAMEPAD => PlayerControlScheme.Gamepad,
            _ => PlayerControlScheme.Keyboard,

        };
    }
   
   
    public void SpawnPlayerOnStart()
    {
        PlayerObj.transform.position = RespawnManager.Instance.GetRespawnPos();
    }
    public void ResetGame()
    {
        PlayerObj.gameObject.SetActive(false);
        PlayerObj.transform.position = RespawnManager.Instance.GetRespawnPos();
        PlayerObj.gameObject.SetActive(true);
        PlayerObj.ResetPlayer();
        LensManager.Instance.ResetLens();
    }
    

   
}
