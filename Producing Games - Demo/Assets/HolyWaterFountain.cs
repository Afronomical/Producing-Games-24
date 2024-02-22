using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWaterFountain : MonoBehaviour
{
    private float radius = 2f;
    public bool isJugFull;
    public Transform fillPos;
    public float fillTime;
    public int maxUsesPerHour;
    public int timesUsedThisHour = 0;



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
                Debug.Log("Jug detected"); 
                if(timesUsedThisHour < maxUsesPerHour && !isJugFull)
                {
                    TooltipManager.Instance.ShowTooltip("Press H to confirm Jug fill");
                    if (Input.GetKeyDown(KeyCode.H))
                    {
                        collider.gameObject.transform.position = fillPos.transform.position;
                        //collider.gameObject.GetComponent<PickUpJug>().enabled = false;
                        StartCoroutine(StartFill());
                        ++timesUsedThisHour;
                        //collider.gameObject.GetComponent<PickUpJug>().enabled = true;
                        break;
                    }
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
        yield return new WaitForSeconds(fillTime);
        Debug.Log("Jug Full"); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
