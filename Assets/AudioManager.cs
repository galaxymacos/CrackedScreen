using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Sound[] Musics;

    public Sound[] BossSounds;
    // Start is called before the first frame update

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
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
        
        foreach (Sound s in Musics)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in BossSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        PlayMusic("ChapterOneBgm");
    }

    public void PlaySfx(string soundName)
    {
        foreach (var sound in sounds)
        {
            if (sound != null)
            {
                if (sound.name == soundName)
                {
                    if (sound.source.loop && !sound.source.isPlaying)
                    {
                        sound.source.Play();
                    }

                    if (!sound.source.loop && !sound.source.isPlaying)
                    {
                        sound.source.Play();
                    }
                }
                else
                {
                    if (sound.loop)
                    {
                        sound.source.Stop();                        
                    }
                    else
                    {
                        // do nothing (let the sound stops itself)
                    }
                }
        
            }
        }
    }
    
    public void PlayMusic(string soundName)
    {
        foreach (var sound in Musics)
        {
            if (sound != null)
            {
                if (sound.name == soundName)
                {
                    if (!sound.source.isPlaying)
                    {
                        sound.source.Play();                        
                    }
                }
                else
                {
                    if (sound.loop)
                    {
                        sound.source.Stop();                        
                    }
                    else
                    {
                        // do nothing (let the sound stops itself)
                    }
                }
        
            }
        }
    }
    
    public void PlayBossSound(string soundName)
    {
        foreach (var sound in BossSounds)
        {
            if (sound != null)
            {
                if (sound.name == soundName)
                {
                    if (!sound.source.isPlaying)
                    {
                        sound.source.Play();                        
                    }
                }
                else
                {
                    if (sound.loop)
                    {
                        sound.source.Stop();                        
                    }
                    else
                    {
                        // do nothing (let the sound stops itself)
                    }
                }
        
            }
        }
    }

    public void StopAllSfx()
    {
        foreach (Sound sound in sounds)
        {
            if (sound != null)
            {
                if (sound.loop)
                {
                    sound.source.Stop();                    
                }
            }
        }
    }

    public void ForceStopAllSfx()
    {
        foreach (Sound sound in sounds)
        {
            sound?.source.Stop();
        }
    }

    public void StopSfx(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound != null && sound.name == name)
            {
                sound.source.Stop();
            }
        }
    }
}
