using UnityEngine;

public class AudioManager : MonoBehaviour
{


    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip portal;
    public AudioClip walk;
    public AudioClip sword;
    public AudioClip bgIngame;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
