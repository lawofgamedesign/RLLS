using UnityEngine;

public class MouseControl : HandControl
{
    /// <summary>
    /// This version of HandControl controls the wrists entirely with mouse movement, rather than mouse buttons
    /// </summary>


    protected const float WRIST_SPIN_SCALE = 0.15f; //movement around the "arm" axis (Unity z-axis)
    protected const float WRIST_SWING_SCALE = 0.15f; //movement to swing the sword (Unity x-axis)


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


        float spinDelta = mouseEvent.PosDelta.x * -1; //have to reverse this to make rightward movement produce rightward spin
        float swingDelta = mouseEvent.PosDelta.y;

        //to allow the player to "calibrate" their mouse position, only actually move when a mouse button is down
        if (Input.GetMouseButton(0))
        {
            if (swingDelta != 0.0f)
            {
                deltaRotation = Quaternion.Euler(baseSwing * swingDelta * WRIST_SWING_SCALE * Time.deltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }


            if (spinDelta != 0.0f)
            {
                deltaRotation = Quaternion.Euler(baseRotation * spinDelta * WRIST_SPIN_SCALE * Time.deltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
        }
    }
}
