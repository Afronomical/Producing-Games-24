using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Plays a random sound in range of the player
/// </summary>

public class ParanoiaSound : MonoBehaviour
{
    public SoundEffect[] sounds;
    public float distanceMin;
    public float distanceMax;
    public bool shouldPlay = true;
    public int minChance;
    public int maxChance;
    public float cooldownTime;
    private bool cooldown;
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
        chance = (int)Random.Range(0, Mathf.Lerp(maxChance, minChance, GameManager.Instance.eventChance / 100)); //(Random.Range(1, 100) <= GameManager.Instance.eventChance)
        if (chance == 1 && !cooldown)
        {
            PlaySoundAtRandLoc();
            StartCoroutine(Cooldown());
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

    private IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
    }
}
