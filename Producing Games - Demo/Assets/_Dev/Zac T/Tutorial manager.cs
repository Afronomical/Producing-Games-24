using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static DoorInteractable;
using static ObjectiveTracker;

public class Tutorialmanager : MonoBehaviour
{

    public ObjectiveTracker tracker;
    public GameObject objectiveTracker;

    public bool startshift, demonCanRage, nexthour, demonbook;

    public DoorInteractable dManager1, dManager2, dManager3, dManagerStorage, dManagerHall;
    public GameObject paient1Door, paient2Door, paient3Door, storageDoor, mainHallDoor;

    public GameObject mainDoorblock,patient1doorblock,patient2doorblock,patient3doorblock,storagedoorblock,halldoorblock;

   // public bool Task1 = false,Task2 = false, Task3 = false, Task4 = false, Task5 = false, Task6 = false, Task7 = false, Task8= false, Task9 = false, Task10 = false, Task11 = false, Task12 = false, Task13 = false;
    public void Start()
    {
        dManager1 = paient1Door.GetComponent<DoorInteractable>();
        dManager2 = paient2Door.GetComponent<DoorInteractable>();
        dManager3 = paient3Door.GetComponent<DoorInteractable>();
        dManagerStorage = storageDoor.GetComponent<DoorInteractable>();
        dManagerHall = mainHallDoor.GetComponent<DoorInteractable>();

        
        tracker = objectiveTracker.GetComponent<ObjectiveTracker>();

        OnMedicineTask();
    }

    public void FixedUpdate()
    {
        
        // tracker.DisplayObjectives();
    }

    




    public void OnMedicineTask()
    {
        tracker.AddObjective("Order Medicine at Computer", objectiveTypes.Main);

    }
    public void onMedicineBuy()
    {

       // Task1 = true;
       startshift = true;
        mainDoorblock.SetActive(false);
        tracker.AddObjective("Start Shift", objectiveTypes.Main);
    }
    public void OnShiftStartTask()
    {
        //Task2 = true;

        storagedoorblock.SetActive(false);
        tracker.AddObjective("Collect Medicine from Storage", objectiveTypes.Main);
    }
    public void OnPatient1Task()
    {
        // Task3 = true;
        // dManagerStorage.ChangeDoorState(DoorStates.Shut);
        patient1doorblock.SetActive(false);
        tracker.AddObjective("Find and Give patient 1 required medication", objectiveTypes.Main);

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
        tracker.AddObjective("Use F clipboard and attend to patient 2", objectiveTypes.Main);
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

        
        // Task7 = true;
       // dManager1.ChangeDoorState(DoorStates.Shut);
       // dManager2.ChangeDoorState(DoorStates.Shut);
        //dManager3.ChangeDoorState(DoorStates.Shut);
       // dManagerStorage.ChangeDoorState(DoorStates.Shut);
        patient1doorblock.SetActive(true);
        patient2doorblock.SetActive(true);
        patient3doorblock.SetActive(true);
        storagedoorblock.SetActive(true);
        tracker.AddObjective("Get back to Study", objectiveTypes.Main);
        }
    }
    public void OnEndShiftTask()
    {
        if(demonCanRage)
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
            demonbook = true;
        }
    }
    public void OnHeartAttackTask()
    {
        // Task10 = true;
        if (demonbook)
        {
            demonbook = true;
            tracker.AddObjective("Save patient 2 from heart attack.", objectiveTypes.Main);
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
        tracker.AddObjective("Play organ to activate altar", objectiveTypes.Main);
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
