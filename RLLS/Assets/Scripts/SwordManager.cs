using UnityEngine;

public class SwordManager
{

    /// <summary>
    /// Swords
    /// </summary>
     
    public enum Swords { Player, Opponent }
    public SwordBehavior playerSword;
    public const string PLAYER_SWORD_OBJ = "Player 1 sword";
    public OpponentSwordBehavior opponentSword;
    public const string OPPONENT_SWORD_OBJ = "Player 2 sword";



    private const string SWORD_OBJ = "sword";


    //sword light intensity
    public enum SwordIntensity { Normal, Full }


    //sword edge positions for the "blade" particles
    public enum BladePositions { Forward, Backward }

 


    public void Setup()
    {
        playerSword = GameObject.Find(PLAYER_SWORD_OBJ).GetComponent<SwordBehavior>();
        playerSword.Setup();

        opponentSword = GameObject.Find(OPPONENT_SWORD_OBJ).GetComponent<OpponentSwordBehavior>();
        opponentSword.Setup();

        //Services.Events.Register<SwordContactEvent>(HandleParry);
    }


    public SwordIntensity GetIntensity(Swords sword)
    {
        return sword == Swords.Player ? playerSword.CurrentIntensity : opponentSword.CurrentIntensity;
    }


    public void ChangeIntensity(Swords sword, SwordIntensity intensity)
    {
        if (sword == Swords.Player) playerSword.ChangeIntensity(intensity);
        else if (sword == Swords.Opponent) opponentSword.ChangeIntensity(intensity);
    }


    public void ReversePlayerEdge(BladePositions newPos)
    {
        playerSword.ChangeEdgePos(newPos);
    }


    //private void HandleParry(global::Event e)
    //{
    //    Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in HandleParry");
    //
    //    ChangeIntensity(Swords.Player, SwordIntensity.Normal);
    //    ChangeIntensity(Swords.Opponent, SwordIntensity.Normal);
    //
    //    SwordContactEvent contactEvent = e as SwordContactEvent;
    //
    //    if (contactEvent.collision.gameObject.name.Contains(SWORD_OBJ))
    //    {
    //        Services.Swordfighters.SetVulnerability(Services.Swordfighters.Player, false);
    //        Services.Swordfighters.SetVulnerability(Services.Swordfighters.Opponent, false);
    //    }
    //}

}
