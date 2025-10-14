using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerMainScript PlayerObj;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
     
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
