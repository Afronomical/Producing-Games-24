using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class HRM : MonoBehaviour
{
    //public static HRM instance;
    public TMP_Text hRateText;
    private int bPM = 70;
    public GameObject staticEffect;
    //public AnimationCurve heartRate;
    private float timer = 0;
    public float timerInterval = 24;

    // Start is called before the first frame update
    void Start()
    {
        //if(instance == null)
        //    instance = this;

        //heartRate = GetComponent<AnimationCurve>();

        //Initialise HR Graph
        //heartRate.AddKey(0, 0);
        //heartRate.AddKey(0.1f, 0);
        //heartRate.AddKey(0.2f, 0.5f);
        //heartRate.AddKey(0.4f, -0.5f);
        //heartRate.AddKey(0.6f, 0.5f);
        //heartRate.AddKey(0.8f, 0);
        //heartRate.AddKey(1, 0);

        //Display HR Graph
    }

    // Update is called once per frame
    void Update()
    {
        //Update bPM to mimic heart beats
        timer++;
        
        if(timer >= timerInterval)
        {
            timer = 0;
            if (UnityEngine.Random.Range(60, 100) > bPM)
                bPM += 1;
            else bPM -= 1;
            hRateText.text = "HR  " + bPM + " (bpm)";
        }

        //Switch from normal vitals to heart attack vitals


    }

    //Function for heart attack task
    public void FlatLine()
    {
        //heartRate.ClearKeys();

        ////Set up
        //heartRate.AddKey(0, 0);
        //heartRate.AddKey(1, 0);

        //Draw

    }

    public void TurnOn()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        staticEffect.SetActive(false);
    }

    public void TurnOff()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
        gameObject.GetComponent<ParticleSystem>().Clear();
        staticEffect.SetActive(true);
    }
}
