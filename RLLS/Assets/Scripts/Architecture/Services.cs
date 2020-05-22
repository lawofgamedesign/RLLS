using UnityEngine;

public static class Services
{
    private static InputManager inputs;
    public static InputManager Inputs
    {
        get
        {
            Debug.Assert(inputs != null, "No input manager.");
            return inputs;
        }
        set { inputs = value; }
    }


    private static EventManager events;
    public static EventManager Events
    {
        get
        {
            Debug.Assert(events != null, "No event manager.");
            return events;
        }
        set { events = value; }
    }
}
