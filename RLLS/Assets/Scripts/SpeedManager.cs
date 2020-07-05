using UnityEngine;

public class SpeedManager
{
    private float overallMultiplier = 1.0f;
    public float OverallMultiplier { get; private set; }
    private float overallIncrement = 0.3f;
    private float strikePenalty = 0.5f; //strikes are slightly slower than repositioning, to promote blocking
    public float StrikePenalty { get; private set; }
    private const string PLAYER_OBJ = "Player 1";
    private const string OPPONENT_OBJ = "Player 2";
    private const string BODY_OBJ = " body";
    private const string HEAD_OBJ = " head";
    private const string OPPONENT_SWORD_OBJ = " sword";
    private float vulnerableMultiplier = 3.0f;
    public float VulnerableMultiplier { get; private set; }
    protected const string STRIKE_MSG = "Strike\nnow!";
    protected const string SPEED_UP_MSG = "Too slow\nto strike!";
    protected const string SUCCESS_MSG = "You win!";
    protected const string LOSE_MSG = "You lose!";


    public void Setup()
    {
        OverallMultiplier = overallMultiplier;
        Time.timeScale = OverallMultiplier; //make sure the game is actually at the advertised speed
        StrikePenalty = strikePenalty;
        VulnerableMultiplier = vulnerableMultiplier;
        Services.Events.Fire(new NewSpeedEvent(OverallMultiplier));
        Services.Events.Register<SwordContactEvent>(IncreaseOveralMultiplier);
    }


    public void IncreaseOveralMultiplier(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in IncreaseOverallMultiplier");

        SwordContactEvent swordEvent = e as SwordContactEvent;

        if (swordEvent.rb.transform.parent.gameObject.name == PLAYER_OBJ && swordEvent.collision.collider.gameObject.name == OPPONENT_OBJ + OPPONENT_SWORD_OBJ)
        {
            OverallMultiplier += overallIncrement;
            Services.Events.Fire(new NewSpeedEvent(OverallMultiplier));

            //if (Services.Speed.OverallMultiplier >= vulnerableMultiplier) Services.UI.ChangeExplanatoryMessage(STRIKE_MSG);
        }
        
        //else if (swordEvent.collision.collider.gameObject.name.Contains(OPPONENT_OBJ) &&
        //            Services.Speed.OverallMultiplier < vulnerableMultiplier) Services.UI.ChangeExplanatoryMessage(SPEED_UP_MSG);
        //else if (swordEvent.collision.collider.gameObject.name.Contains(OPPONENT_OBJ) &&
        //            Services.Speed.OverallMultiplier >= vulnerableMultiplier) Services.UI.ChangeExplanatoryMessage(SUCCESS_MSG);
        //else if ((swordEvent.collision.collider.gameObject.name == PLAYER_OBJ + BODY_OBJ) || (swordEvent.collision.collider.gameObject.name == PLAYER_OBJ + HEAD_OBJ) &&
        //            Services.Speed.OverallMultiplier >= vulnerableMultiplier) Services.UI.ChangeExplanatoryMessage(LOSE_MSG);
    }
}
