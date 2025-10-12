using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;
    private int SpawnID = 0;

    public List<RespawnPoint> RespawnList;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        CheckForRespawns();
    }

    private void CheckForRespawns()
    {
        if (PlayerPrefs.HasKey(GameConstants.PlayerPrefConstants.RESPAWN_POINT))
        {
            SpawnID = PlayerPrefs.GetInt(GameConstants.PlayerPrefConstants.RESPAWN_POINT);
        }
        else
        {
            PlayerPrefs.SetInt(GameConstants.PlayerPrefConstants.RESPAWN_POINT, 0);
        }

       for(int i = 0; i < RespawnList.Count; i++)
       {
            RespawnList[i].TogglePoint(i<=SpawnID);
       }
    }

    public Vector3 GetRespawnPos()
    {
        SpawnID = PlayerPrefs.GetInt(GameConstants.PlayerPrefConstants.RESPAWN_POINT,0);

        return RespawnList[SpawnID].gameObject.transform.position;
    }

    public void UpdateSpawn(RespawnPoint respawnPoint)
    {
        for(int i = 0;i < RespawnList.Count; i++)
        {
            if (RespawnList[i] == respawnPoint)
            {
                SpawnID = i;
                PlayerPrefs.SetInt(GameConstants.PlayerPrefConstants.RESPAWN_POINT, SpawnID);
                return;
            }
        }
    }
}
