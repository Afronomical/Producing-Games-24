using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    private GameObject Door;
    private GameObject FakeDoor;

    private void Start()
    {
        Door = transform.Find("Study Door").gameObject;
        FakeDoor = transform.Find("Fake Door").gameObject;
        Door.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DiegeticUIManager.Instance.hasChecklist && DiegeticUIManager.Instance.hasDemonBook && DiegeticUIManager.Instance.hasPager)
        {
            Door.SetActive(true);
            FakeDoor.SetActive(false);
        }
    }
}
