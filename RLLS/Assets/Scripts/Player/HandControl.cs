using UnityEngine;

public class HandControl : Person
{

    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////


    //the player's sword
    protected const string PLAYER_SWORD = "Player 1 sword";


    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////


    //register for mouse events and get the rigidbody so as to be able to respond to them
    public override void Setup()
    {
        Services.Events.Register<BothMouseButtonsEvent>(SwordStateViaInput);
        Services.Events.Register<KeyDirectionEvent>(MoveHandsViaInput);
        Services.Events.Register<MouseEvent>(RotateHandsViaInput);
        rb = GetComponent<Rigidbody>();
        handTransform = transform.Find(HANDS_TRANSFORM);
        currentState = SwordState.Guard;
        transform.Find(PLAYER_SWORD).GetComponent<SwordBehavior>().Setup();
    }



    //move hands based on player input
    protected virtual void MoveHandsViaInput(global::Event e)
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



    protected virtual void RotateHandsViaInput(global::Event e)
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
    /// Decide whether the player is swinging the sword, or returning to guard.
    /// 
    /// When a BothMouseButtonsEvent is detected, switch between those states.
    /// </summary>
    /// <param name="e">A BothMouseButtonsEvent.</param>
    protected virtual void SwordStateViaInput(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(BothMouseButtonsEvent), "Non-BothMouseButtonsEvent in SwordStateViaInput().");

        if (currentState == SwordState.Swinging) currentState = SwordState.Returning;
        else if (currentState == SwordState.Returning || currentState == SwordState.Guard) currentState = SwordState.Swinging;
    }
}



