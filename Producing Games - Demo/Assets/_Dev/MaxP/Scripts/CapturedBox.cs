using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

public class CapturedBox : MonoBehaviour
{
    
    GameObject playerObj;
    public GameObject tpLoc;
    public GameObject demon1;
    public GameObject demon2;
    public GameObject camObj;
    public SoundEffect jumpScareSound;
    private CameraLook camScript;
    private Flashlight lightScript;

    private Vector3 origPosDemon1;
    private Vector3 origPosDemon2;

    [Header("Demon Move to Locs")]
    public GameObject moveto1;
    public GameObject moveto2;

    [Header("Demon Move Timings")]
    public float stage1 = 3;
    public float stage2 = 3;
    public float stage3 = 0.2f;
    

    void Start()
    {
        playerObj = GameManager.Instance.player;
        camScript = FindAnyObjectByType<CameraLook>();
        lightScript = FindAnyObjectByType<Flashlight>();
        //camScript = gameObject.GetComponent<CameraLook>();

        origPosDemon1 = demon1.transform.position;
        origPosDemon2 = demon2.transform.position;
    }


    void Update()
    {



        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(MainEvent());

        }
    }

    IEnumerator MainEvent()
    {
        
        playerObj.GetComponent<CharacterController>().enabled = false;
        playerObj.GetComponent<PlayerInput>().enabled = false;
        lightScript.intensityIndex = 0;
        lightScript.IntensityChange();
        playerObj.transform.position = tpLoc.transform.position;
        LookAtTarg(demon1.transform.position);
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0;
        float waitTime = stage1;
        while (elapsedTime < waitTime)
        {
            demon1.transform.position = Vector3.Lerp(origPosDemon1, moveto1.transform.position, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime / waitTime)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(2f);
        elapsedTime = 0;
        waitTime = stage2;
        while (elapsedTime < waitTime)
        {
            demon1.transform.position = Vector3.Lerp(moveto1.transform.position, origPosDemon1, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime / waitTime)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(2f);
        AudioManager.instance.PlaySound(jumpScareSound, playerObj.transform);
        elapsedTime = 0;
        waitTime = stage3;
        while (elapsedTime < waitTime)
        {
            demon2.transform.position = Vector3.Lerp(origPosDemon2, moveto2.transform.position, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime / waitTime)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        

        yield return new WaitForSeconds(0.5f);
        demon1.transform.position = origPosDemon1;
        demon2.transform.position = origPosDemon2;
        playerObj.GetComponent<PlayerInput>().enabled = true;
        playerObj.GetComponent<CharacterController>().enabled = true;
        //GameManager.Instance.EndHour();
    }

    void LookAtTarg(Vector3 targCoords)
    {
        Vector3 direction = targCoords - playerObj.transform.position;
        direction.y = 0;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        playerObj.transform.rotation = toRotation;
        camScript.xRot = 0;
    }
}
