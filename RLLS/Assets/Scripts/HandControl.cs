using UnityEngine;

public class HandControl : MonoBehaviour
{

    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////


    //the rigidbody for the player
    protected Rigidbody rb;


    //the transform for the player's hands; used to determine how far they can move
    protected Transform handTransform;
    protected const string HANDS_TRANSFORM = "Hands";


    //hand movement
    protected Vector3 prevLoc = new Vector3(0.0f, 0.0f, 0.0f);
    protected Vector3 newLoc = new Vector3(0.0f, 0.0f, 0.0f);
    protected Vector3 delta;
    protected float moveSpeed = 10.0f;


    //hand rotation
    protected Quaternion deltaRotation;
    protected const float BASE_ROT_SPEED = 50.0f; //starting wrist rotation speed
    protected Vector3 baseRotation = new Vector3(0.0f, 0.0f, BASE_ROT_SPEED);
    protected Vector3 baseSwing = new Vector3(BASE_ROT_SPEED, 0.0f, 0.0f);
    protected Vector3 currentSwingVector = new Vector3(0.0f, 0.0f, 0.0f);
    protected enum SwordState { Swinging, Returning, Guard }
    protected SwordState currentState;

    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////


    //register for mouse events and get the rigidbody so as to be able to respond to them
    public virtual void Setup()
    {
        Services.Events.Register<BothMouseButtonsEvent>(DetermineState);
        Services.Events.Register<KeyDirectionEvent>(MoveHands);
        Services.Events.Register<MouseEvent>(RotateHands);
        rb = GetComponent<Rigidbody>();
        handTransform = transform.Find(HANDS_TRANSFORM);
        newLoc = Input.mousePosition; //avoid a "jump" on the first frame of movement
        currentState = SwordState.Guard;
    }


    //move hands based on player input
    protected virtual void MoveHands(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(KeyDirectionEvent), "Non-KeyDirectionEvent in MoveHands.");

        KeyDirectionEvent dirEvent = e as KeyDirectionEvent;

        //move hands
        InputManager.Directions dir = dirEvent.direction;

        switch (dir)
        {
            case InputManager.Directions.Up:
                rb.AddForce(Vector3.up * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Down:
                rb.AddForce(Vector3.down * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Left:
                rb.AddForce(Vector3.left * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Right:
                rb.AddForce(Vector3.right * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Diag_Up_Left:
                rb.AddForce(Vector3.up * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddForce(Vector3.left * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Diag_Up_Right:
                rb.AddForce(Vector3.up * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddForce(Vector3.right * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Diag_Down_Left:
                rb.AddForce(Vector3.down * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddForce(Vector3.left * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Diag_Down_Right:
                rb.AddForce(Vector3.down * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                rb.AddForce(Vector3.right * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
                break;
        }
    }


    protected virtual void RotateHands(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(MouseEvent), "Non-MouseEvent in MoveHands.");

        MouseEvent mouseEvent = e as MouseEvent;

        //swing or return if both buttons are pressed

        if (mouseEvent.LMB && mouseEvent.RMB)
        {
            rb.MoveRotation(SwingOrReturn());
        }

        //rotate hands if either LMB or RMB is pressed
        else if (mouseEvent.LMB || mouseEvent.RMB)
        {

            //set the amount of rotation for this frame
            if (mouseEvent.LMB) { deltaRotation = Quaternion.Euler(baseRotation * Time.deltaTime); } //rotate left for LMB
            else { deltaRotation = Quaternion.Euler(baseRotation * Time.deltaTime * -1.0f); } //rotate right for RMB

            //actually spin the hands
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
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
    /// Decide whether the player is swinging the sword, or returning to guard.
    /// 
    /// When a BothMouseButtonsEvent is detected, switch between those states.
    /// </summary>
    /// <param name="e">A BothMouseButtonsEvent.</param>
    protected virtual void DetermineState(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(BothMouseButtonsEvent), "Non-BothMouseButtonsEvent in DetermineState().");

        if (currentState == SwordState.Swinging) currentState = SwordState.Returning;
        else if (currentState == SwordState.Returning || currentState == SwordState.Guard) currentState = SwordState.Swinging;
    }
}



