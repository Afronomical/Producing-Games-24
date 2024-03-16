using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorEventManager : MonoBehaviour
{
    public static HorrorEventManager Instance;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    public List<GameObject> Events = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
