using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; 

    public AudioSource backgroundAudioSource; // Fonte para música de fundo
    public AudioSource effectsAudioSource; // Fonte para efeitos sonoros
    public AudioSource warningAudioSource; // Fonte dedicada para som de aviso
    public AudioSource computerAudioSource;
    public AudioClip backgroundMusic;
    public AudioClip computerSound;
    public AudioClip pickupSound;
    public AudioClip winSound;
    public AudioClip warningSound;
    public AudioClip dieSound;

    public bool isWarningPlaying = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Não destruir entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        backgroundAudioSource.clip = backgroundMusic;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    public void PlayPickupSound()
    {
        effectsAudioSource.PlayOneShot(pickupSound);
    }

    public void PlayWinSound()
    {       
        effectsAudioSource.PlayOneShot(winSound);       
    }

    public void PlayDieSound()
    {
        effectsAudioSource.PlayOneShot(dieSound);
    }

    public void PlayComputerSound()
    {
        computerAudioSource.clip = computerSound;
        computerAudioSource.loop = true;
        computerAudioSource.Play();
    }

    public void PlayWarningSound()
    {
        if (!isWarningPlaying)
        {
            warningAudioSource.clip = warningSound;
            warningAudioSource.loop = true;
            warningAudioSource.Play();
            isWarningPlaying = true;
        }
    }

    public void StopWarningSound()
    {
        if (isWarningPlaying)
        {
            warningAudioSource.Stop();
            isWarningPlaying = false;
        }
    }

    public void StopBackgroundMusic()
    {
        backgroundAudioSource.Stop();
    }

    public void StopComputerSound()
    {
        computerAudioSource.Stop();
    }
}
