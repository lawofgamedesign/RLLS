using System;
using System.Collections.Generic;
using UnityEngine;

public class OpponentStances
{
    /// <summary>
    /// Fields
    /// </summary>

    
    //used to position hands across stances
    private Vector3 handWorldStartPos = new Vector3(0.0f, 0.75f, 1.0f);
    private const float ARM_LENGTH = 1.5f; //the linear limit of the configurable joint that constrains hand movement
    private const float DIAG_ARM_LENGTH = 1.06f; //the hands' distance from the origin on any one axis when held in a diagonal position



    /// <summary>
    /// Stances used for attacking
    /// </summary>
    public enum Stances { High, Left, Right, High_Left, High_Right, Neutral, Extended }  //IMPORTANT: Neutral and Extended *must* be the last stances; see GetRandomStance(), below

    public Dictionary<Stances, HandPosition> stances = new Dictionary<Stances, HandPosition>();



    public enum Parries { High, Left, Right }
    public Dictionary<Parries, HandPosition> parries = new Dictionary<Parries, HandPosition>();



    /// <summary>
    /// Functions
    /// </summary>



    public void EstablishStances()
    {
        Vector3 high = handWorldStartPos + new Vector3(0.0f, ARM_LENGTH, 0.0f);
        Vector3 left = handWorldStartPos + new Vector3(ARM_LENGTH, 0.0f, 0.0f); //the opponent's left is in the positive X direction
        Vector3 right = handWorldStartPos + new Vector3(-ARM_LENGTH, 0.0f, 0.0f); //the opponent's right is in the negative X direction
        Vector3 highLeft = handWorldStartPos + new Vector3(DIAG_ARM_LENGTH, DIAG_ARM_LENGTH, 0.0f);
        Vector3 highRight = handWorldStartPos + new Vector3(-DIAG_ARM_LENGTH, DIAG_ARM_LENGTH, 0.0f);
        Vector3 neutral = handWorldStartPos;
        Vector3 extended = handWorldStartPos; //since hands are Z-locked, the extended position is just the starting position

        Quaternion highRot = Quaternion.identity;
        Quaternion leftRot = Quaternion.Euler(new Vector3(0.0f, 0.0f, -90.0f));
        Quaternion rightRot = Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f));
        Quaternion highLeftRot = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45.0f));
        Quaternion highRightRot = Quaternion.Euler(new Vector3(0.0f, 0.0f, 45.0f));
        Quaternion neutralRot = Quaternion.identity;
        Quaternion extendedRot = Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f));

        stances.Add(Stances.High, new HandPosition(high, highRot));
        stances.Add(Stances.Left, new HandPosition(left, leftRot));
        stances.Add(Stances.Right, new HandPosition(right, rightRot));
        stances.Add(Stances.High_Left, new HandPosition(highLeft, highLeftRot));
        stances.Add(Stances.High_Right, new HandPosition(highRight, highRightRot));
        stances.Add(Stances.Neutral, new HandPosition(neutral, neutralRot));
        stances.Add(Stances.Extended, new HandPosition(extended, extendedRot));
    }


    public void EstablishParries()
    {
        Vector3 high = handWorldStartPos + new Vector3(0.6f, 1.0f, 0.0f);
        Vector3 left = handWorldStartPos + new Vector3(0.75f, -0.5f, 0.0f);
        Vector3 right = handWorldStartPos + new Vector3(-0.75f, -0.5f, 0.0f);

        Quaternion highRot = Quaternion.Euler(new Vector3(0.0f, 0.0f, 75.0f));
        Quaternion leftRot = Quaternion.identity;
        Quaternion rightRot = Quaternion.identity;

        parries.Add(Parries.High, new HandPosition(high, highRot));
        parries.Add(Parries.Left, new HandPosition(left, leftRot));
        parries.Add(Parries.Right, new HandPosition(right, rightRot));
    }


    /// <summary>
    /// Returns a random attack stance, so that the opponent can behave unpredictably.
    /// 
    /// IMPORTANT: Enum.GetValues . . . .Length *-2* is used to exclude Neutral and Extended. Otherwise the opponent will try to adopt stances which don't lead to an attack, which is not the
    /// intended use of this function. To get those stances, call them specifically.
    /// </summary>
    /// <returns>A randomly chosen stance.</returns>
    public static Stances GetRandomStance()
    {
        int randomStance = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Stances)).Length - 2);
        return (Stances)randomStance;
    }
}
