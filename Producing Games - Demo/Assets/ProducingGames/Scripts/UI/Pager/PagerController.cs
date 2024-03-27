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

    public bool ActiveNotif = false;
    public GameObject PagerLight;
    public GameObject FPagerLight;

    private void Start()
    {
        offset = pagerInterface.transform.localPosition;
        pagerTooltip.text = "Hold Q to switch to pager!";
        pagerTooltip.enabled = false;
    }

    private void Update()
    {
        //pagerInterface.GetComponent<RectTransform>().position = transform.position + offset;

        if (DiegeticUIManager.Instance.hasPager) pagerTooltip.enabled = true;
        if (ActiveNotif)
        {
            if (arms.holdingPager)
            {
                PagerLight.SetActive(true);
                FPagerLight.SetActive(false);
            } else
            {
                PagerLight.SetActive(false);
                FPagerLight.SetActive(true);
            }
        }
        else
        {
            PagerLight.SetActive(false);
            FPagerLight.SetActive(false);
        }
    }

    public void OnPagerInput(InputAction.CallbackContext context)
    {
        if (pagerInterface != null && DiegeticUIManager.Instance.hasPager && !DiegeticUIManager.Instance.pagerBroken)
        {
            if (context.performed)
                arms.HoldPager();
            if (context.canceled)
                arms.DropPager();

            pagerTooltip.gameObject.SetActive(false);

            //if (!arms.holdingPager) arms.HoldPager();
            //else arms.DropPager();
        }
    }
}
