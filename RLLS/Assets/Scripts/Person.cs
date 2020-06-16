using UnityEngine;

public abstract class Person : MonoBehaviour
{
    /// <summary>
    /// Fields
    /// </summary>

    //the rigidbody that actually moves--the player, the opponent, etc. This is probably different from the sword, the body, etc.
    protected Rigidbody rb;


    //the transform for the swordfighter's hands; used to determine how far they can move
    protected Transform handTransform;
    protected const string HANDS_TRANSFORM = "Hands";


    //hand movement
    protected float moveSpeed = 10.0f;


    //hand rotation
    protected Quaternion deltaRotation;
    protected const float BASE_ROT_SPEED = 50.0f; //starting wrist rotation speed
    protected Vector3 baseRotation = new Vector3(0.0f, 0.0f, BASE_ROT_SPEED);
    protected Vector3 baseSwing = new Vector3(BASE_ROT_SPEED, 0.0f, 0.0f);
    protected Vector3 currentSwingVector = new Vector3(0.0f, 0.0f, 0.0f);
    protected enum SwordState { Swinging, Returning, Guard }
    protected SwordState currentState;



    public abstract void Setup();



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
}
