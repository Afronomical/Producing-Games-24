using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static DoorInteractable;
using static ObjectiveTracker;

public class Tutorialmanager : MonoBehaviour
{

    public ObjectiveTracker tracker;
    public GameObject objectiveTracker;

    

    public DoorInteractable dManager1, dManager2, dManager3, dManagerStorage, dManagerHall;
    public GameObject paient1Door, paient2Door, paient3Door, storageDoor, mainHallDoor;

    public GameObject mainDoorblock,patient1doorblock,patient2doorblock,patient3doorblock,storagedoorblock,halldoorblock;

    public bool Task1 = false,Task2 = false, Task3 = false, Task4 = false, Task5 = false, Task6 = false, Task7 = false, Task8= false, Task9 = false, Task10 = false, Task11 = false, Task12 = false, Task13 = false;
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

        Task1 = true;
        mainDoorblock.SetActive(false);
        tracker.AddObjective("Start Shift", objectiveTypes.Main);
    }
    public void OnShiftStartTask()
    {
        Task2 = true;
        dManager1.ChangeDoorState(DoorStates.Shut);
        tracker.AddObjective("Check Patient 1 task", objectiveTypes.Main);

    }
    public void OnPatient1Task()
    {
        Task3 = true;
        dManagerStorage.ChangeDoorState(DoorStates.Shut);
        tracker.AddObjective("Collect Medicine from Storage", objectiveTypes.Main);
    }
    public void OnCollectMedicine()
    {
        Task4 = true;
        tracker.AddObjective("Take medicine to patient ", objectiveTypes.Main);
    }
    public void OnPatient2Task()
    {
        Task5 = true;
        dManager2.ChangeDoorState(DoorStates.Shut);
        tracker.AddObjective("Attend to patient 2", objectiveTypes.Main);
    }
    public void OnAttendToPatient3Task()
    {
        Task6 = true;
        dManager3.ChangeDoorState(DoorStates.Shut);
        tracker.AddObjective("Attend to patient 3", objectiveTypes.Main);
    }
    public void OnRageMode()
    {
        Task7 = true;
        dManager1.ChangeDoorState(DoorStates.Locked);
        dManager2.ChangeDoorState(DoorStates.Locked);
        dManager3.ChangeDoorState(DoorStates.Locked);
        dManagerStorage.ChangeDoorState(DoorStates.Locked);
        tracker.AddObjective("Get back to Study", objectiveTypes.Main);
    }
    public void OnEndShiftTask()
    {
        Task8 = true;
        tracker.AddObjective("End shift.", objectiveTypes.Main);

    }
    public void OnDemonBookTask()
    {
        Task9 = true;
        tracker.AddObjective("Get Exorcism GuideBook.", objectiveTypes.Main);
    }
    public void OnHeartAttackTask()
    {
        Task10 = true;
        tracker.AddObjective("Save patient 2 from heart attack.", objectiveTypes.Main);
    }
    public void OnHidingTask()
    {
        Task11 = true;
        tracker.AddObjective("Find where patient 3 is hiding.", objectiveTypes.Main);
    }
    public void OnOrganPlayTask()
    {
        Task12 = true;
        mainDoorblock.SetActive(false);
        tracker.AddObjective("Play organ to activate altar", objectiveTypes.Main);
    }
    public void OnExorciseDemonTask()
    {
        Task13 = true;
        tracker.AddObjective("Place exorcism items on altar", objectiveTypes.Main);
    }
    public IEnumerator WaitForOBJ(float waitBetweenOjectives)
    {
        yield return new WaitForSeconds(waitBetweenOjectives);

    }
}
