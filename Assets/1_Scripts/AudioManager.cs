using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("SFX Settings")]
    public int poolSize = 10;

    [Header("SFX Clips")]
    public List<AudioClip> sfxClips = new List<AudioClip>();
    public List<AudioSource> sfxSources = new List<AudioSource>();

    private Dictionary<string, AudioClip> sfxDictionary;
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SetupDictionary();
        InitPool();
    }

    void SetupDictionary()
    {
        sfxDictionary = new Dictionary<string, AudioClip>();

        foreach (var clip in sfxClips)
        {
            if (clip != null && !sfxDictionary.ContainsKey(clip.name))
                sfxDictionary.Add(clip.name, clip);
        }
    }

    void InitPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new GameObject($"SFX_AudioSource_{i}");
            obj.transform.parent = transform;
            
            var source = obj.AddComponent<AudioSource>();
            
            source.playOnAwake = false;
            source.spatialBlend = 1f; // 3D sound
            source.rolloffMode = AudioRolloffMode.Logarithmic;
                
            sfxSources.Add(source);
        }
    }

    public void PlaySFX(string clipName, Vector3 position, float pitch = 1f, float volume = 1f)
    {
        if (!sfxDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            Debug.LogWarning($"Audio '{clipName}' not found in dictionary!");
            return;
        }

        AudioSource availableSource = null;

        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
            {
                availableSource = source;
                break;
            }
        }

        if (availableSource == null)
            availableSource = sfxSources[1];

        availableSource.clip = clip;
        availableSource.transform.position = position;
        availableSource.pitch = pitch;
        availableSource.volume = volume;
        availableSource.Play();
    }
}
