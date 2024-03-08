using UnityEngine;
using UnityEngine.InputSystem;

public class FakeDoorInteractable : InteractableTemplate
{

    public void Awake()
    {

    }

    public override void Interact()
    {
        if (GameManager.Instance.player.GetComponent<PlayerInput>().enabled)
        {

        }
    }
}