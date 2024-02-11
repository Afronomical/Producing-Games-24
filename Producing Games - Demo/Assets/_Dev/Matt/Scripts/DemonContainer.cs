using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonContainer : MonoBehaviour
{
    public List<ScriptableObject> DemonTypes = new(); 

    ///possibly need new base classes for each demon behaviour? Or new function in AI character to only switch between demon states of a specific type  
    public ScriptableObject GetRandomDemonType()
    {
        int choice = UnityEngine.Random.Range(0, DemonTypes.Count);
        return DemonTypes[choice];
    }
}
