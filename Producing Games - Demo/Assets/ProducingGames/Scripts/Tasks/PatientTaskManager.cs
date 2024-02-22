using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;


public class PatientTaskManager : MonoBehaviour
{
    public enum HourlyTasks { Medicine, Injection, Cardiogram, Food, Board, Comfort, Pray };
    public enum RandomTasks { NoTask, Wandering, HeartAttack, Hiding, Cardiogram, Prayer, Hungry, Medication };
    public enum TaskLocation { Bed, Bedside, Board, Altar };


    public static PatientTaskManager instance;


    public GameObject[] patients;
    public HourlyTask[] hourlyTasks;
    public HourlyTask[] playerTasks;
    public RandomTask[] randomTasks;

    [HideInInspector] public List<Task> currentTasks = new List<Task>();

    public int tasksPerPatient = 1;

    public InteractiveObject noTaskPrompt;


    void Awake()
    {
        if (instance == null)
            instance = this;
    }


    public void CheckTaskConditions(GameObject interactedObject)
    {
        for (int i = currentTasks.Count - 1; i >= 0; i--)
        {
            //If it is an hourly task
            if (currentTasks[i].isHourlyTask && !currentTasks[i].taskCompleted)
            {
                //Get the task target
                if (currentTasks[i].taskTarget && currentTasks[i].taskTarget.TryGetComponent(out PatientCharacter character))
                {
                    //If they are in bed
                    if (character.currentState == PatientCharacter.PatientStates.Bed)
                        currentTasks[i].CheckTaskConditions(interactedObject);

                }
                //If it isn't a patient task
                else
                    currentTasks[i].CheckTaskConditions(interactedObject);
            }
            //If it is a random task
            else if (!currentTasks[i].isHourlyTask)
                currentTasks[i].CheckTaskConditions(interactedObject);
                
        }
    }


    public void DetectTasks(GameObject interactedObject)
    {
        for (int i = currentTasks.Count - 1; i >= 0; i--)
        {
            if(!currentTasks[i].taskCompleted)
                currentTasks[i].CheckDetectTask(interactedObject);
        }
    }


    public void SetHourlyTasks()
    {
        for (int i = 0; i < patients.Length; i++)
        {
            if (patients[i].GetComponent<PatientCharacter>().currentHealth > 0)
            {
                List <HourlyTask> tasksSetForThisPatient = new List <HourlyTask>();

                for (int j = 0; j < tasksPerPatient; j++)
                {
                    List<HourlyTask> choiceOfTasks = new List<HourlyTask>();
                    int totalChance = 0;
                    foreach (HourlyTask t in hourlyTasks)  // Check for invalid tasks and calculate total chance
                    {
                        // Check for blocking tasks here

                        if (!tasksSetForThisPatient.Contains(t))  // If this patient doesn't have that task
                        {
                            totalChance += t.chanceToHappen;
                            choiceOfTasks.Add(t);  // Add it to the list of tasks to pick from
                        }
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
                        case HourlyTasks.Food:
                            newTask = transform.AddComponent<HFoodTask>();
                            break;
                        case HourlyTasks.Comfort:
                            newTask = transform.AddComponent<HComfortTask>();
                            break;
                    }


                    if (newTask != null)
                    {
                        currentTasks.Add(newTask);
                        newTask.hTask = chosenTask;
                        newTask.taskTarget = patients[i];
                        CheckList.instance.AddTask(newTask);
                        tasksSetForThisPatient.Add(chosenTask);
                        newTask.TaskStart();
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
                    case RandomTasks.Hiding:
                        newTask = transform.AddComponent<RHidingTask>();
                        break;
                    case RandomTasks.Hungry:
                        newTask = transform.AddComponent<RHungryTask>();
                        break;
                    case RandomTasks.Prayer:
                        newTask = transform.AddComponent<RPrayingTask>();
                        break;
                    case RandomTasks.Medication:
                        newTask = transform.AddComponent<RMedicationTask>();
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
                }
            }
        }
    }



    public void CompleteTask(Task task)
    {
        CheckList.instance.CompleteTask(task);

        if (!task.isHourlyTask)  // Is not hourly task
        {
            currentTasks.Remove(task);
            Destroy(task);
        }
        else  // Is hourly task
        {
            GameManager.Instance.sanityEvents.CompleteHourlyTask();
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