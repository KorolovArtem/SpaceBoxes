using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip gameOverSound;   
    public AudioClip placeBlockSound; 

    private bool isMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip, float volume)
    {
        if (musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.volume = volume;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PlayGameOverSound()
    {
        if (sfxSource != null && gameOverSound != null)
        {
            sfxSource.PlayOneShot(gameOverSound);
        }
    }

    public void PlayPlaceBlockSound()
    {
        if (sfxSource != null && placeBlockSound != null)
        {
            sfxSource.PlayOneShot(placeBlockSound);
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void PauseAllSounds()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
        if (sfxSource != null)
        {
            sfxSource.Pause();
        }
    }
    public void ResumeAllSounds()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
        if (sfxSource != null)
        {
            sfxSource.UnPause();
        }
    }
}