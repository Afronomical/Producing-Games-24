using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeartBeat : MonoBehaviour
{
    public SoundEffect heartBeatSound;
    public float activationRange = 10f;
    [Range(0.0f, 3f)] public float maxPitch;
    [Range(0.0f, 3f)] public float minPitch;
    public float volume = 1;
    public float heartFalloffTime = 3;
    private GameObject demonObj;
    private AudioSource newSound;
    private bool isPlaying;

    private DemonCharacter demonCharacter;
    // Start is called before the first frame update
    void Start()
    {
        //newSound = FindFirstObjectByType<AudioSource>(AudioClip.name.Equals(heartBeatSound.name));
        if (GameManager.Instance.demon != null)
        {
            demonObj = GameManager.Instance.demon;
            demonCharacter = demonObj.GetComponent<DemonCharacter>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (demonObj == null)
        {
            demonObj = GameManager.Instance.demon;
            demonCharacter = demonObj.GetComponent<DemonCharacter>();
        }

        if (demonObj != null && demonCharacter != null)
            activateHeart();
    }

    public void HeartTime()
    {

    }

    public IEnumerator HeartRadius()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            Debug.Log("played");
            float pitchTemporary;
            AudioManager.instance.PlaySound(heartBeatSound, transform);
            AudioManager.instance.ChangeVolume(heartBeatSound, volume);

            while (demonDist() < activationRange)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                pitchTemporary = Mathf.Lerp(maxPitch, minPitch, demonDist() / activationRange);
                AudioManager.instance.ChangePitch(heartBeatSound, pitchTemporary);
            }
            if (demonDist() > activationRange)
            {
                StartCoroutine(HeartBeatStop());
            }
            
        }
        

    }


    IEnumerator HeartBeatStop()
    {
        //yield return new WaitForSeconds(heartFalloffTime);
        Debug.Log("heartstopped");
        float time = heartFalloffTime;
        while (time > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            time -= Time.deltaTime;
            AudioManager.instance.ChangeVolume(heartBeatSound, volume >= 0 ? volume - Time.deltaTime : 0);
        }
        AudioManager.instance.StopSound(heartBeatSound);
        isPlaying = false;
    }

    float demonDist()
    {
        return Vector3.Distance(demonObj.transform.position, transform.position);
    }

    private void activateHeart()
    {
        if (!isPlaying && demonDist() < activationRange && demonCharacter.currentState != DemonCharacter.DemonStates.Inactive)
        {
            
            StartCoroutine(HeartRadius());
            
        }
    }

    
}
