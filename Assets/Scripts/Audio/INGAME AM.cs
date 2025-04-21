using UnityEngine;

public class INGAMEAM : MonoBehaviour
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
    public AudioClip enemyDamage;

    private void Start()
    {
        musicSource.clip = bgIngame;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
