using UnityEngine;

public class KeyboardControl : HandControl
{


    public override void Setup()
    {
        base.Setup();


        //stop listening for mouse events; KeyboardControl doesn't rely on the mouse
        Services.Events.Unregister<BothMouseButtonsEvent>(SwordStateViaInput);
        Services.Events.Unregister<MouseEvent>(RotateHandsViaInput);
        Services.Events.Unregister<BothMouseButtonsUpEvent>(BladeDirectionFeedback);


        //listen for additional keyboard inputs
        Services.Events.Register<CardinalDirectionEvent>(RotateHandsViaKeyboardInput);
    }


    protected virtual void RotateHandsViaKeyboardInput(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(CardinalDirectionEvent), "Non-CardinalDirectionEvent in RotateHandsViaKeyboardInput.");

        CardinalDirectionEvent cardinalDirEvent = e as CardinalDirectionEvent;

        //swing or return if both buttons are pressed

        if (cardinalDirEvent.cardinalDir == KeyboardInput.CardinalDirections.North)
        {
            currentState = SwordState.Swinging;
            rb.MoveRotation(SwingOrReturn());
        } else if (cardinalDirEvent.cardinalDir == KeyboardInput.CardinalDirections.South)
        {
            currentState = SwordState.Returning;
            rb.MoveRotation(SwingOrReturn());
        }

        //rotate hands if the player is inputting the relevant keyboard keys--probably J or L
        else
        {

            //set the amount of rotation for this frame
            if (cardinalDirEvent.cardinalDir == KeyboardInput.CardinalDirections.West) { deltaRotation = Quaternion.Euler(baseRotation * Services.Speed.CurrentMultiplier * Time.deltaTime); } //rotate left for J
            else { deltaRotation = Quaternion.Euler(baseRotation * Services.Speed.CurrentMultiplier * Time.deltaTime * -1.0f); } //rotate right for L

            //actually spin the hands
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}
