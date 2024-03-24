using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupControls : MonoBehaviour
{

    public GameObject[] TooltipsToSupress; 

    void Start()
    {
        for(int i = 0; i < TooltipsToSupress.Length; i++)
        {
            TooltipsToSupress[i].SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for(int i = 0; i < TooltipsToSupress.Length; i++)
        {
            TooltipsToSupress[i].SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
