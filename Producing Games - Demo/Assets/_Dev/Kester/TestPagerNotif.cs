using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPagerNotif : MonoBehaviour
{
    public GameObject pager;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("SendMessage", 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SendMessage()
    {
        pager.GetComponent<PagerDisplay>().DisplayMessage("hello World", 10);
    }
}
