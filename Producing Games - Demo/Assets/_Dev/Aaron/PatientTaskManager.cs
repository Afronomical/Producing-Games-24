using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;


public class PatientTaskManager : MonoBehaviour
{
    public enum HourlyTasks { Medicine };
    public enum TaskLocation { Bed, Bedside, Board, Altar };


    public static PatientTaskManager instance;


    public GameObject[] patients;
    public HourlyTask[] hourlyTasks;
    public HourlyTask[] genericTasks;
    public HourlyTask[] randomTasks;
    private List<Task> currentTasks = new List<Task>();

    public float minTimeBetweenRandomTasks = 10;
    public float maxTimeBetweenRandomTasks = 60;
    public int tasksPerPatient = 1;
    public float hourLength = 10;


    void Start()
    {
        if (instance == null)
            instance = this;

        SetHourlyTasks();
        StartCoroutine(HourTimer());
    }


    void CheckTaskConditions(GameObject interactedObject)
    {
        foreach (Task t in currentTasks)
        {
            t.CheckTaskConditions(interactedObject);
        }
    }


    void SetHourlyTasks()
    {
        for (int p = 0; p < patients.Length; p++)
        {
            for (int j = 0; j < tasksPerPatient; j++)
            {
                List<HourlyTask> choiceOfTasks = new List<HourlyTask>();
                int totalChance = 0;
                for (int i = 0; i < hourlyTasks.Length; i++)  // Check for invalid tasks and calculate total chance
                {
                    // Check for blocking tasks here
                    totalChance += hourlyTasks[i].chanceToHappen;
                    choiceOfTasks.Add(hourlyTasks[i]);
                }

                int rand = Random.Range(0, totalChance);
                HourlyTasks chosenTask = HourlyTasks.Medicine;
                Task newTask = null;

                int x = 0;
                foreach (HourlyTask t in choiceOfTasks)
                {
                    x += t.chanceToHappen;
                    if (rand <= t.chanceToHappen)
                    {
                        chosenTask = t.taskType;
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
    }


    void SetRandomTask()
    {

    }


    void ClearHourlyTasks()
    {
        for (int i = currentTasks.Count - 1; i >= 0; i--)
        {
            if (currentTasks[i].isHourlyTask)
            {
                currentTasks[i].FailTask();
                Destroy(currentTasks[i]);
                currentTasks.RemoveAt(i);
            }
        }
    }


    /*private IEnumerator TaskConditionCheckTimer()
    {
        yield return new WaitForSeconds(1);

        foreach (Task t in currentTasks)
        {
            t.CheckTaskConditions();
        }

        StartCoroutine(TaskConditionCheckTimer());
    }*/


    private IEnumerator HourTimer()
    {
        yield return new WaitForSeconds(hourLength);

        ClearHourlyTasks();
        SetHourlyTasks();

        StartCoroutine(HourTimer());
    }


    private IEnumerator RandomTaskTimer()
    {
        yield return new WaitForSeconds(Random.Range(minTimeBetweenRandomTasks, maxTimeBetweenRandomTasks));

        SetRandomTask();

        StartCoroutine(HourTimer());
    }
}