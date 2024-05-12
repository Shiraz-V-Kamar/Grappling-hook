using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    public static InputsManager Instance { get; private set; }

    [Header("Character Input Values")]
    public Vector2 Move;
    public bool Jump;
    public bool Sprint;
    public bool Grapple;

    public bool cursorLocked = true;

    [Header("Other")]
    public bool Restart;
    public bool Quit;

    private void Awake()
    {
        Instance = this;
    }

    public void OnMove(InputValue value)
    {
        Move = value.Get<Vector2>();   
    }
    public void OnJump(InputValue value)
    {
        Jump = value.isPressed;

    }

    public void OnSprint(InputValue value)
    {
        Sprint = value.isPressed;
    }

    public void OnGrapple(InputValue value)
    {
        Grapple = value.isPressed;
    }

    public void OnRestart(InputValue value)
    {
        Restart = value.isPressed;
    }

    public void OnQuit(InputValue value)
    {
        Quit = value.isPressed;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
