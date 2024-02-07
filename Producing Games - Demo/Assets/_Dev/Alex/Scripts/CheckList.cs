using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    private List<GameObject> taskList = new List<GameObject>();
    //public Image tick;
    public GameObject taskParent;

    public GameObject taskPrefab;

    public static CheckList instance;

    //private int timer = 100;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false); //Toggling the checklist
    }

    // Update is called once per frame
    void Update()
    {

        //if(timer <= 0)
        //{
        //    tick.enabled = true;
        //    taskText[0].color = Color.gray;
        //    taskText[0].fontStyle = FontStyles.Strikethrough;
        //}
        //else timer--;
        //print(timer);
    }

    public void OnTasksInput(InputAction.CallbackContext context)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void AddTask(Task task)
    {
        //Adding it on list
        
        GameObject newTask = GameObject.Instantiate(taskPrefab, taskParent.transform.position, taskParent.transform.rotation);
        newTask.transform.SetParent(taskParent.transform);
        newTask.GetComponent<RectTransform>().localScale = Vector3.one;

        TMP_Text newText = newTask.GetComponent<TMP_Text>();

        //Setting up text
        newText.text = task.taskTarget.name;
        newText.text += " - ";
        if (task.isHourlyTask)
            newText.text += task.hTask.taskName;
        else
            newText.text += task.rTask.taskName;
        //tick.enabled = false; //Check mark for completion of task
        newText.color = Color.white;
        newText.fontStyle = FontStyles.Normal;

        task.checkList = newTask;

        taskList.Add(newTask);
    }

    public void RemoveTask(Task task)
    {
        Destroy(task.checkList);
    }


}
