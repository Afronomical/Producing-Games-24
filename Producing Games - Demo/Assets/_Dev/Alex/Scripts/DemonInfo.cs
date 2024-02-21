using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemonInfo : MonoBehaviour
{
    public TMP_Text demonName;
    //private TMP_Text demonDescription;

    private int childIndex;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void SetBookText()
    {
        demonName.text = NPCManager.Instance.ChosenDemon.demonName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
