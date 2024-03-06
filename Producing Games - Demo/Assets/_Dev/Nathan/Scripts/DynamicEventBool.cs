using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEventBool : MonoBehaviour
{
    GameManager GM;
    ThrownItems thrownItems;
    ShakeOnSpot ShakeOnSpot;
    FOV_Bounce FOV_Bounce;
    Floating_Items Floating_Items;
    CrossBehaviour CrossBehaviour;
    CamShakeOnTrigger CamShakeOnTrigger;

    void Start()
    {
        GM = GameManager.Instance;
    }

    public void resetDynamicEventBool()
    {
        thrownItems.eventTriggered = false;
        ShakeOnSpot.eventTriggered = false;
        FOV_Bounce.eventTriggered = false;
        Floating_Items.eventTriggered = false;
        CrossBehaviour.eventTriggered = false;
        CamShakeOnTrigger.eventTriggered = false;
    }
}
