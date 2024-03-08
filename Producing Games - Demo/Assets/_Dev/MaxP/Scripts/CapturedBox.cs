using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Script for the demon capture event
/// meant to take places when demon captures you, this "cutscene" happens, then the player gets sent to the next shift
/// </summary>

public class CapturedBox : MonoBehaviour
{
    
    private GameObject playerObj;
    private CharacterController playerCont;
    private PlayerInput playerInp;
    public float beforeTpDelay;
    public GameObject tpLoc;
    public GameObject demon1;
    public GameObject demon2;
    public GameObject eyes;
    public SoundEffect mainAmbienceSound;
    public SoundEffect jumpScareSound;
    private CameraLook camScript;
    private Flashlight lightScript;

    private Vector3 origPosDemon1;
    private Vector3 origPosDemon2;

    [Header("Demon Move to Locs")]
    public GameObject moveto1;
    public GameObject moveto2;

    [Header("Demon Move Timings")]
    public float waitForStage1 = 3;
    public float stage1 = 3;
    public float waitForStage2 = 3;
    public float stage2 = 3;
    public float waitForStage3 = 3;
    public float stage3 = 0.2f;
    

    void Start()
    {
        playerObj = GameManager.Instance.player;
        camScript = FindAnyObjectByType<CameraLook>(); //there may be a better way to do this?
        lightScript = FindAnyObjectByType<Flashlight>();

        playerCont = playerObj.GetComponent<CharacterController>();
        playerInp = playerObj.GetComponent<PlayerInput>();


        origPosDemon1 = demon1.transform.position;
        origPosDemon2 = demon2.transform.position;
    }


    void Update()
    {

        //if (Input.GetKey(KeyCode.M))
        //{
        //    StartCoroutine(MainEvent());
        //}


    }

    public IEnumerator MainEvent()
    {
        yield return new WaitForSeconds(beforeTpDelay); //delay before teleported, so demon attack animation has a chance to play... could put this after turning off input...

        playerCont.enabled = false;
        playerInp.enabled = false;

        lightScript.intensityIndex = 0;
        lightScript.IntensityChange();

        playerObj.transform.position = tpLoc.transform.position;
        LookAtTarg(demon1.transform.position);

        yield return new WaitForSeconds(waitForStage1); //stage 1: demon moving into light
        AudioManager.instance.PlaySound(mainAmbienceSound, moveto1.transform);

        float elapsedTime = 0;
        float waitTime = stage1;

        while (elapsedTime < waitTime)
        {
            demon1.transform.position = Vector3.Lerp(origPosDemon1, moveto1.transform.position, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime / waitTime)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        AudioManager.instance.PlaySound(mainAmbienceSound, moveto1.transform);

        yield return new WaitForSeconds(waitForStage2); //stage 2: demon moving back out of light

        elapsedTime = 0;
        waitTime = stage2;

        while (elapsedTime < waitTime)
        {
            demon1.transform.position = Vector3.Lerp(moveto1.transform.position, origPosDemon1, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime / waitTime)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        eyes.SetActive(false);

        yield return new WaitForSeconds(waitForStage3); //stage 3: jumpscare demon

        AudioManager.instance.PlaySound(jumpScareSound, playerObj.transform);

        elapsedTime = 0;
        waitTime = stage3;

        while (elapsedTime < waitTime)
        {
            demon2.transform.position = Vector3.Lerp(origPosDemon2, moveto2.transform.position, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime / waitTime)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        

        yield return new WaitForSeconds(1f);

        eyes.SetActive(true);

        demon1.transform.position = origPosDemon1;
        demon2.transform.position = origPosDemon2;

        playerInp.enabled = true;
        playerCont.enabled = true;

        StartCoroutine(GameManager.Instance.EndHour());
        //GameManager.Instance.EndHour();
    }

    void LookAtTarg(Vector3 targCoords) //makes player face specific coords with cam up and down rotation being centre
    {
        Vector3 direction = targCoords - playerObj.transform.position;
        direction.y = 0;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        playerObj.transform.rotation = toRotation;
        camScript.xRot = 0;
    }
}
