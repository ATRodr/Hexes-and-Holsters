using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource soundFxObject;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // spawn in game object
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);

        // assign audio clip
        audioSource.clip = audioClip;

        // set volume
        audioSource.volume = volume;

        // play sound
        audioSource.Play();

        // get length of audio clip
        float clipLength = audioSource.clip.length;

        // destroy game object after clip length
        Destroy(audioSource.gameObject, clipLength);
    }
}
