using UnityEngine;

public class KeyboardInput : InputManager
{
    /// <summary>
    /// Handle all gameplay input using the keyboard. To accomplish this, send two sets of directions: up-down-left-right and north-south-west-east.
    /// </summary>


    public enum CardinalDirections { North, South, West, East }


    public override void Setup()
    {
        //intentionally blank
    }



    public override void Tick()
    {
        GetDirection();
        GetCardinalDirection();
    }


    protected virtual void GetCardinalDirection()
    {
        if (Input.GetKey(KeyCode.I)) Services.Events.Fire(new CardinalDirectionEvent(CardinalDirections.North));
        else if (Input.GetKey(KeyCode.K)) Services.Events.Fire(new CardinalDirectionEvent(CardinalDirections.South));


        if (Input.GetKey(KeyCode.J)) Services.Events.Fire(new CardinalDirectionEvent(CardinalDirections.West));
        else if (Input.GetKey(KeyCode.L)) Services.Events.Fire(new CardinalDirectionEvent(CardinalDirections.East));
    }
}
