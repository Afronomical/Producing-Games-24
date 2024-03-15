using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Plays a random sound in range of the player
/// </summary>

public class ParanoiaSound : MonoBehaviour
{
    //public int minChance;
    //public int maxChance;

    public SoundEffect soundTest;
    
    public SoundEffect[] sounds;
    public float distanceMin;
    public float distanceMax;
    public bool shouldPlay = true;
    public float cooldownTimeMin = 10;
    public float cooldownTimeMax = 30;
    private float mainDistance;
    private GameObject playerObj;
    private float randX;
    private float randZ;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameManager.Instance.player;
        if (shouldPlay)
        {
            StartCoroutine(Cooldown());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //AudioManager.instance.PlaySound(soundTest, transform);
            PlaySoundAtRandLoc(false);
        }
    }

    //void ChanceForEvent()
    //{
    //    chance = (int)Random.Range(0, Mathf.Lerp(maxChance, minChance, GameManager.Instance.eventChance / 100)); //(Random.Range(1, 100) <= GameManager.Instance.eventChance)
    //    if (chance == 1 && !cooldown)
    //    {
    //        PlaySoundAtRandLoc(false);
    //        StartCoroutine(Cooldown());
    //    }
    //}

    void PlaySoundAtRandLoc(bool startCooldown)
    {
        mainDistance = Random.Range(distanceMin, distanceMax);
        randX = mainDistance - Random.Range(0, mainDistance);
        randZ = mainDistance - randX;
        randX *= Random.Range(0, 2) * 2 - 1;
        randZ *= Random.Range(0, 2) * 2 - 1;

        transform.position = new Vector3(randX, 0, randZ) + playerObj.transform.position;

        AudioManager.instance.PlaySound(sounds[Random.Range(0, sounds.Length)], transform);

        if (startCooldown)
        {
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(Random.Range(cooldownTimeMin, cooldownTimeMax));
        PlaySoundAtRandLoc(true);
    }
}
