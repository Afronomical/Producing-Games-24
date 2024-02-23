using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CapturedBox : MonoBehaviour
{
    
    GameObject playerObj;
    public GameObject tpLoc;

    
    void Start()
    {
        playerObj = GameManager.Instance.player;
    }

    
    void Update()
    {



        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(MainEvent());

        }
    }

    IEnumerator MainEvent()
    {
        playerObj.GetComponent<CharacterController>().enabled = false;
        //playerObj.GetComponent<PlayerInput>().enabled = false;
        playerObj.transform.position = tpLoc.transform.position;
        yield return new WaitForSeconds(1f);
        playerObj.GetComponent<CharacterController>().enabled = true;
    }
}
