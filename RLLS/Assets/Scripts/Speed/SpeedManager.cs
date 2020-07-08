using UnityEngine;

public class SpeedManager
{
    //speeds
    protected const float START_MULTIPLIER = 1.0f;
    protected const float MAX_MULTIPLIER = 5.0f;
    public float CurrentMultiplier { get; private set; }
    private float baseIncrement = 1.0f;
    private float strikePenalty = 0.5f; //strikes are slightly slower than repositioning, to promote blocking
    public float StrikePenalty { get; private set; }
    protected const string ANIMATION_CURVE_OBJ = "AnimationCurvesObj";


    //objects in the game, to determine what the sword touched
    private const string PLAYER_OBJ = "Player 1";
    private const string OPPONENT_OBJ = "Player 2";
    private const string BODY_OBJ = " body";
    private const string HEAD_OBJ = " head";
    private const string OPPONENT_SWORD_OBJ = " sword";


    //if using the speed variant, these establish when the swordfighters become vulnerable
    private float vulnerableMultiplier = 3.0f;
    public float VulnerableMultiplier { get; private set; }


    //UI messages
    protected const string STRIKE_MSG = "Strike\nnow!";
    protected const string SPEED_UP_MSG = "Too slow\nto strike!";
    protected const string SUCCESS_MSG = "You win!";
    protected const string LOSE_MSG = "You lose!";


    protected Utilities.AnimationCurves curves;


    public void Setup()
    {
        Time.timeScale = 1.0f; //make sure the game is ready to start after a reset
        CurrentMultiplier = START_MULTIPLIER;
        StrikePenalty = strikePenalty;
        VulnerableMultiplier = vulnerableMultiplier;
        Services.Events.Fire(new NewSpeedEvent(CurrentMultiplier));
        Services.Events.Register<SwordContactEvent>(IncreaseCurrentMultiplier);
        curves = Resources.Load<Utilities.AnimationCurves>(ANIMATION_CURVE_OBJ);
    }


    public void IncreaseCurrentMultiplier(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in IncreaseOverallMultiplier");

        SwordContactEvent swordEvent = e as SwordContactEvent;

        if (swordEvent.rb.transform.parent.gameObject.name == PLAYER_OBJ && swordEvent.collision.collider.gameObject.name == OPPONENT_OBJ + OPPONENT_SWORD_OBJ)
        {
            CurrentMultiplier += GetNextIncrement();
            Services.Events.Fire(new NewSpeedEvent(CurrentMultiplier));

            //if (Services.Speed.OverallMultiplier >= vulnerableMultiplier) Services.UI.ChangeExplanatoryMessage(STRIKE_MSG);
        }
        
        //else if (swordEvent.collision.collider.gameObject.name.Contains(OPPONENT_OBJ) &&
        //            Services.Speed.OverallMultiplier < vulnerableMultiplier) Services.UI.ChangeExplanatoryMessage(SPEED_UP_MSG);
        //else if (swordEvent.collision.collider.gameObject.name.Contains(OPPONENT_OBJ) &&
        //            Services.Speed.OverallMultiplier >= vulnerableMultiplier) Services.UI.ChangeExplanatoryMessage(SUCCESS_MSG);
        //else if ((swordEvent.collision.collider.gameObject.name == PLAYER_OBJ + BODY_OBJ) || (swordEvent.collision.collider.gameObject.name == PLAYER_OBJ + HEAD_OBJ) &&
        //            Services.Speed.OverallMultiplier >= vulnerableMultiplier) Services.UI.ChangeExplanatoryMessage(LOSE_MSG);
    }


    private float GetNextIncrement()
    {
        return baseIncrement * curves.rapidDecline.Evaluate(CurrentMultiplier / MAX_MULTIPLIER);
    }
}
