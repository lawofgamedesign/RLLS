using UnityEngine;

public class MouseEvent : Event
{

    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////

    //mouse position
    private readonly Vector3 pos;
    public Vector3 Pos
    {
        get { return pos; }
    }


    //mouse buttons
    private readonly bool lmbPressed;
    public bool LMB
    {
        get { return lmbPressed; }
    }
    private readonly bool rmbPressed;
    public bool RMB
    {
        get { return rmbPressed; }
    }


    //mouse wheel status
    private readonly float wheelDelta;
    public float WheelDelta
    {
        get { return wheelDelta; }
    }


    //how much the mouse moved from last frame to this one
    protected readonly Vector3 delta;


    /////////////////////////////////////////////
    /// Functions
    /////////////////////////////////////////////

    
    //constructor
    public MouseEvent(Vector3 pos, Vector3 delta, Vector2 wheelDelta, bool lmbPressed, bool rmbPressed)
    {
        this.pos = pos;
        this.delta = delta;
        this.wheelDelta = wheelDelta.y;
        this.lmbPressed = lmbPressed;
        this.rmbPressed = rmbPressed;
    }
}
