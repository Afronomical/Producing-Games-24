using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PatientTaskManager : MonoBehaviour
{
    public enum HourlyTasks { Medicine };
    public enum TaskLocation { Bed, Bedside, Board, Altar };

    public GameObject[] patients;
    public HourlyTask[] hourlyTasks;
    private List<Task> currentTasks = new List<Task>();


    void Start()
    {
        SetHourlyTasks();
        StartCoroutine(HourTimer());
    }


    void Update()
    {
        
    }


    void SetHourlyTasks()
    {
        // Shuffle the patient array?
        for (int p = 0; p < patients.Length; p++)
        {
            HourlyTasks[] choiceOfTasks;
            int totalChance = 0;
            for (int i = 0; i < hourlyTasks.Length; i++)
            {
                if (hourlyTasks[i].BlockingTasks)
                totalChance += hourlyTasks[i].chanceToHappen;
            }

            int rand = Random.Range(0, totalChance);
            HourlyTasks chosenTask = HourlyTasks.Medicine;
            Task newTask = null;

            int x = 0;
            for (int i = 0; i < hourlyTasks.Length; i++)
            {
                x += hourlyTasks[i].chanceToHappen;
                if (rand <= hourlyTasks[i].chanceToHappen)
                {
                    chosenTask = hourlyTasks[i].taskType;
                    break;
                }
            }



            switch (chosenTask)
            {
                case HourlyTasks.Medicine:
                    newTask = transform.AddComponent<HMedicineTask>();
                    break;
            }

            if (newTask != null)
            {
                currentTasks.Add(newTask);
                newTask.patient = patients[p];
            }
        }
    }


    void SetRandomTask()
    {

    }


    void ClearHourlyTasks()
    {
        foreach (Task t in currentTasks)
        {
            t.FailTask();
            currentTasks.Remove(t);
        }
    }


    private IEnumerator HourTimer()
    {
        yield return new WaitForSeconds(60);

        ClearHourlyTasks();
        SetHourlyTasks();

        StartCoroutine(HourTimer());
    }
}