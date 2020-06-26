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


    //the swordfighter's body, head, etc.
    protected Rigidbody bodyRb;
    protected Rigidbody headRb;
    protected const string BODY_OBJ = " body";
    protected const string HEAD_OBJ = " head";
    protected float vulnerableSpeed = 3.0f;
    protected const string STRIKE_MSG = "Strike\nnow!";


    //hand movement
    protected float moveSpeed = 10.0f;
    public float MoveSpeed { get; private set; }


    //hand rotation
    protected Quaternion deltaRotation;
    protected const float BASE_ROT_SPEED = 50.0f; //starting wrist rotation speed
    public float RotSpeed { get; private set; }
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



    /// <summary>
    /// Functions
    /// </summary>


    public virtual void Setup()
    {
        rb = GetComponent<Rigidbody>();
        Rb = rb;
        bodyRb = GameObject.Find(gameObject.name + BODY_OBJ).GetComponent<Rigidbody>();
        headRb = GameObject.Find(gameObject.name + HEAD_OBJ).GetComponent<Rigidbody>();
        Services.Events.Register<NewSpeedEvent>(BecomeVulnerable);
        MoveSpeed = moveSpeed;
        RotSpeed = BASE_ROT_SPEED;
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

        deltaRotation = Quaternion.Euler(currentSwingVector * Services.Speed.OverallMultiplier * Time.deltaTime);
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
        return Quaternion.identity;
    }



    protected void BecomeVulnerable(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(NewSpeedEvent), "Non-NewSpeedEvent in BecomeVulnerable");

        NewSpeedEvent speedEvent = e as NewSpeedEvent;

        if (speedEvent.newSpeed >= vulnerableSpeed)
        {
            bodyRb.isKinematic = false;
            headRb.isKinematic = false;
            Services.UI.ChangeExplanatoryMessage(STRIKE_MSG);
        }
    }
}
