using UnityEngine;

public class InputManager
{
    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////

    private Vector3 lastFramePos = new Vector3(0.0f, 0.0f);
    private Vector3 thisFramePos = new Vector3(0.0f, 0.0f);



    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////



    public virtual void Tick()
    {
        lastFramePos = thisFramePos;

        thisFramePos = Input.mousePosition;

        Services.Events.Fire(new MouseEvent(thisFramePos, thisFramePos - lastFramePos));
    }
}
