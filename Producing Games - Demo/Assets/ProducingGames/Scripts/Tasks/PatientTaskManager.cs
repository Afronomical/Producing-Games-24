using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;


public class PatientTaskManager : MonoBehaviour
{
    public enum HourlyTasks { Medicine, Injection };
    public enum RandomTasks { NoTask, Wandering, HeartAttack };
    public enum TaskLocation { Bed, Bedside, Board, Altar };


    public static PatientTaskManager instance;


    public GameObject[] patients;
    public HourlyTask[] hourlyTasks;
    public HourlyTask[] genericTasks;

    public RandomTask[] randomTasks;

    [HideInInspector] public List<Task> currentTasks = new List<Task>();

    public int tasksPerPatient = 1;


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
            if (patients[i].GetComponent<PatientCharacter>().currentHealth > 0)
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
                        if (rand >= x)
                        {
                            x += t.chanceToHappen;
                            if (rand <= x)
                            {
                                chosenTask = t;
                            }
                        }
                        else x += t.chanceToHappen;
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


                    if (newTask != null)
                    {
                        currentTasks.Add(newTask);
                        newTask.hTask = chosenTask;
                        newTask.taskTarget = patients[i];
                        patients[i].transform.Find("Eye 1").GetComponent<MeshRenderer>().material = chosenTask.taskEyes;
                        patients[i].transform.Find("Eye 2").GetComponent<MeshRenderer>().material = chosenTask.taskEyes;
                        CheckList.instance.AddTask(newTask);
                    }
                }

                
            }
        }
    }



    public void SetRandomTasks()
    {
        for (int i = 0; i < patients.Length; i++)
        {
            if (patients[i].GetComponent<PatientCharacter>().currentHealth > 0)
            {
                List<RandomTask> choiceOfTasks = new List<RandomTask>();
                int totalChance = 0;
                foreach (RandomTask t in randomTasks)  // Check for invalid tasks and calculate total chance
                {
                    // Check for blocking tasks here

                    totalChance += t.chanceToHappen;
                    choiceOfTasks.Add(t);


                    if (t.taskName == "Heart Attack")  // Heart attack chance increase
                    {
                        PatientCharacter patient = patients[i].GetComponent<PatientCharacter>();
                        int heartAttackChance = patient.startingHealth - patient.currentHealth;
                        for (int j = 0; j < heartAttackChance; j++)
                        {
                            totalChance += t.chanceToHappen;
                            choiceOfTasks.Add(t);
                        }
                    }
                }

                int rand = Random.Range(0, totalChance);
                RandomTask chosenTask = randomTasks[0];
                Task newTask = null;

                int x = 0;
                foreach (RandomTask t in choiceOfTasks)
                {
                    if (rand >= x)
                    {
                        x += t.chanceToHappen;
                        if (rand <= x)
                        {
                            chosenTask = t;
                        }
                    }
                    else x += t.chanceToHappen;
                }


                switch (chosenTask.taskType)
                {
                    case RandomTasks.Wandering:
                        newTask = transform.AddComponent<RWanderingTask>();
                        break;
                    case RandomTasks.HeartAttack:
                        newTask = transform.AddComponent<RHeartAttackTask>();
                        break;
                    default:
                        break;
                }


                if (newTask != null)
                {
                    currentTasks.Add(newTask);
                    newTask.rTask = chosenTask;
                    newTask.isHourlyTask = false;
                    newTask.taskTarget = patients[i];
                    newTask.TaskStart();
                    CheckList.instance.AddTask(newTask);
                }
            }
        }
    }



    public void CompleteTask(Task task)
    {
        CheckList.instance.RemoveTask(task);
        if (!task.isHourlyTask)
        {
            currentTasks.Remove(task);
            Destroy(task);
        }



    }


    public void ClearTasks()
    {
        for (int i = currentTasks.Count - 1; i >= 0; i--)
        {
            if (currentTasks[i].isHourlyTask)
            {

                if (!currentTasks[i].taskCompleted)
                    currentTasks[i].FailTask();



            }
            CheckList.instance.RemoveTask(currentTasks[i]);
            Destroy(currentTasks[i]);
            currentTasks.RemoveAt(i);
        }
    }
}