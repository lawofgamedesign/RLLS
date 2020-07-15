using UnityEngine;

public class WheelControl : HandControl
{
    /// <summary>
    /// This version of HandControl uses the mousewheel to swing the sword, rather than alternating between both mouse buttons
    /// </summary>


    //scales mouse wheel delta to make swinging more responsive
    protected const float SWING_MULTIPLIER = 25.0f;
    

    public override void Setup()
    {
        base.Setup();

        //don't use the both mouse buttons method of control
        Services.Events.Unregister<BothMouseButtonsEvent>(SwordStateViaInput);
        Services.Events.Unregister<BothMouseButtonsUpEvent>(BladeDirectionFeedback);
    }

    protected override void RotateHandsViaInput(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(MouseEvent), "Non-MouseEvent in MoveHands.");

        MouseEvent mouseEvent = e as MouseEvent;


        if (mouseEvent.WheelDelta != 0.0f)
        {
            deltaRotation = Quaternion.Euler(baseSwing * mouseEvent.WheelDelta * SWING_MULTIPLIER * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        

        //rotate hands if either LMB or RMB is pressed
        if (mouseEvent.LMB || mouseEvent.RMB)
        {

            //set the amount of rotation for this frame
            if (mouseEvent.LMB) { deltaRotation = Quaternion.Euler(baseRotation * Services.Speed.CurrentMultiplier * Time.deltaTime); } //rotate left for LMB
            else { deltaRotation = Quaternion.Euler(baseRotation * Services.Speed.CurrentMultiplier * Time.deltaTime * -1.0f); } //rotate right for RMB

            //actually spin the hands
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}
