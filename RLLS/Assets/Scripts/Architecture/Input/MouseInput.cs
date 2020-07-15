using UnityEngine;

public class MouseInput : InputManager
{

    /// <summary>
    /// This InputManager variant relies entirely on the mouse's movement for sword control.
    /// </summary>
    public override void Tick()
    {
        GetDirection();
        GetMouseState();
    }
}
