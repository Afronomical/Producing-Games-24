using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteWire : MonoBehaviour
{
    private SatelliteBox box;
    public bool followingMouse;
    public bool connected;
    public SatelliteBox.Colours colour;
    public GameObject line;
    public GameObject wireEnd;

    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;
    public LineRenderer lineRenderer;
    public int pointCount = 12;

    void Start()
    {
        box = transform.parent.GetComponent<SatelliteBox>();
        pointB.position = new Vector3(pointB.position.x, wireEnd.transform.GetChild(0).position.y, pointB.position.z);
        pointD.position = new Vector3(wireEnd.transform.GetChild(0).position.x, pointD.position.y, pointD.position.z);
    }

    void Update()
    {
        pointD.position = box.virtualMouse.transform.position;

        var pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1 / pointCount)
        {
            Vector3 LineAB = Vector3.Lerp(pointA.position, pointB.position, ratio);
            Vector3 LineBC = Vector3.Lerp(pointB.position, pointC.position, ratio);
            Vector3 LineCD = Vector3.Lerp(pointC.position, pointD.position, ratio);

            Vector3 LineAB_BC = Vector3.Lerp(LineAB, LineBC, ratio);
            Vector3 LineBC_CD = Vector3.Lerp(LineBC, LineCD, ratio);

            Vector3 bezierPoint = Vector3.Lerp(LineAB_BC, LineBC_CD, ratio);
            pointList.Add(bezierPoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    public void Hold()
    {
        followingMouse = true;
        line.SetActive(true);
    }


    public void Release()
    {
        followingMouse = false;
    }


    public void Connect()
    {
        connected = true;
        followingMouse = false;
    }
}