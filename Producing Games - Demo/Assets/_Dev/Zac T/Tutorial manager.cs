using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static DemonCharacter;
using static DoorInteractable;
using static ObjectiveTracker;

public class Tutorialmanager : MonoBehaviour
{

    public ObjectiveTracker tracker;
    public GameObject objectiveTracker;

    public GameManager manager;
    public GameObject gameManager;

    public GameObject demon;
    public DemonCharacter dCharacter;

    public GameObject patient1Trigger, patient2Trigger, patient3Trigger, demonbookTrigger;
    public GameObject demonBookAsset;

    public bool startshift , demonCanRage, nexthour, demonbook;

    public DoorInteractable dManager1, dManager2, dManager3, dManagerStorage, dManagerHall;
    public GameObject paient1Door, paient2Door, paient3Door, storageDoor, mainHallDoor;

    public GameObject mainDoorblock, patient1doorblock, patient2doorblock, patient3doorblock, storagedoorblock, halldoorblock,demonBookBlock;

    public bool clipBoardChecked = false;
    public bool shiftStarted = false;
    public bool medsCollected = false;
    public bool gotBacktoStudy = false;
    public bool nextHourStarted = true;

  

    // public bool Task1 = false,Task2 = false, Task3 = false, Task4 = false, Task5 = false, Task6 = false, Task7 = false, Task8= false, Task9 = false, Task10 = false, Task11 = false, Task12 = false, Task13 = false;
    private void Awake()
    {
        dManager1 = paient1Door.GetComponent<DoorInteractable>();
        dManager2 = paient2Door.GetComponent<DoorInteractable>();
        dManager3 = paient3Door.GetComponent<DoorInteractable>();
        dManagerStorage = storageDoor.GetComponent<DoorInteractable>();
        dManagerHall = mainHallDoor.GetComponent<DoorInteractable>();
        dCharacter = demon.GetComponent<DemonCharacter>();

        tracker = objectiveTracker.GetComponent<ObjectiveTracker>();
    }

    public void Start()
    {


        OnMedicineTask();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && shiftStarted)
        {
            onCollectMedicine();
            shiftStarted = true;
        }

        

        if (manager.currentHour == 2 && nextHourStarted)
        {  
            gotBacktoStudy = true;
        }

        if (gotBacktoStudy)
        {
            mainDoorblock.SetActive(true);
            demon.SetActive(false);
            demonbookTrigger.SetActive(true);
            tracker.AddObjective("Find the demon book", objectiveTypes.Main);
            gotBacktoStudy = false;
            nextHourStarted = false;
        }

        // tracker.DisplayObjectives();
    }

    

    private void OnTriggerExit(Collider other)
    {
        
    }
    public void OnMedicineTask()
    {
        tracker.AddObjective("Order Medicine at Computer", objectiveTypes.Main);
        startshift = false;
    }
    public void onMedicineBuy()
    {

        // Task1 = true;
        if(startshift==false)
        {
        startshift = true;
        mainDoorblock.SetActive(false);
        tracker.AddObjective("Start Shift", objectiveTypes.Main);
        }
        
    }
    public void startHourTwo()
    {
        demonBookAsset.SetActive(false); 
        mainDoorblock.SetActive(false);
        demonbookTrigger.SetActive(false) ;
        tracker.AddObjective("Start Shift", objectiveTypes.Main);
    }
    public void OnShiftStartTask()
    {
        //Task2 = true;
        shiftStarted = true;
        tracker.AddObjective("Check patient 1 tasks on clipbaord", objectiveTypes.Main);

    }

    public void onCollectMedicine()
    {
        storagedoorblock.SetActive(false);
        tracker.AddObjective("Collect medicine from storage", objectiveTypes.Main);
    }
    public void OnPatient1Task()
    {
        // Task3 = true;
        // dManagerStorage.ChangeDoorState(DoorStates.Shut);




        tracker.AddObjective("Find and Give patient 1 required medication", objectiveTypes.Main);
        patient1doorblock.SetActive(false);

    }
    public void OnCollectMedicine()
    {
        // Task4 = true;
        // tracker.AddObjective("Take medicine to patient ", objectiveTypes.Main);
    }
    public void OnPatient2Task()
    {
        // Task5 = true;
        patient2doorblock.SetActive(false);
        tracker.AddObjective("Attend to patient 2", objectiveTypes.Main);
    }
    public void OnAttendToPatient3Task()
    {
        // Task6 = true;
        // dManager3.ChangeDoorState(DoorStates.Shut);
        tracker.AddObjective("Attend to patient 3", objectiveTypes.Main);
        demonCanRage = true;

    }
    public void OnRageMode()
    {

        if (demonCanRage)
        {
            dCharacter.SetInRageMode(true);
            dManager3.ChangeDoorState(DoorStates.Open);
            patient3doorblock.SetActive(false);
            dManager1.ChangeDoorState(DoorStates.Shut);
            dManager2.ChangeDoorState(DoorStates.Shut);
            demon.SetActive(true);
            dManagerStorage.ChangeDoorState(DoorStates.Shut);
            patient1doorblock.SetActive(true);
            patient2doorblock.SetActive(true);

            storagedoorblock.SetActive(true);
            tracker.AddObjective("Get back to Study", objectiveTypes.Main);
        }
    }
    public void OnEndShiftTask()
    {
        if (demonCanRage)
        {
            tracker.AddObjective("End shift at bed.", objectiveTypes.Main);
            nexthour = true;
        }
        // Task8 = true;


    }
    public void OnDemonBookTask()
    {
        if (nexthour)
        {
            // Task9 = true;
            patient1doorblock.SetActive(false);
            patient2doorblock.SetActive(false);
            patient3doorblock.SetActive(false);
            storagedoorblock.SetActive(false);
            tracker.AddObjective("Get Exorcism GuideBook.", objectiveTypes.Main);
            demonBookBlock.SetActive(false);
           
        }
    }
    public void OnHeartAttackTask()
    {
        // Task10 = true;
        if (nexthour)
        {
            demonbook = true;
            tracker.AddObjective("Save patient 1 from heart attack.", objectiveTypes.Main);
        }
    }
    public void OnRequestMedicationTask()
    {
        // Task10 = true;
        if (demonbook)
        {
            demonbook = true;
            tracker.AddObjective("Attend to patient 2 take note their activity", objectiveTypes.Main);
        }
    }
    public void OnHidingTask()
    {
        // Task11 = true;
        if (demonbook)
        {
            tracker.AddObjective("Find where patient 3 is hiding and escort them to bed.", objectiveTypes.Main);
        }

    }
    public void OnOrganPlayTask()
    {
        if (demonbook)
        {
            // Task12 = true;
            mainDoorblock.SetActive(false);
            tracker.AddObjective("Enter main hall and play organ to activate exorcism", objectiveTypes.Main);
        }
    }
    public void OnExorciseDemonTask()
    {
        // Task13 = true;
        tracker.AddObjective("Place exorcism items on altar" +
            "Use B to look at Guidebook", objectiveTypes.Main);
    }
    public IEnumerator WaitForOBJ(float waitBetweenOjectives)
    {
        yield return new WaitForSeconds(waitBetweenOjectives);

    }
}