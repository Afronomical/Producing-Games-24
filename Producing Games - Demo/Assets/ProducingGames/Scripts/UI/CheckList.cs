using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    [Header("Instantiating Tasks")]
    private List<GameObject> taskList = new List<GameObject>(); //Used for the task prefabs 
    private GameObject taskParent;
    public List<GameObject> taskLayouts; //Used for where the tasks appear
    public GameObject taskPrefab;
    //public Image tick;

    [Header("Page Flipping")]
    public GameObject[] pageArray;
    private int pageIndex;

    public float turnPageDelay;
    private float turnPageTimer;
    public SoundEffect pageSound;

    public PlayerArms arms;

    public static CheckList instance;
    public TMP_Text listTooltip;

    //private int timer = 100;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false); //Toggling the checklist
        pageArray[0].SetActive(true);
        listTooltip.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        turnPageTimer += Time.deltaTime;

        if (DiegeticUIManager.Instance.hasChecklist) listTooltip.enabled = true;

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
        if(DiegeticUIManager.Instance.hasChecklist)
        {
            if (context.performed)
                arms.HoldClipboard();
            if (context.canceled)
                arms.DropClipboard();

            listTooltip.gameObject.SetActive(false);
        }
    }

    public void OnFlipPage(InputAction.CallbackContext context)
    {
        if (context.performed && turnPageTimer >= turnPageDelay && gameObject.activeSelf)
        {
            pageArray[pageIndex].SetActive(false);

            if (context.ReadValue<Vector2>().y < 0 && pageIndex < pageArray.Length - 1)
            {
                ++pageIndex;
                AudioManager.instance.PlaySound(pageSound, null);
            }
            else if (context.ReadValue<Vector2>().y > 0 && pageIndex > 0)
            {
                --pageIndex;
                AudioManager.instance.PlaySound(pageSound, null);
            }

            pageArray[pageIndex].SetActive(true);
            turnPageTimer = 0;
        }
    }

    public void AddTask(Task task)
    {
        //Adding it on list
        if (task.taskTarget == NPCManager.Instance.patientList[0])
        {
            taskParent = taskLayouts[0];
        }
        else if (task.taskTarget == NPCManager.Instance.patientList[1])
        {
            taskParent = taskLayouts[1];
        }
        else if (task.taskTarget == NPCManager.Instance.patientList[2])
        {
            taskParent = taskLayouts[2];
        }
        else if (task.taskTarget == NPCManager.Instance.patientList[3])
        {
            taskParent = taskLayouts[3];
        }
        else taskParent = taskLayouts[4];

        GameObject newTask = GameObject.Instantiate(taskPrefab, taskParent.transform.position, taskParent.transform.rotation);
        newTask.transform.SetParent(taskParent.transform);
        newTask.GetComponent<RectTransform>().localScale = Vector3.one;

        TMP_Text newText = newTask.GetComponent<TMP_Text>();

        //Setting up text
        newText.text = "";
        if (task.isHourlyTask)
            newText.text += task.hTask.taskName;
        else
            newText.text += task.rTask.taskName;
        //tick.enabled = false; //Check mark for completion of task
        newText.color = Color.black;
        //newText.fontStyle = FontStyles.Normal;

        task.checkList = newTask;


        taskList.Add(newTask);
    }

    public void RemoveTask(Task task)
    {
        Destroy(task.checkList);
    }

    public void CompleteTask(Task task)
    {
        TMP_Text newText = task.checkList.GetComponent<TMP_Text>();
        newText.color = Color.grey;
        newText.fontStyle = FontStyles.Strikethrough;
    }
}
