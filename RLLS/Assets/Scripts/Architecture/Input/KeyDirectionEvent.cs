using UnityEngine;

public class KeyDirectionEvent : Event
{
    //Directions
    
    public readonly InputManager.Directions direction;

    public KeyDirectionEvent(InputManager.Directions dir)
    {
        direction = dir;
    }
}
