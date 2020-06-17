using UnityEngine;

public abstract class Person : MonoBehaviour
{
    /// <summary>
    /// Fields
    /// </summary>

    //the rigidbody that actually moves--the player, the opponent, etc. This is probably different from the sword, the body, etc.
    protected Rigidbody rb;
    public Rigidbody Rb { get; private set; }


    //the transform for the swordfighter's hands; used to determine how far they can move
    protected Transform handTransform;
    protected const string HANDS_TRANSFORM = "Hands";


    //hand movement
    protected float moveSpeed = 10.0f;
    public float MoveSpeed { get; private set; }


    //hand rotation
    protected Quaternion deltaRotation;
    protected const float BASE_ROT_SPEED = 50.0f; //starting wrist rotation speed
    protected Vector3 baseRotation = new Vector3(0.0f, 0.0f, BASE_ROT_SPEED);
    protected Vector3 baseSwing = new Vector3(BASE_ROT_SPEED, 0.0f, 0.0f);
    protected Vector3 currentSwingVector = new Vector3(0.0f, 0.0f, 0.0f);
    protected enum SwordState { Swinging, Returning, Guard }
    protected SwordState currentState;


    //used to let opponents plan their moves
    protected OpponentStances.Stances lastOpponentStance;
    protected OpponentStances.Stances intendedOpponentStance;
    protected OpponentStances.Stances nextStance;


    //the opponents's sword
    protected const string OPPONENT_SWORD = "Player 2 sword";



    public virtual void Setup()
    {
        rb = GetComponent<Rigidbody>();
        Rb = rb;
        MoveSpeed = moveSpeed;
    }



    /// <summary>
    /// Find the new rotation of the hands as they swing or return.
    /// </summary>
    /// <returns>The new rotation, as a quaternion</returns>
    protected virtual Quaternion SwingOrReturn()
    {
        if (currentState == SwordState.Swinging) currentSwingVector = baseSwing;
        else if (currentState == SwordState.Returning) currentSwingVector = -1 * baseSwing;
        else Debug.Log("Incorrect SwordState in SwingOrReturn() " + currentState.ToString());

        deltaRotation = Quaternion.Euler(currentSwingVector * Time.deltaTime);
        return rb.rotation * deltaRotation;
    }


    /// <summary>
    /// Rotates the hands to an externally-determined angle.
    /// 
    /// Tasks that swing the opponent's sword use this rather than SwingOrReturn(), which accommodates the player's input.
    /// </summary>
    /// <param name="vector">The intended direction of the rotation.</param>
    /// <returns>The rotation actually achieved within this unit of time.</returns>
    public Quaternion SwingOnCommand(Vector3 vector)
    {
        deltaRotation = Quaternion.Euler(vector * Time.deltaTime);
        return rb.rotation * deltaRotation;
    }
}
