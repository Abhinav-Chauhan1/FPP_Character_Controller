using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    void Start()
    {
        //Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    //Sprint
    public bool GetSprintInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        return false;
    }
    //Crouch
    public bool GetCrouchInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.C);
        }

        return false;
    }
    //Crouch Over
    public bool GetCrouchInputReleased()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyUp(KeyCode.C);
        }

        return false;
    }

    //Jump
    public bool GetJumpInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        return false;
    }
}
