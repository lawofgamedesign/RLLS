using System;
using System.Collections.Generic;
using UnityEngine;

public class OpponentStances
{
    /// <summary>
    /// Fields
    /// </summary>


    private Vector3 handWorldStartPos = new Vector3(0.0f, 0.0f, 1.0f);
    private const float ARM_LENGTH = 1.5f; //the linear limit of the configurable joint that constrains hand movement
    private const float DIAG_ARM_LENGTH = 1.06f; //the hands' distance from the origin on any one axis when held in a diagonal position

    public enum Stances { High, Left, Right, High_Left, High_Right, Extended }

    public Dictionary<Stances, HandPosition> stances = new Dictionary<Stances, HandPosition>();


    public void EstablishStances()
    {
        Vector3 high = handWorldStartPos + new Vector3(0.0f, ARM_LENGTH, 0.0f);
        Vector3 left = handWorldStartPos + new Vector3(ARM_LENGTH, 0.0f, 0.0f); //the opponent's left is in the positive X direction
        Vector3 right = handWorldStartPos + new Vector3(-ARM_LENGTH, 0.0f, 0.0f); //the opponent's right is in the negative X direction
        Vector3 highLeft = handWorldStartPos + new Vector3(DIAG_ARM_LENGTH, DIAG_ARM_LENGTH, 0.0f);
        Vector3 highRight = handWorldStartPos + new Vector3(-DIAG_ARM_LENGTH, DIAG_ARM_LENGTH, 0.0f);
        Vector3 extended = handWorldStartPos; //since hands are Z-locked, the extended position is just the starting position

        Quaternion highRot = Quaternion.identity;
        Quaternion leftRot = Quaternion.Euler(new Vector3(0.0f, 0.0f, -90.0f));
        Quaternion rightRot = Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f));
        Quaternion highLeftRot = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45.0f));
        Quaternion highRightRot = Quaternion.Euler(new Vector3(0.0f, 0.0f, 45.0f));
        Quaternion extendedRot = Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f));

        stances.Add(Stances.High, new HandPosition(high, highRot));
        stances.Add(Stances.Left, new HandPosition(left, leftRot));
        stances.Add(Stances.Right, new HandPosition(right, rightRot));
        stances.Add(Stances.High_Left, new HandPosition(highLeft, highLeftRot));
        stances.Add(Stances.High_Right, new HandPosition(highRight, highRightRot));
        stances.Add(Stances.Extended, new HandPosition(extended, extendedRot));
    }


    public static Stances GetRandomStance()
    {
        int randomStance = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Stances)).Length);
        return (Stances)randomStance;
    }
}
