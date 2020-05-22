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
}
