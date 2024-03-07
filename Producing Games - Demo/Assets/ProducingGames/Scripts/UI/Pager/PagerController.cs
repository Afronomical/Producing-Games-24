using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PagerController : MonoBehaviour
{
    public GameObject pagerInterface;
    public PlayerArms arms;
    Vector3 offset;

    private void Start()
    {
        offset = pagerInterface.transform.localPosition;
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

            //if (!arms.holdingPager) arms.HoldPager();
            //else arms.DropPager();
        }
    }
}
