using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlashlightLEDs : MonoBehaviour
{
    public Flashlight flashlight;
    

    MeshRenderer meshRenderer;

    [SerializeField] Material chargedMat;
    
    [SerializeField] Material deadMat;

    public float TimeBetweenImpulses;

    bool canFlicker = true;

    public LEDState ledState = LEDState.Charged;
    public enum LEDState
    {
        Charged, Flashing, Dead
    }
    
    IEnumerator Flicker(Material firstColour, Material colourToFlashTo)
    {
            yield return new WaitForSeconds(TimeBetweenImpulses / 2);
             canFlicker = false;
            meshRenderer.material = firstColour;
            Debug.Log("Flashlight LED set flicker ON");

            yield return new WaitForSeconds(TimeBetweenImpulses / 2);
            meshRenderer.material = colourToFlashTo;
            Debug.Log("Flashlight LED set flicker OFF");
        canFlicker = true;

            
       
    }

    private void Start()
    {
        flashlight = FindAnyObjectByType<Flashlight>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        ChangeLEDSettings(ledState);
    }

    public void ChangeLEDSettings(LEDState state)
    {
        ledState = state;

        switch (state)
        {
            case LEDState.Charged:

                StopAllCoroutines();
                canFlicker = true;
                meshRenderer.material = chargedMat;

                break;
            case LEDState.Flashing:
                if (canFlicker)
                {
                    StartCoroutine(Flicker(chargedMat, deadMat));  
                }

                break;


            case LEDState.Dead:
                StopAllCoroutines();
                canFlicker = true;
                meshRenderer.material = deadMat;

                break;
            default:
                break;
        }
    }

}
