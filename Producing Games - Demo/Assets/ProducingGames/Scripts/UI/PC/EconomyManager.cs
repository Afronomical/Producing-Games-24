using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;
    [SerializeField]private int budget = 100;
    public TMP_Text money;
    private int deliveryTime = 0;
    public List<GameObject> boughtItems = new();



    private void Awake()
    {
        
    }
    public void InitializeCommands()
    {

        CommandConsole.Instance.Add1000Cash += Add1000Income;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeCommands();
        if (instance == null)
            instance = this;

        if (money != null)
            money.text = "£ " + budget;
    }

    public void DisplayBudget()
    {
        if(budget < 0) budget = 0;
        money.text = "£ " + budget;
    }

    public void AddIncome(int amount) { budget += amount; DisplayBudget(); }

    public void Add1000Income()
    {
        Debug.Log("Cash command Invoked");
        AddIncome(1000);
    }

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
        DisplayBudget();
    }

    public void OrderItem(GameObject item)
    {
        deliveryTime = GameManager.Instance.currentHour + 1;
        boughtItems.Add(item);
    }

    public void SpawnItem()
    {
        //if(deliveryTime == GameManager.Instance.currentHour)
        //{
            InteractableSpawner.instance.SpawnAllPurchasedItems(boughtItems, InteractableSpawner.instance.shopSpawns);
            deliveryTime = 0;
            boughtItems.Clear();
            //Debug.Log("Spawned");
        //}
    }
}
