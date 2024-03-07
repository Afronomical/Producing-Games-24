using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    private GameObject Door;
    private GameObject FakeDoor;
    private GameObject TaskManager;
    public Transform demonRage;
    public int stage = 0;

    private void Start()
    {
        Door = transform.Find("Study Door").gameObject;
        FakeDoor = transform.Find("Fake Door").gameObject;
        Door.SetActive(false);
        TaskManager = GameObject.Find("TaskManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (DiegeticUIManager.Instance.hasChecklist && DiegeticUIManager.Instance.hasDemonBook && DiegeticUIManager.Instance.hasPager && stage == 0)
        {
            Door.SetActive(true);
            FakeDoor.SetActive(false);
            stage++;
        }

        if(stage == 1 && GameManager.Instance.currentTime > 0.06f)
        {
            Door.SetActive(false);
            FakeDoor.SetActive(true);
            stage++;
        }

        if (stage == 2) {
            Task[] CurrentTasks = TaskManager.GetComponents<Task>();
            for (int i = 0; i < CurrentTasks.Length; i++)
            {
                if (!CurrentTasks[i].taskCompleted)
                {
                    break;
                }
                if(i == CurrentTasks.Length - 1)
                {
                    Door.SetActive(true);
                    FakeDoor.SetActive(false);
                    stage++;
                }
            }
        }
        
        if (stage == 3 && GameManager.Instance.currentTime == 0)
        {
            Door.SetActive(false);
            FakeDoor.SetActive(true);
            stage++;
        }

        if (stage == 4 && GameManager.Instance.gameObject.GetComponent<EconomyManager>().boughtItems.Count > 0)
        {
            Door.SetActive(true);
            FakeDoor.SetActive(false);
            stage++;
        }

        /*
        if(stage == 5 && GameManager.Instance.currentTime > 0.6f)
        {
            GameManager.Instance.demon.SetActive(true);
            GameManager.Instance.demon.transform.position = demonRage.position;
            stage++;
        }
        */
    }
}
