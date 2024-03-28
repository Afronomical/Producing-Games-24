using UnityEngine;
using static ObjectiveTracker;

public class Tutorialmanager : MonoBehaviour
{

    public ObjectiveTracker tracker;
    public GameObject objectiveTracker;


    public GameObject paient1Door, paient2Door, paient3Door, storageDoor, mainHallDoor;



    public void Start()
    {

        tracker = objectiveTracker.GetComponent<ObjectiveTracker>();
    }







    public void OnMedicineTask()
    {
        tracker.AddObjective("Order Medicine at Computer", objectiveTypes.Main);

    }
    public void onMedicineBuy()
    {

        tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Start Shift", objectiveTypes.Main);
    }
    public void OnShiftStartTask()
    {
        tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Check on Patient 1", objectiveTypes.Main);

    }
    public void OnPatient1Task()
    {
        tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Collect Medicine from Storage", objectiveTypes.Main);
    }
    public void OnCollectMedicine()
    {
        tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
    }
    public void OnPatient2Task()
    {

        tracker.AddObjective("Attend to patient 2", objectiveTypes.Main);
    }
    public void OnAttendToPatient3Task()
    {
        tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Attend to patient 3", objectiveTypes.Main);
    }
    public void OnRageMode()
    {
       tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Get back to Study", objectiveTypes.Main);
    }
    public void OnEndShiftTask()
    {
         tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("End shift.", objectiveTypes.Main);

    }
    public void OnDemonBookTask()
    {
        tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Get Exorcism GuideBook.", objectiveTypes.Main);
    }
    public void OnHeartAttackTask()
    {
        tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Save patient 2 from heart attack.", objectiveTypes.Main);
    }
    public void OnHidingTask()
    {
         tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Find where patient 3 is hiding.", objectiveTypes.Main);
    }
    public void OnOrganPlayTask()
    {
        tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Play organ to activate altar", objectiveTypes.Main);
    }
    public void OnExorciseDemonTask()
    {
        tracker.RemoveObjective(ObjectiveTracker.instance.mainObj);
        tracker.AddObjective("Place exorcism items on altar", objectiveTypes.Main);
    }
}
