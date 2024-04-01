using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class ComputerCursor : InspectableObject
{
    public SoundEffect clickSound;
    public TMP_Text clock;
    public SoundEffect VoiceLines;

    [Header("Fake Cursor")]
    public Texture2D cursor;
    public Texture2D cursorClicked;
    [SerializeField] private PlayerInput playerInput;
    private Mouse virtualMouse;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private float cursorSpeed = 1000;
    private bool previousMouseState;
    [SerializeField] private RectTransform canvasRectTransform;

    private void VirtualMouse()
    {
        //If using gamepad
        if (virtualMouse == null) virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        else if (!virtualMouse.added) InputSystem.AddDevice(virtualMouse);

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateCursor;
    }

    public override void Interact()
    {
        base.Interact();
        AudioManager.instance.PlaySound(VoiceLines, gameObject.transform);

        VirtualMouse();

        ChangeCursor(cursor);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        actionTooltip.enabled = true;
        actionTooltip.text = "Press C to leave your PC!";
    }

    protected override void Update()
    {
        base.Update();

        if (stopLooking)
        {
            actionTooltip.enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            InputSystem.onAfterUpdate -= UpdateCursor;
        }

        DisplayTime();
    }

    private void ChangeCursor(Texture2D cursorType)
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }

    private void UpdateCursor()
    {
        if (virtualMouse == null || Gamepad.current == null) return;

        Vector2 deltaValue = Gamepad.current.rightStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPos = virtualMouse.position.ReadValue();
        Vector2 newPos = currentPos + deltaValue;

        newPos.x = Mathf.Clamp(newPos.x, 0f, Screen.width);
        newPos.y = Mathf.Clamp(newPos.y, 0f, Screen.height);

        InputState.Change(virtualMouse.position, newPos);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool aButtonPressed = Gamepad.current.aButton.IsPressed();
        if (previousMouseState != aButtonPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonPressed;
        }

        AnchorCursor(newPos);
    }

    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, null, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }

    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (context.performed && looking)
            AudioManager.instance.PlaySound(clickSound, null);
    }

    private void DisplayTime()
    {
        string time = "0";
        time += GameManager.Instance.currentHour.ToString();

        time += ":";
        if (GameManager.Instance.currentTime < 10)
        {
            time += "0";
        }
        time += (int)GameManager.Instance.currentTime;
        time += " AM";

        clock.text = time.ToUpper();
    }
}
