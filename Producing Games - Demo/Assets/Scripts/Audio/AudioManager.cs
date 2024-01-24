using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Camera camera;

    public float globalVolume = 1;
    public float soundEffectVolume = 1;
    public float musicVolume = 1;

    private List<AudioSource> effectSources;
    private AudioSource musicSource;

    [Header("Spacial Audio")]
    public float maxListenDistance;
    public AnimationCurve distanceFalloff;



    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);



        camera = Camera.main;



        int i = 0;
        foreach (Transform child in transform)
        {
            if (i == 0)  
                musicSource = child.GetComponent<AudioSource>();  // Set the music source
            else
                effectSources.Add(child.GetComponent<AudioSource>());  // Add the effect sources
            Debug.Log(i);
            i++;
        }
    }



    public void PlaySound(SoundEffect effect, Vector3 effectPosition)
    {
        AudioSource source = musicSource;

        if (!effect.isMusic)
        {
            for (int i = 0; i < effectSources.Count; i++)
            {
                if (!effectSources[i].isPlaying)
                {
                    source = effectSources[i];
                }
            }

            if (source == musicSource)  // If it couldn't find an empty source
            {
                AudioSource lowestPrioritySource = effectSources[0];
                for (int i = 0; i < effectSources.Count; i++)  // Find the active source with the lowest priority
                {
                    if (effectSources[i].priority < lowestPrioritySource.priority)
                    {
                        lowestPrioritySource = effectSources[i];
                    }
                }

                Debug.Log("Sound effect stopped due to not having enough free sources, Source: " + lowestPrioritySource.name + " Priority: " + lowestPrioritySource.priority);
            }
        }



        float volume = effect.volume;
        volume *= globalVolume;
        volume *= effect.isMusic ? musicVolume : soundEffectVolume;

        source.volume = volume;
        source.clip = effect.audioClip;
        source.pitch = effect.pitch;
        source.priority = effect.priority;
        source.loop = effect.loop;


        if (effect.spacialAudio)
        {
            source.transform.position = effectPosition;
            source.spatialBlend = 1;
            source.maxDistance = maxListenDistance;
            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, distanceFalloff);
        }
        else
            source.spatialBlend = 0;
    }



    public void PlayMusic(SoundEffect effect)
    {
        AudioSource source = musicSource;

        float volume = effect.volume;
        volume *= globalVolume;
        volume *= musicVolume;

        source.volume = volume;
        source.clip = effect.audioClip;
        source.pitch = effect.pitch;
        source.priority = effect.priority;
        source.loop = effect.loop;
    }



    public void StopSound(SoundEffect effect)
    {
        for (int i = 0; i < effectSources.Count; i++)
        {
            if (effectSources[i].clip == effect.audioClip)  // Find the source playing this clip
            {
                effectSources[i].Stop();
            }
        }
    }



    public void StopMusic(SoundEffect effect)
    {
        musicSource.Stop();
    }



    public void StopAllSounds()
    {
        for (int i = 0; i < effectSources.Count; i++)  // For every effect source
        {
            effectSources[i].Stop();  // Stop the sound
        }
    }



    public float GetSoundDistance(Vector3 effectPosition)
    {
        return Vector3.Distance(camera.transform.position, effectPosition);
    }
}
