using UnityEngine;

public class SpeedManager
{
    private float overallMultiplier = 1.0f;
    public float OverallMultiplier { get; private set; }
    private float overallIncrement = 0.5f;
    private const string PLAYER_OBJ = "Player 1";


    public void Setup()
    {
        OverallMultiplier = overallMultiplier;
        Services.Events.Fire(new NewSpeedEvent(OverallMultiplier));
        Services.Events.Register<SwordContactEvent>(IncreaseOveralMultiplier);
    }


    public void IncreaseOveralMultiplier(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in IncreaseOverallMultiplier");

        SwordContactEvent swordEvent = e as SwordContactEvent;

        if (swordEvent.rb.transform.parent.gameObject.name == PLAYER_OBJ)
        {
            OverallMultiplier += overallIncrement;
            Services.Events.Fire(new NewSpeedEvent(OverallMultiplier));
        }
    }
}
