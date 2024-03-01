using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterFountain : MonoBehaviour
{
    private float radius = 2f;
    public bool isJugFull;
    private bool placed;
    public Transform fillPos;
    public float fillTime;
    public int maxUsesPerHour;
    public int timesUsedThisHour = 0;
    public int FountainCapacity;
    public int fillDrain;
    public SoundEffect fillSound;

    private Collider jugCollider;

    private void Start()
    {
        isJugFull = false;
    }
    private void Update()
    {
        JugInRadius();
    }


    void JugInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider collider in colliders)
        {
            if(collider.gameObject.TryGetComponent(out PickUpJug HolyJug))
            {
               jugCollider = collider;
                Debug.Log("Jug detected"); 
                if(timesUsedThisHour < maxUsesPerHour && !isJugFull && !placed && FountainCapacity >= fillDrain)
                {
                    //TooltipManager.Instance.ShowTooltip("Press H to confirm Jug fill");
                    collider.gameObject.GetComponent<InteractableTemplate>().collectible.tooltipText = ("Press H to confirm Jug Fill");
                    if (Input.GetKeyDown(KeyCode.H))
                    {
                        TooltipManager.Instance.HideTooltip();
                        
                        placed = true;
                        collider.gameObject.transform.position = fillPos.transform.position;
                        collider.gameObject.GetComponent<InteractableTemplate>().hasBeenPlaced = true;
                       
                        StartCoroutine(StartFill());
                        collider.gameObject.GetComponent<PickUpJug>().capacity += fillDrain;
                        collider.gameObject.GetComponent<Rigidbody>().mass *= 4;
                        collider.gameObject.GetComponent<InteractableTemplate>().collectible.tooltipText = ("Pick up Holy Water Jug");
                        ++timesUsedThisHour;
                       
                        //collider.gameObject.GetComponent<PickUpJug>().enabled = true;
                        break;
                    }
                }
                else
                {
                    //TooltipManager.Instance.HideTooltip();
                }
                
            }
            else
            {
                //TooltipManager.Instance.ShowTooltip("Jug cannot be filled at this time");
            }
        }
    }

    public IEnumerator StartFill()
    {
        Debug.Log("Jug Filling");
        AudioManager.instance.PlaySound(fillSound, transform);
        yield return new WaitForSeconds(fillTime);
        Debug.Log("Jug Full");
        isJugFull = true;
        jugCollider.GetComponent<InteractableTemplate>().hasBeenPlaced = false;
        FountainCapacity -= fillDrain;
        
        //collider.GetComponent<InteractableTemplate>().enabled = true;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    

}
