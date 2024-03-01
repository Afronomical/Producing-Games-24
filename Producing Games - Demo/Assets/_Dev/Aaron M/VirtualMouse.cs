using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class VirtualMouse : MonoBehaviour
{
    public UnityEvent clickEvent;
    public UnityEvent releaseClickEvent;
    public List<GameObject> collidingObjects = new List<GameObject>();
    private bool click, releaseClick;
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        cam = Camera.main;
    }


    void Update()
    {
        ray = cam.ScreenPointToRay( Input.mousePosition );
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, hit.point.y, hit.point.z), Time.deltaTime * 100);
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (!collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Add(collision.gameObject);
        }
    }


    private void OnTriggerExit(Collider collision)
    {
        if (collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Remove(collision.gameObject);
        }
    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //currentInput = context.ReadValue<Vector2>();
    }


    public void OnSelectInput(InputAction.CallbackContext context)
    {
        //if (context.performed)

    }
}
