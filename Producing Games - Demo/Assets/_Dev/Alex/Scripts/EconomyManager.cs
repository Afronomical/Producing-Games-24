using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;
    private int budget = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayBudget(TMP_Text moneyText)
    {
        moneyText.text = "£ " + budget;
    }

    public void AddIncome(int amount) { budget += amount; }

    public void RemoveIncome(int amount) { budget -= amount; }
}
