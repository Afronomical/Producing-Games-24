using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CubeDestroyTest : InteractableTemplate
{
    public GameObject playerGateBlock;
    private bool collectedOnce = false;
    public override void Interact()
    {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("AdamsScene") && playerGateBlock.activeSelf && collectedOnce == false)
        {
            playerGateBlock.SetActive( false );
            //Tutorialmanager.instance.OnCollectMedicine();
            collectedOnce = true;
        }

        Debug.Log("***Cube code is running*** \n Collected " + collectible.name);
        InventoryHotbar.instance.AddToInventory(collectible);
        Destroy(gameObject);
    }
}
