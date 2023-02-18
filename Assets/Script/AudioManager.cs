using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioManager instance;
    public Sound[] sounds;
    private bool isMutedMusic = false;
    private bool isMutedSound = false;
    // Start is called before the first frame update
    void Awake()
    {
        
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            //s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            Unmute(s);
        }

    }
    void Update() 
    {
        if(GameManger.Instance.isMutedMusic && !isMutedMusic)
        {
            Sound s = Array.Find(sounds, sound => sound.name == "GameLoopBeat");
            Mute(s);
            isMutedMusic = true;
        }
        if(GameManger.Instance.isMutedSound && !isMutedSound)
        {
            foreach(Sound s in sounds)
            {
                Mute(s);
                isMutedSound = true;
            }
        }   
        
        if(!GameManger.Instance.isMutedMusic && isMutedMusic)
        {
            Sound s = Array.Find(sounds, sound => sound.name == "GameLoopBeat");
            Unmute(s);
            isMutedMusic = false;
        }
        if(!GameManger.Instance.isMutedSound && isMutedSound)
        {
            foreach(Sound s in sounds)
            {
                if(s.name == "GameLoopBeat") 
                {
                    if(!GameManger.Instance.isMutedMusic)
                    {
                        Unmute(s);
                    }
                }
                else
                {
                    Unmute(s);
                    isMutedSound = false;
                }
                
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
    public void Unmute(Sound s)
    {
        s.source.volume = s.volume;
    }
    public void Mute(Sound s)
    {
        s.source.volume = 0;
    }
}
