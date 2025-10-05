using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour
{
    public AudioSource VoiceSource;
    public List<AudioSource> SFXSources;

    
    public SoundDataSO SoundData;

    private PlayerAudioData.PlayerAudioClips footstepCounter = PlayerAudioData.PlayerAudioClips.FootstepOne;
    private int jumpCounter = (int)PlayerAudioData.PlayerAudioClips.JumpOne;
    private int damageCounter = (int)PlayerAudioData.PlayerAudioClips.DamageOne;
    private List<PlayerAudioData.PlayerAudioClips> Footsteps = new List<PlayerAudioData.PlayerAudioClips> { PlayerAudioData.PlayerAudioClips.FootstepOne, PlayerAudioData.PlayerAudioClips.FootstepTwo, PlayerAudioData.PlayerAudioClips.FootstepThree, PlayerAudioData.PlayerAudioClips.FootstepFour };

    #region Internal Functions
    
    private void PlayOnSFX(AudioClip clip)
    {
        foreach (var item in SFXSources)
        {
            if (item.isPlaying)
                continue;

            item.clip = clip;
            item.Play();
            break;
        }
    }

    private void PlayOnVoice(AudioClip clip)
    {
        VoiceSource.clip = clip;
        VoiceSource.Play();
    }
    #endregion

    #region Public Functions

    public void OnVolumeChange(bool flag)
    {
        float volume = SoundManager.Instance.GetIngameVol;

        VoiceSource.volume = volume;
        
        foreach(var item in SFXSources)
        {
            item.volume = volume;
        }
    }

    #endregion

    #region SoundCall Functions
    public void PlayFootsteps()
    {
        
        PlayOnSFX(SoundData.GetPlayerAudioClip(footstepCounter));

        List<PlayerAudioData.PlayerAudioClips> newFootsteps = new List<PlayerAudioData.PlayerAudioClips>(Footsteps);
    
        newFootsteps.Remove(footstepCounter);

        int location = Random.Range(0, newFootsteps.Count);

        footstepCounter = newFootsteps[location];
    }

    public void PlaySwordSound(int soundId)
    {
        PlayerAudioData.PlayerAudioClips clipName = soundId switch
        {
            0 => PlayerAudioData.PlayerAudioClips.SwordSlashOne,
            1=> PlayerAudioData.PlayerAudioClips.SwordSlashTwo,
            2=> PlayerAudioData.PlayerAudioClips.SwordSlashThree,
            _=> PlayerAudioData.PlayerAudioClips.SwordSlashOne
        };

        PlayOnSFX(SoundData.GetPlayerAudioClip(clipName));

    }

    public void PlayAttackVoice(int voiceId)
    {
        PlayerAudioData.PlayerAudioClips clipName = voiceId switch
        {
            0 => PlayerAudioData.PlayerAudioClips.AttackVoiceOne,
            1 => PlayerAudioData.PlayerAudioClips.AttackVoiceTwo,
            2 => PlayerAudioData.PlayerAudioClips.AttackVoiceThree,
            _ => PlayerAudioData.PlayerAudioClips.AttackVoiceOne
        };

       PlayOnVoice(SoundData.GetPlayerAudioClip(clipName));
       
    }

    public void PlayJumpSound()
    {
        PlayOnVoice(SoundData.GetPlayerAudioClip((PlayerAudioData.PlayerAudioClips)jumpCounter));

        jumpCounter++;

        if (jumpCounter > (int)PlayerAudioData.PlayerAudioClips.JumpThree)
            jumpCounter = (int)PlayerAudioData.PlayerAudioClips.JumpOne;
    }
    public void PlayDamageSound()
    {
        PlayOnVoice(SoundData.GetPlayerAudioClip((PlayerAudioData.PlayerAudioClips)damageCounter));

        damageCounter++;

        if (damageCounter > (int)PlayerAudioData.PlayerAudioClips.DamageThree)
            damageCounter = (int)PlayerAudioData.PlayerAudioClips.DamageOne;
    }

    public void PlayLanding() => PlayOnVoice(SoundData.GetPlayerAudioClip(PlayerAudioData.PlayerAudioClips.LandingVoice));

    public void PlayCrouch() => PlayOnVoice(SoundData.GetPlayerAudioClip(PlayerAudioData.PlayerAudioClips.CrouchVoice));

    #endregion
}


