using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    private GameObject Door;
    private GameObject FakeDoor;
    private GameObject TaskManager;
    private GameObject Altar;
    private GameObject FakeAltar;
    public Transform demonRage;
    public int stage = 0;

    private void Start()
    {
        Door = transform.Find("Study Door").gameObject;
        FakeDoor = transform.Find("Fake Door").gameObject;
        Door.SetActive(false);
        Altar = GameObject.Find("AltarObjects");
        FakeAltar = Altar.transform.Find("FakeAltar").gameObject;
        Altar = Altar.transform.Find("Altar").gameObject;
        Altar.SetActive(false);
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

        
        if(stage == 5)
        {
            Debug.Log("Attempting");
            GameManager.Instance.demon.SetActive(true);
            Debug.Log(GameManager.Instance.demon.activeInHierarchy);
            GameManager.Instance.demon.GetComponent<DemonCharacter>().ChangeDemonState(DemonCharacter.DemonStates.Patrol);
            stage++;
        }
        
        if(stage == 6 && GameManager.Instance.currentHour == 3)
        {
            GameManager.Instance.demon.SetActive(false);
            Altar.SetActive(true);
            FakeAltar.SetActive(false);
            stage++;
        }
    }
}
