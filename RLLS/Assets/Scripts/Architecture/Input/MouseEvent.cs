using UnityEngine;

public class MouseEvent : Event
{
    private readonly Vector3 pos;
    public Vector3 Pos
    {
        get { return pos; }
    }


    protected readonly Vector3 delta;


    public MouseEvent(Vector3 pos, Vector3 delta)
    {
        this.pos = pos;
        this.delta = delta;
    }
}
