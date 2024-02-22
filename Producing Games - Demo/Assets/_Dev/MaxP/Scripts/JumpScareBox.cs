using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareBox : MonoBehaviour
{
    //public static GameManager Instance;
    GameObject playerObj;
    public GameObject tpLoc;
    
    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {


        
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.player.GetComponent<CharacterController>().enabled = false;
            GameManager.Instance.player.transform.position = tpLoc.transform.position;
            GameManager.Instance.player.GetComponent<CharacterController>().enabled = true;

        }
    }
}
