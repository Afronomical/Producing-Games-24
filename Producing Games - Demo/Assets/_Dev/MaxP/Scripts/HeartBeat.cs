using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    public SoundEffect heartBeatSound;

    private GameObject demonObj;
    private AudioSource newSound;
    private bool isPlaying;
    // Start is called before the first frame update
    void Start()
    {

        //newSound = FindFirstObjectByType<AudioSource>(AudioClip.name.Equals(heartBeatSound.name));
        demonObj = GameManager.Instance.demon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HeartTime()
    {

    }

    public void HeartRadius()
    {
        float pitchTemp;
        if (!isPlaying)
        {
            AudioManager.instance.PlaySound(heartBeatSound, transform);

        }
        isPlaying = true;
    }

    //float demonDist()
    //{

    //}
}
