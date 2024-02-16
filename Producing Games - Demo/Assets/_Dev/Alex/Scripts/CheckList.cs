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
    public GameObject pageOne;
    public GameObject pageTwo;
    public GameObject pageThree;

    private GameObject taskParent;

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
        pageTwo.SetActive(false);
        pageThree.SetActive(false);
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

    public void OnFlipPage(InputAction.CallbackContext context)
    {
        if(pageOne.activeSelf)
        {
            pageOne.SetActive(false);
            pageTwo.SetActive(true);
        }
        else if(pageTwo.activeSelf)
        {
            pageTwo.SetActive(false);
            pageThree.SetActive(true);
        }
        else if(pageThree.activeSelf)
        {
            pageThree.SetActive(false);
            pageOne.SetActive(true);
        }
    }

    public void AddTask(Task task)
    {
        //Adding it on list
        if(taskList.Count <= 3)
        {
            taskParent = pageOne;
        }
        else if(taskList.Count <= 6)
        {
            taskParent = pageTwo;
        }
        else { taskParent = pageThree;}

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
