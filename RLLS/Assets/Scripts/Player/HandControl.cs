using UnityEngine;

public class HandControl : Person
{

    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////



    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////


    //register for mouse events and get the rigidbody so as to be able to respond to them
    public override void Setup()
    {
        base.Setup();
        Services.Events.Register<BothMouseButtonsEvent>(SwordStateViaInput);
        Services.Events.Register<KeyDirectionEvent>(MoveHandsViaInput);
        Services.Events.Register<MouseEvent>(RotateHandsViaInput);
        handTransform = transform.Find(HANDS_TRANSFORM);
        currentState = SwordState.Guard;
        Services.Events.Register<BothMouseButtonsUpEvent>(BladeDirectionFeedback);
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
                rb.AddForce(ApplyMovement(Vector3.up), ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Down:
                rb.AddForce(ApplyMovement(Vector3.down), ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Left:
                rb.AddForce(ApplyMovement(Vector3.left), ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Right:
                rb.AddForce(ApplyMovement(Vector3.right), ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Diag_Up_Left:
                rb.AddForce(ApplyMovement(Vector3.up), ForceMode.VelocityChange);
                rb.AddForce(ApplyMovement(Vector3.left), ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Diag_Up_Right:
                rb.AddForce(ApplyMovement(Vector3.up), ForceMode.VelocityChange);
                rb.AddForce(ApplyMovement(Vector3.right), ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Diag_Down_Left:
                rb.AddForce(ApplyMovement(Vector3.down), ForceMode.VelocityChange);
                rb.AddForce(ApplyMovement(Vector3.left), ForceMode.VelocityChange);
                break;
            case InputManager.Directions.Diag_Down_Right:
                rb.AddForce(ApplyMovement(Vector3.down), ForceMode.VelocityChange);
                rb.AddForce(ApplyMovement(Vector3.right), ForceMode.VelocityChange);
                break;
        }
    }


    protected Vector3 ApplyMovement(Vector3 dir)
    {
        return dir * moveSpeed * Time.deltaTime * Services.Speed.CurrentMultiplier;
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
            if (mouseEvent.LMB) { deltaRotation = Quaternion.Euler(baseRotation * Services.Speed.CurrentMultiplier * Time.deltaTime); } //rotate left for LMB
            else { deltaRotation = Quaternion.Euler(baseRotation * Services.Speed.CurrentMultiplier * Time.deltaTime * -1.0f); } //rotate right for RMB

            //actually spin the hands
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }



    /// <summary>
    /// Decide whether the player is swinging the sword, or returning to guard.
    /// 
    /// When a BothMouseButtonsEvent is detected, switch between those states. In addition, send out an event so that the opponent can decide whether and how to respond.
    /// </summary>
    /// <param name="e">A BothMouseButtonsEvent.</param>
    protected virtual void SwordStateViaInput(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(BothMouseButtonsEvent), "Non-BothMouseButtonsEvent in SwordStateViaInput().");

        if (currentState == SwordState.Swinging || currentState == SwordState.Guard)
        {
            currentState = SwordState.Returning;
            Services.Events.Fire(new WithdrawingEvent());
        }
        else if (currentState == SwordState.Returning)
        {
            currentState = SwordState.Swinging;
            Services.Events.Fire(new StrikingEvent());
        }
    }


    #region sword edge



    protected void BladeDirectionFeedback(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(BothMouseButtonsUpEvent), "Non-BothMouseButtonsUpEvent in BladeDirectionFeedback");

        if (currentState == SwordState.Swinging || currentState == SwordState.Guard) Services.Swords.ReversePlayerEdge(SwordManager.BladePositions.Backward);
        else Services.Swords.ReversePlayerEdge(SwordManager.BladePositions.Forward);
    }


    #endregion
}



