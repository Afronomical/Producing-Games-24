using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;


public class TutorialTaskManager : MonoBehaviour
{
    public static TutorialTaskManager instance;

    [SerializeField] private TutorialTasks[] patients;

    [SerializeField] private TutorialPlayerTasks player;


    void Awake()
    {
        if (instance == null)
            instance = this;
    }


    public void SetHourlyTasks()
    {
        for (int i = 0; i < patients.Length; i++)
        {
            if (patients[i].patient.GetComponent<PatientCharacter>().currentHealth > 0)
            {
                Task newTask = null;
                PatientTaskManager.HourlyTasks chosenTask = patients[i].hour1Hourly;
                switch (GameManager.Instance.currentHour)
                {
                    case 1:
                        chosenTask = patients[i].hour1Hourly;
                        break;
                    case 2:
                        chosenTask = patients[i].hour2Hourly;
                        break;
                    case 3:
                        chosenTask = patients[i].hour3Hourly;
                        break;
                    case 4:
                        chosenTask = patients[i].hour4Hourly;
                        break;
                    case 5:
                        chosenTask = patients[i].hour5Hourly;
                        break;
                    case 6:
                        chosenTask = patients[i].hour6Hourly;
                        break;
                    case 7:
                        chosenTask = patients[i].hour7Hourly;
                        break;
                    case 8:
                        chosenTask = patients[i].hour8Hourly;
                        break;
                }

                if (chosenTask != PatientTaskManager.HourlyTasks.NoTask)
                {
                    switch (chosenTask)
                    {
                        case PatientTaskManager.HourlyTasks.Medicine:
                            newTask = transform.AddComponent<HMedicineTask>();
                            newTask.hTask = PatientTaskManager.instance.hourlyTasks[0];
                            break;
                        case PatientTaskManager.HourlyTasks.Injection:
                            newTask = transform.AddComponent<HInjectionTask>();
                            newTask.hTask = PatientTaskManager.instance.hourlyTasks[1];
                            break;
                        case PatientTaskManager.HourlyTasks.Food:
                            newTask = transform.AddComponent<HFoodTask>();
                            newTask.hTask = PatientTaskManager.instance.hourlyTasks[2];
                            break;
                        case PatientTaskManager.HourlyTasks.Comfort:
                            newTask = transform.AddComponent<HComfortTask>();
                            newTask.hTask = PatientTaskManager.instance.hourlyTasks[3];
                            break;
                    }


                    if (newTask != null)
                    {
                        PatientTaskManager.instance.currentTasks.Add(newTask);
                        newTask.taskTarget = patients[i].patient;
                        CheckList.instance.AddTask(newTask);
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
            if (patients[i].patient.GetComponent<PatientCharacter>().currentHealth > 0)
            {
                Task newTask = null;
                PatientTaskManager.RandomTasks chosenTask = patients[i].hour1Random;
                switch (GameManager.Instance.currentHour)
                {
                    case 1:
                        chosenTask = patients[i].hour1Random;
                        break;
                    case 2:
                        chosenTask = patients[i].hour2Random;
                        break;
                    case 3:
                        chosenTask = patients[i].hour3Random;
                        break;
                    case 4:
                        chosenTask = patients[i].hour4Random;
                        break;
                    case 5:
                        chosenTask = patients[i].hour5Random;
                        break;
                    case 6:
                        chosenTask = patients[i].hour6Random;
                        break;
                    case 7:
                        chosenTask = patients[i].hour7Random;
                        break;
                    case 8:
                        chosenTask = patients[i].hour8Random;
                        break;
                }

                if (chosenTask != PatientTaskManager.RandomTasks.NoTask)
                {
                    switch (chosenTask)
                    {
                        case PatientTaskManager.RandomTasks.Wandering:
                            newTask = transform.AddComponent<RWanderingTask>();
                            newTask.rTask = PatientTaskManager.instance.randomTasks[1];
                            break;
                        case PatientTaskManager.RandomTasks.HeartAttack:
                            newTask = transform.AddComponent<RHeartAttackTask>();
                            newTask.rTask = PatientTaskManager.instance.randomTasks[2];
                            break;
                        case PatientTaskManager.RandomTasks.Hiding:
                            newTask = transform.AddComponent<RHidingTask>();
                            newTask.rTask = PatientTaskManager.instance.randomTasks[3];
                            break;
                        case PatientTaskManager.RandomTasks.Hungry:
                            newTask = transform.AddComponent<RHungryTask>();
                            newTask.rTask = PatientTaskManager.instance.randomTasks[4];
                            break;
                        case PatientTaskManager.RandomTasks.Prayer:
                            newTask = transform.AddComponent<RPrayingTask>();
                            newTask.rTask = PatientTaskManager.instance.randomTasks[5];
                            break;
                        case PatientTaskManager.RandomTasks.Medication:
                            newTask = transform.AddComponent<RMedicationTask>();
                            newTask.rTask = PatientTaskManager.instance.randomTasks[6];
                            break;
                        default:
                            break;
                    }


                    if (newTask != null)
                    {
                        PatientTaskManager.instance.currentTasks.Add(newTask);
                        newTask.isHourlyTask = false;
                        newTask.TaskStart();
                    }
                }
            }
        }
    }



    public void SetPlayerTask()
    {
        Task newTask = null;
        PatientTaskManager.HourlyTasks chosenTask = player.hour1Task;
        switch(GameManager.Instance.currentHour)
        {
            case 1:
                chosenTask = player.hour1Task;
                break;
            case 2:
                chosenTask = player.hour2Task;
                break;
            case 3:
                chosenTask = player.hour3Task;
                break;
            case 4:
                chosenTask = player.hour4Task;
                break;
            case 5:
                chosenTask = player.hour5Task;
                break;
            case 6:
                chosenTask = player.hour6Task;
                break;
            case 7:
                chosenTask = player.hour7Task;
                break;
            case 8:
                chosenTask = player.hour8Task;
                break;
        }

        if (chosenTask != PatientTaskManager.HourlyTasks.NoTask)
        {
            switch (chosenTask)
            {
                case PatientTaskManager.HourlyTasks.Pray:
                    newTask = transform.AddComponent<PPrayTask>();
                    newTask.hTask = PatientTaskManager.instance.playerTasks[1];
                    newTask.taskTarget = PatientTaskManager.instance.altar;
                    break;
                case PatientTaskManager.HourlyTasks.Satellite:
                    newTask = transform.AddComponent<PSatelliteTask>();
                    newTask.hTask = PatientTaskManager.instance.playerTasks[2];
                    newTask.taskTarget = PatientTaskManager.instance.satellite;
                    break;
            }


            if (newTask != null)
            {
                PatientTaskManager.instance.currentTasks.Add(newTask);
                CheckList.instance.AddTask(newTask);
                newTask.TaskStart();
            }
        }
    }



    [System.Serializable]
    private struct TutorialTasks
    {
        public GameObject patient;
        [Space(30)]
        public PatientTaskManager.HourlyTasks hour1Hourly;
        public PatientTaskManager.RandomTasks hour1Random;
        [Space(10)]
        public PatientTaskManager.HourlyTasks hour2Hourly;
        public PatientTaskManager.RandomTasks hour2Random;
        [Space(10)]
        public PatientTaskManager.HourlyTasks hour3Hourly;
        public PatientTaskManager.RandomTasks hour3Random;
        [Space(10)]
        public PatientTaskManager.HourlyTasks hour4Hourly;
        public PatientTaskManager.RandomTasks hour4Random;
        [Space(10)]
        public PatientTaskManager.HourlyTasks hour5Hourly;
        public PatientTaskManager.RandomTasks hour5Random;
        [Space(10)]
        public PatientTaskManager.HourlyTasks hour6Hourly;
        public PatientTaskManager.RandomTasks hour6Random;
        [Space(10)]
        public PatientTaskManager.HourlyTasks hour7Hourly;
        public PatientTaskManager.RandomTasks hour7Random;
        [Space(10)]
        public PatientTaskManager.HourlyTasks hour8Hourly;
        public PatientTaskManager.RandomTasks hour8Random;

        public TutorialTasks(GameObject _patient,
        PatientTaskManager.HourlyTasks _hour1Hourly,
        PatientTaskManager.RandomTasks _hour1Random,
        PatientTaskManager.HourlyTasks _hour2Hourly,
        PatientTaskManager.RandomTasks _hour2Random,
        PatientTaskManager.HourlyTasks _hour3Hourly,
        PatientTaskManager.RandomTasks _hour3Random,
        PatientTaskManager.HourlyTasks _hour4Hourly,
        PatientTaskManager.RandomTasks _hour4Random,
        PatientTaskManager.HourlyTasks _hour5Hourly,
        PatientTaskManager.RandomTasks _hour5Random,
        PatientTaskManager.HourlyTasks _hour6Hourly,
        PatientTaskManager.RandomTasks _hour6Random,
        PatientTaskManager.HourlyTasks _hour7Hourly,
        PatientTaskManager.RandomTasks _hour7Random,
        PatientTaskManager.HourlyTasks _hour8Hourly,
        PatientTaskManager.RandomTasks _hour8Random)
        {
            patient = _patient;
            hour1Hourly = _hour1Hourly;
            hour1Random = _hour1Random;
            hour2Hourly = _hour2Hourly;
            hour2Random = _hour2Random;
            hour3Hourly = _hour3Hourly;
            hour3Random = _hour3Random;
            hour4Hourly = _hour4Hourly;
            hour4Random = _hour4Random;
            hour5Hourly = _hour5Hourly;
            hour5Random = _hour5Random;
            hour6Hourly = _hour6Hourly;
            hour6Random = _hour6Random;
            hour7Hourly = _hour7Hourly;
            hour7Random = _hour7Random;
            hour8Hourly = _hour8Hourly;
            hour8Random = _hour8Random;
        }
    }



    [System.Serializable]
    private struct TutorialPlayerTasks
    {
        public PatientTaskManager.HourlyTasks hour1Task;
        public PatientTaskManager.HourlyTasks hour2Task;
        public PatientTaskManager.HourlyTasks hour3Task;
        public PatientTaskManager.HourlyTasks hour4Task;
        public PatientTaskManager.HourlyTasks hour5Task;
        public PatientTaskManager.HourlyTasks hour6Task;
        public PatientTaskManager.HourlyTasks hour7Task;
        public PatientTaskManager.HourlyTasks hour8Task;

        public TutorialPlayerTasks(
        PatientTaskManager.HourlyTasks _hour1Task,
        PatientTaskManager.HourlyTasks _hour2Task,
        PatientTaskManager.HourlyTasks _hour3Task,
        PatientTaskManager.HourlyTasks _hour4Task,
        PatientTaskManager.HourlyTasks _hour5Task,
        PatientTaskManager.HourlyTasks _hour6Task,
        PatientTaskManager.HourlyTasks _hour7Task,
        PatientTaskManager.HourlyTasks _hour8Task)
        {
            hour1Task = _hour1Task;
            hour2Task = _hour2Task;
            hour3Task = _hour3Task;
            hour4Task = _hour4Task;
            hour5Task = _hour5Task;
            hour6Task = _hour6Task;
            hour7Task = _hour7Task;
            hour8Task = _hour8Task;
        }
    }
}