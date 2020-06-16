using UnityEngine;

public class HandPosition
{
    public readonly Vector3 worldPosition;
    public readonly Quaternion handRotation;


    public HandPosition(Vector3 worldPosition, Quaternion handRotation)
    {
        this.worldPosition = worldPosition;
        this.handRotation = handRotation;
    }
}
