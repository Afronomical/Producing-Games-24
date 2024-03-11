using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays a random sound in range of the player
/// </summary>

public class ParanoiaSound : MonoBehaviour
{
    public SoundEffect[] sounds;
    public int chanceOfActive = 100;
    public float distanceMax;
    public float distanceMin;
    public bool shouldPlay = true;
    private int chance;
    private float mainDistance;
    private float distanceDelta;
    private GameObject playerObj;
    private float randX;
    private float randZ;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKey(KeyCode.M))
        //{
        //    ChanceForEvent();
        //}
        if (shouldPlay)
        {
            ChanceForEvent();
        }
        
    }

    void ChanceForEvent()
    {
        chance = Random.Range(0, chanceOfActive);
        if (chance == 1)
        {
            PlaySoundAtRandLoc();
        }
    }

    void PlaySoundAtRandLoc()
    {
        mainDistance = Random.Range(distanceMin, distanceMax);
        randX = mainDistance - Random.Range(0, mainDistance);
        randZ = mainDistance - randX;
        randX *= Random.Range(0, 2) * 2 - 1;
        randZ *= Random.Range(0, 2) * 2 - 1;

        transform.position = new Vector3(randX, 0, randZ) + playerObj.transform.position;
        AudioManager.instance.PlaySound(sounds[Random.Range(0, sounds.Length)], transform);
    }
}
