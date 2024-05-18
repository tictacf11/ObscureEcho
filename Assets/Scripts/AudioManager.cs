using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioClip flipSound;
    public AudioClip matchSound;
    public AudioClip mismatchSound;
    public AudioClip gameOverSound;

    [SerializeField] int audioSourcePoolSize = 4; 
    private List<AudioSource> audioSourcePool;
    private bool isMuted = false;
    public bool IsMuted => isMuted;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSourcePool = new List<AudioSource>();
            for (int i = 0; i < audioSourcePoolSize; i++)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSourcePool.Add(audioSource);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (!isMuted && clip != null)
        {
            AudioSource availableSource = GetAvailableAudioSource();
            if (availableSource != null)
            {
                availableSource.PlayOneShot(clip);
            }
        }
    }

    public void Mute()
    {
        isMuted = true;
        foreach (AudioSource source in audioSourcePool) source.Stop();
    }

    public void Unmute() => isMuted = false;

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        AudioSource audioSource = audioSourcePool[0];
        audioSourcePool.RemoveAt(0);
        audioSourcePool.Add(audioSource);
        return audioSource;
    }
}
