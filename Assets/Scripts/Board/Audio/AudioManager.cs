using System.Collections;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioClip[] themeMusic;
    public AudioSource themeAudioSource;
    public String themeName;
    public bool mute;

    public static AudioManager instance;

    void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play (string name)
    {
        try
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
        }
        catch (NullReferenceException) { }
    }

    private void Start()
    {
        mute = false;
        playRandomTheme();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            playRandomTheme();
        }
        if (!themeAudioSource.isPlaying)
        {
            playRandomTheme();
        }
    }

    public void playRandomTheme()
    {
        int randClip = UnityEngine.Random.Range(0, themeMusic.Length);
        themeAudioSource.clip = themeMusic[randClip];
        themeAudioSource.Play();
        themeName = getThemeName();
    }

    public String getThemeName()
    {
        return themeAudioSource.clip.name;
    }

    public void muteMusic()
    {
        if (!mute)
        {
            themeAudioSource.mute = true;
            mute = true;
        }
        else
        {
            themeAudioSource.mute = false;
            mute = false;
        }
    }
}
