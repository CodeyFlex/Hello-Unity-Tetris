using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicManager : MonoBehaviour
{
    public static BGMusicManager Instance = null; //So there can only ever be one BGMusicManager

    //Sound effect references
    public AudioClip gameOverSound;
    public AudioClip bgMusic;

    private AudioSource bgMusicAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Singleton rule, destroys old sound manager if there
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        AudioSource theSource = GetComponent<AudioSource>();

        bgMusicAudio = theSource;

        BGMusicManager.Instance.PlayOneShot(BGMusicManager.Instance.bgMusic); //BG Music
    }

    //Other game objects call this method to play sounds
    public void PlayOneShot(AudioClip clip)
    {
        bgMusicAudio.PlayOneShot(clip);
    }
}
