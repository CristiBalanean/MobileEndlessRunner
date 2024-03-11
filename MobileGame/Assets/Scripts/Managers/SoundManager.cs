using UnityEngine.Audio;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private Sound[] sounds;

    [SerializeField] private AudioMixerGroup soundsMixer;

    private void Awake()
    {
        instance = this;

        foreach(var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = sound.audioMixerGroup;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
        }
    }

    public void Play(string soundName)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        if (sound == null)
            return;
        sound.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == soundName);
        if (sound == null)
            return;
        sound.source.Stop();
    }

    public void SlowSounds()
    {
        foreach (var sound in sounds)
            if (sound.audioMixerGroup == soundsMixer && sound.name != "Slowdown")
                sound.source.enabled = false;
    }

    public void NormalSounds()
    {
        foreach (var sound in sounds)
            if (sound.audioMixerGroup == soundsMixer)
                sound.source.enabled = true;
    }
}
