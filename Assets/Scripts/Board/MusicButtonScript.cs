using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButtonScript : MonoBehaviour {

    public AudioSource src;
    private bool musicPlay = true;

    public void PauseMusic()
    {
        src.Pause();
        musicPlay = false;
    }
    public void PlayMusic()
    {
        if (!musicPlay)
        {
            src.Play();
            musicPlay = true;
        }
    }
}
