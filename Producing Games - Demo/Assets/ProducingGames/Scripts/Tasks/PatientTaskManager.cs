using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;


public class PatientTaskManager : MonoBehaviour
{
    public enum HourlyTasks
    {
        Medicine, Injection, Cardiogram, Food, Board, Comfort,  // Hourly
        NoTask, Pray, Clean, CheckFuse, CheckWater, Satellite
    };  // Player
    public enum RandomTasks { NoTask, Wandering, HeartAttack, Hiding, Cardiogram, Prayer, Hungry, Medication };
    public enum TaskLocation { Bed, Bedside, Board, Altar };


    public static PatientTaskManager instance;


    public GameObject[] patients;
    public HourlyTask[] hourlyTasks;
    public HourlyTask[] playerTasks;
    public RandomTask[] randomTasks;

    [SerializeField] private DemonTellChance environmentDemonTells = new DemonTellChance();
    [SerializeField] private DemonTellChance patientDemonTells = new DemonTellChance();

    [Space(10f)]
    [HideInInspector] public List<Task> currentTasks = new List<Task>();

    public int tasksPerPatient = 1;

    public InteractiveObject noTaskPrompt;

    private bool fuseHasBroken = false;
    private bool waterHasBroken = false;
    private bool satelliteHasBroken = false;

    [Header("Task Objects")]
    public GameObject altar;
    public GameObject fuse;
    public GameObject pipes;
    public GameObject satellite;
    public GameObject[] tables;


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
            if (!currentTasks[i].taskCompleted)
                currentTasks[i].CheckDetectTask(interactedObject);
        }
    }


    public void SetHourlyTasks()
    {
        for (int i = 0; i < patients.Length; i++)
        {
            if (patients[i].GetComponent<PatientCharacter>().currentHealth > 0)
            {
                List<HourlyTask> tasksSetForThisPatient = new List<HourlyTask>();

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

                    else if (t.taskName == "Hiding")
                    {
                        if (NPCManager.Instance.ChosenDemon.demonName == "Leviathan" || NPCManager.Instance.ChosenDemon.demonName == "Beezlebub")
                        {
                            int hidingChance = GetDemonTellTaskChance(patientDemonTells, patients[i].GetComponent<PatientCharacter>().hasBeenHiding);
                            for (int j = 0; j < hidingChance; j++)
                            {
                                totalChance += t.chanceToHappen;
                                choiceOfTasks.Add(t);
                            }
                        }
                    }

                    else if (t.taskName == "Medication")
                    {
                        if (NPCManager.Instance.ChosenDemon.demonName == "Leviathan" || NPCManager.Instance.ChosenDemon.demonName == "Mammon")
                        {
                            int greedChance = GetDemonTellTaskChance(patientDemonTells, patients[i].GetComponent<PatientCharacter>().hasBeenGreedy);
                            for (int j = 0; j < greedChance; j++)
                            {
                                totalChance += t.chanceToHappen;
                                choiceOfTasks.Add(t);
                            }
                        }
                    }

                    else if (t.taskName == "Hungry")
                    {
                        if (NPCManager.Instance.ChosenDemon.demonName == "Mammon" || NPCManager.Instance.ChosenDemon.demonName == "Beezlebub")
                        {
                            int hungryChance = GetDemonTellTaskChance(patientDemonTells, patients[i].GetComponent<PatientCharacter>().hasBeenHungry);
                            for (int j = 0; j < hungryChance; j++)
                            {
                                totalChance += t.chanceToHappen;
                                choiceOfTasks.Add(t);
                            }
                        }
                    }

                    else
                    {
                        totalChance += t.chanceToHappen;
                        choiceOfTasks.Add(t);
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



    public void SetPlayerTask()
    {
        List<HourlyTask> choiceOfTasks = new List<HourlyTask>();
        int totalChance = 0;
        foreach (HourlyTask t in playerTasks)  // Check for invalid tasks and calculate total chance
        {
            if (t.taskName == "Fix Satellite")
            {
                if (NPCManager.Instance.ChosenDemon.demonName == "Leviathan" || NPCManager.Instance.ChosenDemon.demonName == "Beezlebub")
                {
                    int satelliteChance = GetDemonTellTaskChance(patientDemonTells, satelliteHasBroken, true);
                    for (int j = 0; j < satelliteChance; j++)
                    {
                        totalChance += t.chanceToHappen;
                        choiceOfTasks.Add(t);
                    }
                }
            }

            else if (t.taskName == "Fix Water")
            {
                if (NPCManager.Instance.ChosenDemon.demonName == "Leviathan" || NPCManager.Instance.ChosenDemon.demonName == "Mammon")
                {
                    int waterChance = GetDemonTellTaskChance(patientDemonTells, waterHasBroken, true);
                    for (int j = 0; j < waterChance; j++)
                    {
                        totalChance += t.chanceToHappen;
                        choiceOfTasks.Add(t);
                    }
                }
            }

            else if (t.taskName == "Change Fuse")
            {
                if (NPCManager.Instance.ChosenDemon.demonName == "Mammon" || NPCManager.Instance.ChosenDemon.demonName == "Beezlebub")
                {
                    int fuseChance = GetDemonTellTaskChance(patientDemonTells, fuseHasBroken, true);
                    for (int j = 0; j < fuseChance; j++)
                    {
                        totalChance += t.chanceToHappen;
                        choiceOfTasks.Add(t);
                    }
                }
            }

            else
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
            case HourlyTasks.Pray:
                newTask = transform.AddComponent<PPrayTask>();
                newTask.taskTarget = altar;
                break;
            case HourlyTasks.Satellite:
                newTask = transform.AddComponent<PSatelliteTask>();
                newTask.taskTarget = satellite;
                break;
        }


        if (newTask != null)
        {
            currentTasks.Add(newTask);
            newTask.hTask = chosenTask;
            CheckList.instance.AddTask(newTask);
            newTask.TaskStart();
        }
    }



    public void CompleteTask(Task task)
    {
        CheckList.instance.CompleteTask(task);

        if (!task.isHourlyTask)  // Is not hourly task
        {
            //currentTasks.Remove(task);
            //Destroy(task);
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
            if (currentTasks[i].checkList != null) CheckList.instance.RemoveTask(currentTasks[i]);
            Destroy(currentTasks[i]);
            currentTasks.RemoveAt(i);
        }
    }


    private int GetDemonTellTaskChance(DemonTellChance chances, bool repeating, bool Environment = false)
    {
        int sanityEffect = GameManager.Instance.startingSanity - GameManager.Instance.GetSanity();

        switch(GameManager.Instance.currentHour)
        {
            case 1:
                return chances.hour1Chance;
            case 2:
                if (!repeating) return chances.hour2Chance;
                else return (chances.hour2RepeatChance + sanityEffect);
            case 3:
                if (!repeating) return chances.hour3Chance;
                else return (chances.hour3RepeatChance + sanityEffect);
            case 4:
                if (!repeating) return chances.hour4Chance;
                else return (chances.hour4RepeatChance + sanityEffect);
            case 5:
                if (!repeating) return chances.hour5Chance;
                else return (chances.hour5RepeatChance + sanityEffect);
            case 6:
                if (!Environment) return chances.hour6Chance;
                else return (chances.hour6Chance + sanityEffect);
            case 7:
                if (!Environment) return chances.hour7Chance;
                else return (chances.hour7Chance + sanityEffect);
            case 8:
                if (!Environment) return chances.hour8Chance;
                else return (chances.hour8Chance + sanityEffect);
            default:
                Debug.LogError("GetDemonTellChance in PatientTaskManager failed");
                return 0;
        }
    }



    [System.Serializable]
    private struct DemonTellChance
    {
        [Range(0, 100)] public int hour1Chance;
        [Range(0, 100)] public int hour2Chance;
        [Range(0, 100)] public int hour2RepeatChance;
        [Range(0, 100)] public int hour3Chance;
        [Range(0, 100)] public int hour3RepeatChance;
        [Range(0, 100)] public int hour4Chance;
        [Range(0, 100)] public int hour4RepeatChance;
        [Range(0, 100)] public int hour5Chance;
        [Range(0, 100)] public int hour5RepeatChance;
        [Range(0, 100)] public int hour6Chance;
        [Range(0, 100)] public int hour7Chance;
        [Range(0, 100)] public int hour8Chance;

        public DemonTellChance(int _hour1Chance, int _hour2Chance, int _hour2RepeatChance, int _hour3Chance, int _hour3RepeatChance, int _hour4Chance, int _hour4RepeatChance, int _hour5Chance, int _hour5RepeatChance, int _hour6Chance, int _hour7Chance, int _hour8Chance)
        {
            hour1Chance = _hour1Chance;
            hour2Chance = _hour2Chance;
            hour2RepeatChance = _hour2RepeatChance;
            hour3Chance = _hour3Chance;
            hour3RepeatChance = _hour3RepeatChance;
            hour4Chance = _hour4Chance;
            hour4RepeatChance = _hour4RepeatChance;
            hour5Chance = _hour5Chance;
            hour5RepeatChance = _hour5RepeatChance;
            hour6Chance = _hour6Chance;
            hour7Chance = _hour7Chance;
            hour8Chance = _hour8Chance;
        }
    }
}