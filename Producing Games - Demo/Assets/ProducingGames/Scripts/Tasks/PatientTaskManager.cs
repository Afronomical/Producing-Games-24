using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;


public class PatientTaskManager : MonoBehaviour
{
    public enum HourlyTasks { Medicine, Injection };
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


    void Awake()
    {
        if (instance == null)
            instance = this;
    }


    public void CheckTaskConditions(GameObject interactedObject)
    {
        if (!GameManager.Instance.shiftEndActive)
        {
            for (int i = currentTasks.Count - 1; i >= 0; i--)
            {
                if (!currentTasks[i].taskCompleted)
                    currentTasks[i].CheckTaskConditions(interactedObject);
            }
        }
    }


    public void SetHourlyTasks()
    {
        for (int i = 0; i < patients.Length; i++)
        {
            for (int j = 0; j < tasksPerPatient; j++)
            {
                List<HourlyTask> choiceOfTasks = new List<HourlyTask>();
                int totalChance = 0;
                foreach (HourlyTask t in hourlyTasks)  // Check for invalid tasks and calculate total chance
                {
                    // Check for blocking tasks here
                    totalChance += t.chanceToHappen;
                    choiceOfTasks.Add(t);
                }

                int rand = Random.Range(0, totalChance);
                HourlyTask chosenTask = choiceOfTasks[0];
                Task newTask = null;

                int x = 0;
                foreach (HourlyTask t in choiceOfTasks)
                {
                    x += t.chanceToHappen;
                    if (rand <= t.chanceToHappen)
                    {
                        chosenTask = t;
                    }
                }



                switch (chosenTask.taskType)
                {
                    case HourlyTasks.Medicine:
                        newTask = transform.AddComponent<HMedicineTask>();
                        break;
                    case HourlyTasks.Injection:
                        newTask = transform.AddComponent<HInjectionTask>();
                        break;
                }

                if (newTask.isHourlyTask) newTask.hTask = chosenTask;
                else if (!newTask.isHourlyTask) newTask.rTask = chosenTask;

                if (newTask != null)
                {
                    currentTasks.Add(newTask);
                    newTask.taskTarget = patients[i];
                    patients[i].transform.Find("Eye 1").GetComponent<MeshRenderer>().material = chosenTask.taskEyes;
                    patients[i].transform.Find("Eye 2").GetComponent<MeshRenderer>().material = chosenTask.taskEyes;
                }
            }
        }
    }


    public void SetRandomTask()
    {

    }


    public void CompleteTask(Task task)
    {

        //currentTasks.Remove(task);
        //Destroy(task);
    }


    public void ClearHourlyTasks()
    {
        for (int i = currentTasks.Count - 1; i >= 0; i--)
        {
            if (currentTasks[i].isHourlyTask)
            {
                if (!currentTasks[i].taskCompleted)
                    currentTasks[i].FailTask();
                Destroy(currentTasks[i]);
                currentTasks.RemoveAt(i);
            }
        }
    }
}