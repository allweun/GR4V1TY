using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound currentSound in sounds){
            currentSound.source = gameObject.AddComponent<AudioSource>();
            currentSound.source.clip = currentSound.clip;
            currentSound.source.volume = currentSound.volume;
            currentSound.source.pitch = currentSound.pitch;
            currentSound.source.loop = currentSound.loop;
        }
        if(!PlayerPrefs.HasKey("MUSIC")){
            Play("music1");
            PlayerPrefs.SetInt("MUSIC", 1);            
        }else if(PlayerPrefs.GetInt("MUSIC")==1){
            Play("music1");
        }
    }

    public void Play(string name){
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if(currentSound == null){
            Debug.LogWarning("Sound not found");
            return;
        }
        currentSound.source.Play();
    }
    public void Silence(string name){
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if(currentSound ==null){
            Debug.LogWarning("Sound not found");
            return;
        }
        currentSound.source.volume = 0;
    }
    public void VolumeUp(string name){
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if(currentSound ==null){
            Debug.LogWarning("Sound not found");
            return;
        }
        currentSound.source.volume = 1;
    }
    public void Stop(string name){
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if(currentSound == null){
            Debug.LogWarning("Sound not found");
            return;
        }
        currentSound.source.Stop();
    }
}