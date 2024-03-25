using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class MinigameCursor : MonoBehaviour
{
    public UnityEvent clickEvent;
    public UnityEvent releaseClickEvent;
    public List<GameObject> collidingObjects = new List<GameObject>();
    [Range(0, 2)] public float sensitivity;
    public Vector2 cursorMovementArea;
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 targetPos;
    private PlayerInput input;
    private MeshRenderer rend;

    void Start()
    {
        cam = Camera.main;
        input = GameManager.Instance.player.GetComponent<PlayerInput>();
        rend = GetComponent<MeshRenderer>();
    }


    void Update()
    {
        if (input.currentControlScheme != "Gamepad")
        {
            rend.enabled = false;
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                targetPos = new Vector3(transform.position.x, hit.point.y, hit.point.z);
            }
        }
        else
            rend.enabled = true;

        // Clamp the movement area
        if (targetPos.z < transform.parent.position.z - cursorMovementArea.x) targetPos.z = transform.parent.position.z - cursorMovementArea.x;
        else if (targetPos.z > transform.parent.position.z + cursorMovementArea.x) targetPos.z = transform.parent.position.z + cursorMovementArea.x;

        if (targetPos.y < transform.parent.position.y - cursorMovementArea.y) targetPos.y = transform.parent.position.y - cursorMovementArea.y;
        else if (targetPos.y > transform.parent.position.y + cursorMovementArea.y) targetPos.y = transform.parent.position.y + cursorMovementArea.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 100);
    }


    void OnTriggerEnter(Collider collision)
    {
        if (!collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Add(collision.gameObject);
        }
    }


    void OnTriggerExit(Collider collision)
    {
        if (collidingObjects.Contains(collision.gameObject))
        {
            collidingObjects.Remove(collision.gameObject);
        }
    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 currentInput = context.ReadValue<Vector2>();
        currentInput *= sensitivity / 10;
        targetPos = new Vector3(transform.position.x, transform.position.y + currentInput.y, transform.position.z - currentInput.x);
    }


    public void OnSelectInput(InputAction.CallbackContext context)
    {
        if (gameObject.activeSelf)
        {
            if (context.performed)
                clickEvent.Invoke();
            else if (context.canceled)
                releaseClickEvent.Invoke();
        }
    }
}
