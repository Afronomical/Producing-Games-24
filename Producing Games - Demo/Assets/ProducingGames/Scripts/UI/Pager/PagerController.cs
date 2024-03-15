using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PagerController : MonoBehaviour
{
    public GameObject pagerInterface;
    public PlayerArms arms;
    Vector3 offset;
    public TMP_Text pagerTooltip;

    private void Start()
    {
        offset = pagerInterface.transform.localPosition;
        pagerTooltip.text = "Hold Q to switch to pager!";
    }

    private void Update()
    {
        //pagerInterface.GetComponent<RectTransform>().position = transform.position + offset;
    }

    public void OnPagerInput(InputAction.CallbackContext context)
    {
        if (pagerInterface != null && DiegeticUIManager.Instance.hasPager && !DiegeticUIManager.Instance.pagerBroken)
        {
            if (context.performed)
                arms.HoldPager();
            if (context.canceled)
                arms.DropPager();

            pagerTooltip.enabled = false;

            //if (!arms.holdingPager) arms.HoldPager();
            //else arms.DropPager();
        }
    }
}
