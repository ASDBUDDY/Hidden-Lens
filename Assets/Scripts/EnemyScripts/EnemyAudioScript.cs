using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioScript : MonoBehaviour
{
    private EnemyTypeEnum enemyType;

    public SoundDataSO SoundDataCenter;

    public AudioSource EnemyAudioSource;

    private List<EnemyAudioData> audioData;

    private void Start()
    {
        SetupAudioSystem();
    }
    private void SetupAudioSystem()
    {
        enemyType = GetComponent<EnemyBaseScript>().EnemyType;

        audioData =  SoundDataCenter.GetEnemyAudioDatas(enemyType);
    }

    private void PlayEnemySound(AudioClip clip)
    {
        EnemyAudioSource.clip = clip;
        EnemyAudioSource.Play();
    }

    private AudioClip GetAudioClip(EnemyAudioData.EnemyAudioClips clipName)
    {
        foreach (var item in audioData)
        {
            if (item.AudioName == clipName)
                return item.Audio;
        }

        return audioData[0].Audio;
    }

    #region SoundCall Functions
    public void PlayIdle() => PlayEnemySound(GetAudioClip(EnemyAudioData.EnemyAudioClips.Idle));
   

    public void PlayDamage() => PlayEnemySound(GetAudioClip(EnemyAudioData.EnemyAudioClips.Damage));

    public void PlayAttack() => PlayEnemySound(GetAudioClip(EnemyAudioData.EnemyAudioClips.Attack));

    public void PlayHeal() => PlayEnemySound(GetAudioClip(EnemyAudioData.EnemyAudioClips.Heal));

    public void PlayDeath() => PlayEnemySound(GetAudioClip(EnemyAudioData.EnemyAudioClips.Death));

    #endregion

}
