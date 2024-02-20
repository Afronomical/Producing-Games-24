using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;
    private int budget = 100;
    public TMP_Text money;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;

        money.text = "£ " + budget;
    }

    public void DisplayBudget()
    {
        if(budget < 0) budget = 0;
        money.text = "£ " + budget;
    }

    public void AddIncome(int amount) { budget += amount; }

    public void RemoveIncome(int amount) 
    {
        if(budget <= 0) 
        {
            //Stop buying/spawning items if broke
            //Display message to tell player they are broke
        }
        else
        {
            budget -= amount;
            //Spawn item in storage
        }
    }
}
