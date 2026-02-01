using System;
using MonkeyJam.Managers;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        EventManager.Instance.OnSoundRequested += PlaySoundFXClip;
    }
    
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource, audioSource.clip.length);
    }

    private void OnDisable()
    {
        EventManager.Instance.OnSoundRequested -= PlaySoundFXClip;
    }
}
