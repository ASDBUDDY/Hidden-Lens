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
    public enum PlayerControlScheme
    {
        Keyboard=0,
        Gamepad=1
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
   
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        CurrentPlayerInput = PlayerObj.gameObject.GetComponent<PlayerInput>();
     
    }

    public void ResetGame()
    {
        PlayerObj.gameObject.SetActive(false);
        PlayerObj.transform.position = RespawnManager.Instance.GetRespawnPos();
        PlayerObj.gameObject.SetActive(true);
        PlayerObj.ResetPlayer();
        LensManager.Instance.ResetLens();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
