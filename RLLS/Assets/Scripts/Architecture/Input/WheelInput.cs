using UnityEngine;

public class WheelInput : InputManager
{


    /// <summary>
    /// WheelInput relies on the mouse wheel instead of both buttons for sword movement
    /// </summary>
    public override void Tick()
    {
        GetDirection();
        GetMouseState();
    }
}
