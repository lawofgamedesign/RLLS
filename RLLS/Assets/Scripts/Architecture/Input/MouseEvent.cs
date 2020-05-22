using UnityEngine;

public class MouseEvent : Event
{
    protected readonly Vector3 pos;
    protected readonly Vector3 delta;

    public MouseEvent(Vector3 pos, Vector3 delta)
    {
        this.pos = pos;
        this.delta = delta;
    }
}
