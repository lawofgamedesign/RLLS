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


    private static TaskManager tasks;
    public static TaskManager Tasks
    {
        get
        {
            Debug.Assert(tasks != null, "No task manager.");
            return tasks;
        }
        set { tasks = value; }
    }


    private static OpponentStances stance;
    public static OpponentStances Stance
    {
        get
        {
            Debug.Assert(stance != null, "No stance list.");
            return stance;
        }
        set { stance = value; }
    }


    private static SpeedManager speed;
    public static SpeedManager Speed
    {
        get
        {
            Debug.Assert(speed != null, "No speed manager.");
            return speed;
        }
        set { speed = value; }
    }


    private static UIManager uI;
    public static UIManager UI
    {
        get
        {
            Debug.Assert(uI != null, "No UI Manager.");
            return uI;
        }
        set { uI = value; }
    }


    private static SwordManager swords;
    public static SwordManager Swords
    {
        get
        {
            Debug.Assert(swords != null, "No sword manager.");
            return swords;
        }
        set { swords = value;  }
    }


    private static SwordfighterManager swordfighters;
    public static SwordfighterManager Swordfighters
    {
        get
        {
            Debug.Assert(swordfighters != null, "No swordfighter manager.");
            return swordfighters;
        }
        set { swordfighters = value; }
    }
}
