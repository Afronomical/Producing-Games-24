using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class RandomPill : InteractableTemplate, IConsumable
{
    GameObject player;
    Image panel;
    public SoundEffect munchSoundEffect;

    private void Start()
    {
        player = GameObject.Find("Player");
        panel = GameObject.Find("CameraDimOverlay").GetComponent<Image>();
    }


    public void Consume()
    {

        AudioManager.instance.PlaySound(munchSoundEffect, player.transform);
        Debug.Log("Consuming random pill");
        InventoryHotbar.instance.RemoveFromInventory(collectible);

        int rand = Random.Range(1, 100);

        if (rand >= 0 && rand < 25)
        {
            SlowPlayerDown();

        }
        else if (rand >= 25 && rand < 50)
        {
            SpeedPlayerUp();

        }
        else if (rand >= 50 && rand < 75)
        {
            DimPlayerScreen();

        }
        else
        {

            StopPlayer();
        }

        Destroy(gameObject);
    }

    public override void Interact()
    {
        Debug.Log("***Player has picked up random pill***");
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }

    private void SlowPlayerDown()
    {
        Debug.Log("Slowed effect");
        player.GetComponent<PlayerMovement>().slowedEffect = true;
        player.GetComponent<PlayerMovement>().walkSpeed /= 1.5f;
        player.GetComponent<PlayerMovement>().sprintSpeed /= 1.5f;
        player.GetComponent<PlayerMovement>().crouchSpeed /= 1.5f;

        player.GetComponent<Flashlight>().light.intensity *= 10;
    }

    private void SpeedPlayerUp()
    {
        Debug.Log("Speed up effect");
        player.GetComponent<PlayerMovement>().boostedEffect = true;
        player.GetComponent<PlayerMovement>().walkSpeed *= 1.2f;
        player.GetComponent<PlayerMovement>().sprintSpeed *= 1.2f;
        player.GetComponent<PlayerMovement>().crouchSpeed *= 1.2f;

    }

    private void StopPlayer()
    {
        Debug.Log("Stopped effect");
        player.GetComponent<PlayerMovement>().stoppedEffect = true;
        player.GetComponent<PlayerMovement>().walkSpeed = 0;
        player.GetComponent<PlayerMovement>().sprintSpeed = 0;
        player.GetComponent<PlayerMovement>().crouchSpeed = 0f;

        if(GameManager.Instance.demon.activeSelf)
        {
            GameManager.Instance.demon.GetComponent<DemonCharacter>().agent.velocity /= 2;

        }
    }

    private void DimPlayerScreen()
    {
        Debug.Log("Dimmed effect");
        panel.enabled = true;
        player.GetComponent<PlayerMovement>().dimmedEffect = true;
    }

    public enum Effects
    {
        Slow,
        Speed,
        Stop,
        Dim,
    }
    
    public static void SetPlayerEffect(Effects effects)
    {
        switch (effects)
        {
            case Effects.Slow:
                break;
            case Effects.Speed:
                break;
            case Effects.Stop:
                break;
            case Effects.Dim:
                break;
            default:
                break;
        }
    }

}
