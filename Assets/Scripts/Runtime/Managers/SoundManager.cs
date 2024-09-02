using System.Collections;
using System.Collections.Generic;
using Runtime.Data.UnityObject;
using Runtime.Enums;
using Runtime.Extensions;
using Runtime.Managers;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [Header("Default Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Glissando Audio Source")]
    [SerializeField] private AudioSource glissandoAudioSource;

    [Header("Random Pitch Audio Source")] 
    [SerializeField] private AudioSource randomPitchAudioSource;

    [Header("Sound's")]
    [SerializeField] private CD_GameSound COLLECTION;

    [Header("Glissando Settings")]
    [SerializeField] private float glissandoPitchRange = 2f;
    [SerializeField] private float glissandoDuration = 1f;
    [SerializeField] private float glissandoDefaultPitch = 1f;
    [SerializeField] private Coroutine glissandoCoroutine;

    public void PlaySound(GameSoundType soundType)
    {
        if (SettingsManager.Instance.isSoundActive)
        {
            foreach (var sound in COLLECTION.gameSoundData)
            {
                if (soundType == sound.gameSoundType)
                {
                    if (sound.hasRandomPitch)
                    {
                        randomPitchAudioSource.pitch = Random.Range(0.8f, 1.2f);
                        randomPitchAudioSource.PlayOneShot(sound.audioClip);
                        break;
                    }
                    else if (sound.hasGlissando)
                    {
                        if (glissandoCoroutine != null)
                        {
                            StopCoroutine(glissandoCoroutine);
                        }
                        glissandoCoroutine = StartCoroutine(PlayGlissando(sound.audioClip));
                        break;
                    }
                    else
                    {
                        audioSource.PlayOneShot(sound.audioClip);
                        break;   
                    }
                }
            }
        }
    }
    
    private IEnumerator PlayGlissando(AudioClip clip)
    {
        float elapsedTime = 0f;
        float initialPitch = glissandoAudioSource.pitch;

        glissandoAudioSource.PlayOneShot(clip);

        while (elapsedTime < glissandoDuration)
        {
            float t = elapsedTime / glissandoDuration;
            glissandoAudioSource.pitch = Mathf.Lerp(initialPitch, initialPitch + glissandoPitchRange, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        glissandoAudioSource.pitch = glissandoDefaultPitch;
    }
}
