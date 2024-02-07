using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    //public TMP_Text task;
    //public Image tick;

    private int timer = 100;
    private int taskAmount = 0;

    public PatientTaskManager patientTaskManager;
    public GameObject task;

    //private void Awake()
    //{
    //    tick = GetComponent<Image>();
    //    task = GetComponent<TMP_Text>();
    //}

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        //taskAmount = patientTaskManager.currentTasks.Length();

        //tick.enabled = false;
        //task.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

        //foreach(var t in patientTaskManager.currentTasks)
        {

            //Add tasks dynamically to checklist
            //if(currentTasks.Length() > taskAmount)
            {
                //Add (this task) prefab and overwrite text
            }
            //else if(currentTasks.Length() < taskAmount)
            {
                //Remove (this) tasks that are no longer relevant
            }
        }

        if(timer <= 0)
        {
            //tick.enabled = true;
            //task.color = Color.gray;
        }
        else timer--;
        print(timer);
    }

    public void OnTasksInput(InputAction.CallbackContext context)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
