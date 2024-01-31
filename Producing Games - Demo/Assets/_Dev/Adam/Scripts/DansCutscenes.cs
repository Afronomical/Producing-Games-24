using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DansCutscenes : MonoBehaviour
{
    public Camera cam;
    public List<Transform> points;
    private int index = 0;

    bool GoIn;
    bool GoOut;

    private void Start()
    {
        //cam = Camera.main;
    }

    private void Update()
    {
        //if (points == null || points.Count < 1)
        //    return;

        if (Input.GetKeyDown(KeyCode.K))
            GoIn = true;

        if (GoIn)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, points[index].position, 2.5f * Time.deltaTime);

            if(cam.transform.rotation != points[index].rotation)
            {
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, points[index].rotation, 3f * Time.deltaTime);
            }
            if (cam.transform.position == points[index].position && cam.transform.rotation == points[index].rotation)
            {
                index++;
                if (index >= points.Count)
                {
                    Debug.Log("Done going in");
                    index = 0;
                    GoIn = false;
                    GoOut = true;
                }

            }
        }

        if (GoOut)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, points[index].position, 2.5f * Time.deltaTime);

            if (cam.transform.rotation != points[index].rotation)
            {
                cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, points[index].rotation, 3f * Time.deltaTime);
            }
            if (cam.transform.position == points[index].position && cam.transform.rotation == points[index].rotation)
            {
                index++;
                if (index >= points.Count)
                {
                    Debug.Log("Done going in");
                    index = 0;
                    GoOut = false;
                }

            }
        }
            
            

    }
}
