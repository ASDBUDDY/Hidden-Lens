using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects / SoundDataSO")]

public class SoundDataSO : ScriptableObject
{
    public List<PlayerAudioData> PlayerAudioDataList;

    public List<EnemyAudioType> EnemyAudioDataList;


    public List<EnemyAudioData> GetEnemyAudioDatas(EnemyTypeEnum enemyType)
    {
        foreach(var item in EnemyAudioDataList)
        {
            if (item.enemyType == enemyType)
                return item.enemyAudioDatas;
        }
        return EnemyAudioDataList[0].enemyAudioDatas;
    }

    public AudioClip GetPlayerAudioClip(PlayerAudioData.PlayerAudioClips audioName)
    {
        foreach (var item in PlayerAudioDataList)
        {
            if (audioName == item.AudioName)
            {
                return item.Audio;
            }
        }
        return PlayerAudioDataList[0].Audio;
    }
}
[System.Serializable]
public class PlayerAudioData
{
    public PlayerAudioClips AudioName;

    public AudioClip Audio;
    public enum PlayerAudioClips
    {
        FootstepOne = 0,
        FootstepTwo = 1,
        FootstepThree = 2,
        FootstepFour = 3,
        SwordSlashOne = 4,
        SwordSlashTwo = 5,
        SwordSlashThree = 6,
        JumpOne = 7,
        JumpTwo = 8,
        JumpThree = 9,
        DamageOne = 10,
        DamageTwo = 11,
        DamageThree = 12,
        AttackVoiceOne = 13,
        AttackVoiceTwo = 14,
        AttackVoiceThree = 15,
        CrouchVoice = 16,
        LandingVoice = 17,
        DashSFX = 18,

    }
}

[System.Serializable]
public class EnemyAudioData
{
    public EnemyAudioClips AudioName;

    public AudioClip Audio;

    public enum EnemyAudioClips
    {
        Idle= 0,
        Damage = 1,
        Attack = 2,
        Death = 3,
        Heal = 4,
    }
}

[System.Serializable]
public class EnemyAudioType
{
    public EnemyTypeEnum enemyType;

    public List<EnemyAudioData> enemyAudioDatas;
}