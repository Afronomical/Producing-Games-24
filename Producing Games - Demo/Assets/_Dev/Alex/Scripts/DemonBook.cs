using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DemonBook : MonoBehaviour
{
    [Header("Page Flip")]
    public GameObject[] pageArray;
    private int pageIndex;

    public float turnPageDelay;
    private float turnPageTimer;

    void Start()
    {
        //gameObject.SetActive(false); //Toggling the checklist
        pageArray[0].SetActive(true);
    }

    private void Update()
    {
        turnPageTimer = Time.deltaTime;
    }

    public void OnReadBook(InputAction.CallbackContext context)
    {
        if(DiegeticUIManager.Instance.hasDemonBook)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public void OnFlipPage(InputAction.CallbackContext context)
    {
        if (context.performed && gameObject.activeSelf)
        {
            pageArray[pageIndex].SetActive(false);

            if (context.ReadValue<Vector2>().y > 0 && pageIndex < pageArray.Length - 1)
            {
                ++pageIndex;
            }
            else if (context.ReadValue<Vector2>().y < 0 && pageIndex > 0)
            {
                --pageIndex;
            }

            pageArray[pageIndex].SetActive(true);
            turnPageTimer = 0;
        }
    }
}
