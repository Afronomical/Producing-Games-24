using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Camera camera;

    public GameObject sourcePrefab;
    public int numberOfEffectSources;

    public float globalVolume = 1;
    public float soundEffectVolume = 1;
    public float musicVolume = 1;

    private List<AudioSource> effectSources = new List<AudioSource>();
    private AudioSource musicSource, ambienceSource;

    [Header("Spacial Audio")]
    public float maxListenDistance;
    public AnimationCurve distanceFalloff;


    public SoundEffect testSound;


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

        SetupAudioSources();
    }



    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject obj = Instantiate(sourcePrefab);
            obj.transform.position = new Vector3(Random.Range(0, 50), Random.Range(0, 50), Random.Range(0, 50));
            PlaySound(testSound, obj.transform);
        }
    }



    private void SetupAudioSources()
    {
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        ambienceSource = transform.GetChild(1).GetComponent<AudioSource>();

        for (int i = 0; i < numberOfEffectSources; i++)
        {
            GameObject newSource = Instantiate(sourcePrefab);
            newSource.transform.SetParent(transform);
            newSource.name = "Effect Source (" + i + ")";
            effectSources.Add(newSource.GetComponent<AudioSource>());
        }
    }



    public void PlaySound(SoundEffect effect, Transform effectParent)
    {
        AudioSource source = musicSource;


        if (!effect.isMusic)
        {
            for (int i = 0; i < effectSources.Count; i++)
            {
                if (!effectSources[i].isPlaying)
                {
                    source = effectSources[i];
                    break;
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
        Debug.Log(source.name);

        SetMainSourceSettings(source, effect);

        if (effect.spacialAudio)
        {
            source.transform.position = effectParent.position;

            if (effect.followObject)
                source.transform.parent = effectParent;
            else
                source.transform.parent = transform;

            source.spatialBlend = 1;
            source.maxDistance = effect.travelDistance;
            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, distanceFalloff);
        }
        else
        {
            source.spatialBlend = 0;
            source.transform.parent = transform;
        }

        source.Play();
    }



    public void PlayMusic(SoundEffect effect)
    {
        AudioSource source = musicSource;

        SetMainSourceSettings(source, effect);

        source.Play();
    }


    public void PlayGlobalAmbience(SoundEffect effect)
    {
        AudioSource source = ambienceSource;

        SetMainSourceSettings(source, effect);

        source.Play();
    }



    public void StopSound(SoundEffect effect)
    {
        for (int i = 0; i < effectSources.Count; i++)  // Check through all effect sources
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



    public void StopGlobalAmbience(SoundEffect effect)
    {
        ambienceSource.Stop();
    }



    public void StopAllSounds()
    {
        for (int i = 0; i < effectSources.Count; i++)  // For every effect source
        {
            effectSources[i].Stop();  // Stop the sound
        }
    }



    private void SetMainSourceSettings(AudioSource source, SoundEffect effect)
    {
        source.clip = effect.audioClip;
        source.pitch = effect.pitch;
        source.priority = effect.priority;
        source.loop = effect.loop;

        float volume = effect.volume;
        volume *= globalVolume;
        volume *= effect.isMusic ? musicVolume : soundEffectVolume;
        source.volume = volume;
    }
}
