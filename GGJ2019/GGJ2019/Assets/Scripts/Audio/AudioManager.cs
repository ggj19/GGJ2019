using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class AudioManager : AdvSingletonComponent<AudioManager>
{
    [Header("Component")]
    [SerializeField] int maxSoundCount = 30;

    [Header("Pitch")]
    [SerializeField] float minPitch = 0.95f;
    [SerializeField] float maxPitch = 1.05f;

    [SerializeField] Dictionary<string, AudioClip> clips;

    public float musicVolume
    {
        get { return PlayerPrefs.GetFloat("Music Volume", 1); }
        set
        {
            PlayerPrefs.SetFloat("Music Volume", value);
            music.volume = value;
        }
    }

    public float soundVolume
    {
        get { return PlayerPrefs.GetFloat("Sound Volume", 1); }
        set
        {
            PlayerPrefs.SetFloat("Sound Volume", value);
            foreach (var sound in sounds)
                sound.volume = value;
        }
    }

    public bool useVibrate
    {
        get { return PlayerPrefs.GetInt("Vibrate ", 1) == 1; }
        set { PlayerPrefs.SetInt("Vibrate", value ? 1 : 0); }
    }

    private AudioSource music;
    private AudioSource[] sounds;

    protected override void Initialize()
    {
        base.Initialize();

        // Music
        music = new GameObject().AddComponent<AudioSource>();
        music.name = "Music";
        music.transform.SetParent(transform);
        music.loop = true;

        // Sound
        var sound = new GameObject();
        sound.transform.SetParent(transform);
        sound.name = "Sound";

        sounds = new AudioSource[maxSoundCount];
        for (int i = 0; i < maxSoundCount; i++)
            sounds[i] = sound.AddComponent<AudioSource>();

        musicVolume = musicVolume;
        soundVolume = soundVolume;
    }

    public static void SetMusicVolume(float musicVolume) { instance.musicVolume = musicVolume; }
    public static void SetSoundVolume(float soundVolume) { instance.soundVolume = soundVolume; }

    public static void PlaySound(string clipName)
    {
        if (!instance.clips.ContainsKey(clipName))
        {
            Debug.LogError("Not Contains Clip " + clipName);
            return;
        }
        var clip = instance.clips[clipName];
        PlaySound(clip);
    }

    // 원형
    public static void PlaySound(AudioClip clip) { instance.PlaySound_(clip, instance.soundVolume); }

    public static void PlaySound(AudioClip clip, float volume) { instance.PlaySound_(clip, volume); }
    public void PlaySound_(AudioClip clip, float volume)
    {
        AudioSource source = FindIdleSoundSource();
        if (source == null)
            return;

        source.volume = volume;
        source.clip = clip;
        source.pitch = Random.Range(minPitch, maxPitch);

        source.Play();
    }

    private Dictionary<string, AudioSource> loopSounds = new Dictionary<string, AudioSource>();

    public static void PlayLoopSound(AudioClip clip, string id) { instance.PlayLoopSound_(clip, id); }
    private void PlayLoopSound_(AudioClip clip, string id)
    {
        if (loopSounds.ContainsKey(id))
            return;

        AudioSource source = FindIdleSoundSource();
        if (source == null)
            return;

        source.clip = clip;
        source.pitch = Random.Range(minPitch, maxPitch);
        source.loop = true;

        source.Play();

        loopSounds.Add(id, source);
    }

    public static void StopLoopSound(string id)
    {
        if (isDestroyed)
            return;
        instance.StopLoopSound_(id);
    }

    private void StopLoopSound_(string id)
    {
        if (!loopSounds.ContainsKey(id))
            return;

        loopSounds[id].loop = false;
        loopSounds[id].Stop();
        loopSounds.Remove(id);
    }

    private AudioSource FindIdleSoundSource()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].isPlaying)
                continue;
            return sounds[i];
        }
        return null;
    }

    public static void PlayMusic(AudioClip clip, float fadingTime = 0.3f) { instance.PlayMusic_(clip, fadingTime); }
    public static void PlayMusic(string clipName, float fadingTime = 0.3f)
    {
        var clip = instance.clips[clipName];
        if (instance.music.clip == clip)
            return;
        PlayMusic(clip, fadingTime);
    }

    public void PlayMusic_(AudioClip clip, float fadingTime = 0.3f)
    {
        float half = fadingTime / 2;
        music.clip = clip;
        //var seq = DOTween.Sequence();

        //var fadeOut = DOTween.To(
        //    () => music.volume,
        //    v => music.volume = v,
        //    0, half
        //);

        //fadeOut.OnPlay(() =>
        //{
        //    instance.music.clip = clip;
        //    instance.music.Play();
        //});

        //var fadeIn = DOTween.To(
        //    () => music.volume,
        //    v => music.volume = v,
        //    musicVolume, half
        //);

        //fadeOut.OnComplete(() => music.clip = clip);
        //seq.Append(fadeOut);
        //seq.Append(fadeIn);
    }

}