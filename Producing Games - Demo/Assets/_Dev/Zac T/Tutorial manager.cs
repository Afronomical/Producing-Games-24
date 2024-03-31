using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static DemonCharacter;
using static DoorInteractable;
using static ObjectiveTracker;

class Tutorialmanager : MonoBehaviour
{
    public static Tutorialmanager instance;

    public ObjectiveTracker tracker;
    public GameObject objectiveTracker;

    public GameManager manager;
    public GameObject gameManager;

    public GameObject demon;
    public DemonCharacter dCharacter;

    public bool startshift = false, demonCanRage, nexthour, demonbook;

    public DoorInteractable dManager1, dManager2, dManager3, dManagerStorage, dManagerHall;
    public GameObject paient1Door, paient2Door, paient3Door, storageDoor, mainHallDoor;

    public GameObject mainDoorblock,patient1doorblock,patient2doorblock,patient3doorblock,storagedoorblock,halldoorblock;

    public bool clipBoardChecked = false;
    public bool shiftStarted = false;
    public bool medsCollected = false;
    public bool gotBacktoStudy = false;

    string objectiveToAdd;

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
        if(instance != null)
            instance = this;

        OnMedicineTask();
    }

    public void Update()
    {
        if ( Input.GetKeyDown(KeyCode.F) && shiftStarted ) 
        {
            OnCollectMedicine();
        }

        if (manager.currentHour == 2)
        {
            mainDoorblock.SetActive(true);
            demon.SetActive(false);
            gotBacktoStudy = true;
        }

        if (gotBacktoStudy)
        {
            tracker.AddObjective("Find the demon book");
            gotBacktoStudy = false;
        }

        if (ObjectiveTracker.instance.removed)
        {
            tracker.AddObjective(objectiveToAdd);
        }

        // tracker.DisplayObjectives();
    }


    public void OnMedicineTask()
    {
        //tracker.AddObjective("Order Medicine at Computer");
        objectiveToAdd = "Order Medicine at Computer";
    }
    public void onMedicineBuy()
    {
        // Task1 = true;
        startshift = true;
        mainDoorblock.SetActive(false);
        ObjectiveTracker.instance.removed = false;
        objectiveToAdd = "Start Shift";
    }
    public void OnShiftStartTask()
    {
        //Task2 = true;
        shiftStarted = true;
        ObjectiveTracker.instance.removed = false;
        objectiveToAdd = "Check patient 1 tasks on clipbaord";
    }

    public void OnCollectMedicine()
    {
        storagedoorblock.SetActive(false);
        tracker.AddObjective("Collect medicine from storage");
    }
    public void OnPatient1Task()
    {
        // Task3 = true;
        // dManagerStorage.ChangeDoorState(DoorStates.Shut);
        
        
        
            
            tracker.AddObjective("Find and Give patient 1 required medication");
            patient1doorblock.SetActive(false);
        
    }
    //public void OnCollectMedicine()
    //{
       // Task4 = true;
       // tracker.AddObjective("Take medicine to patient ", objectiveTypes.Main);
    //}
    public void OnPatient2Task()
    {
       // Task5 = true;
        patient2doorblock.SetActive(false);
        tracker.AddObjective("Use F clipboard and attend to patient 2");
    }
    public void OnAttendToPatient3Task()
    {
       // Task6 = true;
       // dManager3.ChangeDoorState(DoorStates.Shut);
        tracker.AddObjective("Attend to patient 3");
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
        tracker.AddObjective("Get back to Study");
        }
    }
    public void OnEndShiftTask()
    {
        if(demonCanRage)
        {
            tracker.AddObjective("End shift at bed.");
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
            tracker.AddObjective("Get Exorcism GuideBook.");
            demonbook = true;
        }
    }
    public void OnHeartAttackTask()
    {
        // Task10 = true;
        if (demonbook)
        {
            demonbook = true;
            tracker.AddObjective("Save patient 2 from heart attack.");
        }
    }
    public void OnHidingTask()
    {
        // Task11 = true;
        if (demonbook)
        {
            tracker.AddObjective("Find where patient 3 is hiding and escort them to bed.");
        }
       
    }
    public void OnOrganPlayTask()
    {
        if (demonbook)
        {
        // Task12 = true;
        mainDoorblock.SetActive(false);
        tracker.AddObjective("Play organ to activate altar");
        }
    }
    public void OnExorciseDemonTask()
    {
       // Task13 = true;
        tracker.AddObjective("Place exorcism items on altar" +
            "Use B to look at Guidebook");
    }
    public IEnumerator WaitForOBJ(float waitBetweenOjectives)
    {
        yield return new WaitForSeconds(waitBetweenOjectives);

    }
}
