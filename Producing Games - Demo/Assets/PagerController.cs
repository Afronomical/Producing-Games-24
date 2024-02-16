using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PagerController : MonoBehaviour
{
    public GameObject pagerInterface;
    public PlayerArms arms;


    private void Start()
    {
        // Set the pagerInterface to be initially invisible
        if (pagerInterface != null)
        {
            pagerInterface.SetActive(false);
        }

    }

    public void OnPagerInput(InputAction.CallbackContext context)
    {
        if (pagerInterface != null)
        {
            pagerInterface.SetActive(!pagerInterface.activeSelf);
            if (pagerInterface.activeSelf) arms.HoldPager();
            else arms.DropPager();
        }
    }
}
