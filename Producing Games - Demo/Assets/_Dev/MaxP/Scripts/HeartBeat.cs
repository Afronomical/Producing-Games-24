using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeartBeat : MonoBehaviour
{
    public SoundEffect heartBeatSound;
    [Range(0.0f, 3f)] public float maxPitch;
    [Range(0.0f, 3f)] public float minPitch;
    public float heartFalloffTime = 3;
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
        float pitchTemporary;
        float maxDistance = demonDist();

        while (demonDist() < maxDistance)
        {
            AudioManager.instance.PlaySound(heartBeatSound, transform);
            pitchTemporary = Mathf.Lerp(minPitch, maxPitch, demonDist() / maxDistance);
        }

    }


    IEnumerator HeartBeatStop()
    {
        yield return new WaitForSeconds(heartFalloffTime);
        float time = heartFalloffTime / 2;
        while (time > 0)
        {
            time -= Time.deltaTime;
        }
    }

    float demonDist()
    {
        return Vector3.Distance(demonObj.transform.position, transform.position);
    }

}
