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
    public SoundEffect pageSound;
    public TMP_Text bookTooltip;
    public PlayerArms arms;

    void Start()
    {
        pageArray[0].SetActive(true);
        bookTooltip.enabled = true;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        turnPageTimer = Time.deltaTime;

        if (DiegeticUIManager.Instance.hasDemonBook) bookTooltip.enabled = true;
    }

    public void OnReadBook(InputAction.CallbackContext context)
    {
        if(DiegeticUIManager.Instance.hasDemonBook)
        {
            if (context.performed)
                arms.HoldBook();
            if (context.canceled)
                arms.DropBook();

            bookTooltip.gameObject.SetActive(false);
        }
    }

    public void OnFlipPage(InputAction.CallbackContext context)
    {
        if (context.performed && gameObject.activeSelf)
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
}
