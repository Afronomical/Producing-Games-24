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
        money.text = "£ " + budget;
    }

    public void AddIncome(int amount) { budget += amount; }

    public void RemoveIncome(int amount) { budget -= amount; }
}
