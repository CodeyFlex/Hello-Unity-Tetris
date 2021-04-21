using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance = null; //So there can only ever be one SoundManager

    //Sound effect references
    public AudioClip rotateLeftSound;
    public AudioClip rotateRightSound;
    public AudioClip shapeMoveSound;
    public AudioClip gameOverSound;
    public AudioClip tetrisClearSound;
    public AudioClip shapeLandSound;

    private AudioSource soundEffectAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton rule, destroys old sound manager if there
        if (Instance == null)
        {
            Instance = this;
        }else if (Instance != this)
        {
            Destroy(gameObject);
        }

        AudioSource theSource = GetComponent<AudioSource>();

        soundEffectAudio = theSource;

    }

    //Other game objects call this method to play sounds
    public void PlayOneShot(AudioClip clip)
    {
        soundEffectAudio.PlayOneShot(clip);
    }
}
