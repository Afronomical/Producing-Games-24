using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakOnImpact : MonoBehaviour
{
    public bool isThrown;
    bool collided = false;

    private void Awake()
    {
        isThrown = false;
    }

    private void Update()
    {
        if(isThrown && collided)
            Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null)
        {
            collided = true;
        }
        

        
    }
}
