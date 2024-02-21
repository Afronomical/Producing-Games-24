using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemonInfo : MonoBehaviour
{
    public DemonItemsSO demon;
    private TMP_Text demonName;
    private TMP_Text demonDescription;

    private int childIndex;

    // Start is called before the first frame update
    void Start()
    {
        demonName = GetComponent<TMP_Text>();
        childIndex = gameObject.transform.childCount;
        demonDescription = GetComponentInChildren<TMP_Text>();
    }

    private void SetBookText()
    {
        demonName.text = demon.demonName;
        demonDescription.text = demon.demonName;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
