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

    private Vector3 LineCD;
    private Vector3 LineAB;
    private Vector3 LineBC;
    private Vector3 LineAB_BC;
    private Vector3 LineBC_CD;
    private Vector3 bezierPoint;
    [HideInInspector] public List<Vector3> pointList = new List<Vector3>();

    public void InitializeWire()
    {
        box = transform.parent.GetComponent<SatelliteBox>();
        if (lineRenderer) lineRenderer.enabled = false;
        connected = false;
    }


    void Update()
    {
        if (lineRenderer && box)
        {
            if (followingMouse)
            {
                pointD.position = box.cursor.transform.position;
                float yDistAD = pointD.position.y - pointA.position.y;
                float zDistAD = pointD.position.z - pointA.position.z;
                pointC.position = new Vector3(pointD.position.x, pointD.position.y + (yDistAD / 7.5f), pointA.position.z + (zDistAD / 1.75f));

                SetLineCurve();
            }
        }
    }


    private void SetLineCurve()
    {
        pointList.Clear();
        for (float ratio = 0; ratio <= 1.0f; ratio += 1.0f / pointCount)  // Calculate bezier curve
        {
            LineAB = Vector3.Lerp(pointA.position, pointB.position, ratio);
            LineBC = Vector3.Lerp(pointB.position, pointC.position, ratio);
            LineCD = Vector3.Lerp(pointC.position, pointD.position, ratio);

            LineAB_BC = Vector3.Lerp(LineAB, LineBC, ratio);
            LineBC_CD = Vector3.Lerp(LineBC, LineCD, ratio);

            bezierPoint = Vector3.Lerp(LineAB_BC, LineBC_CD, ratio);
            pointList.Add(bezierPoint);
        }
        lineRenderer.positionCount = pointList.Count;  // Add the points along the curve to the line renderer
        lineRenderer.SetPositions(pointList.ToArray());
    }


    public void Hold()
    {
        followingMouse = true;
        lineRenderer.enabled = true;
    }


    public void Release()
    {
        followingMouse = false;
        lineRenderer.enabled = false;
    }


    public void Connect()
    {
        connected = true;
        followingMouse = false;
        lineRenderer.enabled = true;

        pointD.position = wireEnd.transform.GetChild(0).position;
        float yDistAD = pointD.position.y - pointA.position.y;
        float zDistAD = pointD.position.z - pointA.position.z;
        pointC.position = new Vector3(pointD.position.x, pointD.position.y + (yDistAD / 7.5f), pointA.position.z + (zDistAD / 1.75f));

        SetLineCurve();
    }
}