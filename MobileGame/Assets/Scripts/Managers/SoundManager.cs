using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        instance = this;

        foreach(var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play(string soundName)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        if (sound == null)
            return;
        sound.source.Play();
    }
}
