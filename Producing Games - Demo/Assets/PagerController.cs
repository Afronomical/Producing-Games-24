using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagerController : MonoBehaviour
{

    public GameObject pagerInterface;


    private void Start()
    {
        // Set the pagerInterface to be initially invisible
        if (pagerInterface != null)
        {
            pagerInterface.SetActive(false);
        }

    }


    private void Update()
    {
        // Check for TAB key press
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Toggle the visibility of the pager interface
            if (pagerInterface != null)
            {
                pagerInterface.SetActive(!pagerInterface.activeSelf);
            }
        }      
    }
}
