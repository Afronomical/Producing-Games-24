using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static ObjectiveTracker;

public class Tutorialmanager : MonoBehaviour
{

    public ObjectiveTracker tracker;
    public GameObject objectiveTracker;


    public GameObject paient1Door, paient2Door, paient3Door, storageDoor, mainHallDoor;

    public GameObject mainDoorblock,patient1doorblock,patient2doorblock,patient3doorblock,storagedoorblock,halldoorblock;

    public void Start()
    {

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

      
        mainDoorblock.SetActive(false);
        tracker.AddObjective("Start Shift", objectiveTypes.Main);
    }
    public void OnShiftStartTask()
    {
        patient1doorblock.SetActive(false);
        tracker.AddObjective("Check on Patient 1", objectiveTypes.Main);

    }
    public void OnPatient1Task()
    {
       storagedoorblock.SetActive(false);
        tracker.AddObjective("Collect Medicine from Storage", objectiveTypes.Main);
    }
    public void OnCollectMedicine()
    {
        tracker.AddObjective(" ", objectiveTypes.Main);
    }
    public void OnPatient2Task()
    {
        patient2doorblock.SetActive(false);
        tracker.AddObjective("Attend to patient 2", objectiveTypes.Main);
    }
    public void OnAttendToPatient3Task()
    {
        patient3doorblock.SetActive(false);
        tracker.AddObjective("Attend to patient 3", objectiveTypes.Main);
    }
    public void OnRageMode()
    {
       
        tracker.AddObjective("Get back to Study", objectiveTypes.Main);
    }
    public void OnEndShiftTask()
    {
        
        tracker.AddObjective("End shift.", objectiveTypes.Main);

    }
    public void OnDemonBookTask()
    {
       
        tracker.AddObjective("Get Exorcism GuideBook.", objectiveTypes.Main);
    }
    public void OnHeartAttackTask()
    {
      
        tracker.AddObjective("Save patient 2 from heart attack.", objectiveTypes.Main);
    }
    public void OnHidingTask()
    {

        tracker.AddObjective("Find where patient 3 is hiding.", objectiveTypes.Main);
    }
    public void OnOrganPlayTask()
    {
        mainDoorblock.SetActive(false);
        tracker.AddObjective("Play organ to activate altar", objectiveTypes.Main);
    }
    public void OnExorciseDemonTask()
    {
       
        tracker.AddObjective("Place exorcism items on altar", objectiveTypes.Main);
    }
    public IEnumerator WaitForOBJ(float waitBetweenOjectives)
    {
        yield return new WaitForSeconds(waitBetweenOjectives);

    }
}
