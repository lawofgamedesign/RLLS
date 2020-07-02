using UnityEngine;

public class SwordManager
{

    /// <summary>
    /// Swords
    /// </summary>
    /// 
    public SwordBehavior playerSword;
    public const string PLAYER_SWORD_OBJ = "Player 1 sword";
    public OpponentSwordBehavior opponentSword;
    public const string OPPONENT_SWORD_OBJ = "Player 2 sword";


    public enum SwordIntensity { Normal, Full }

    public enum Swords { Player, Opponent }


    public void Setup()
    {
        playerSword = GameObject.Find(PLAYER_SWORD_OBJ).GetComponent<SwordBehavior>();
        playerSword.Setup();

        opponentSword = GameObject.Find(OPPONENT_SWORD_OBJ).GetComponent<OpponentSwordBehavior>();
        opponentSword.Setup();

        Services.Events.Register<SwordContactEvent>(HandleParry);
    }


    public SwordIntensity GetIntensity(Swords sword)
    {
        return sword == Swords.Player ? playerSword.CurrentIntensity : opponentSword.CurrentIntensity;
    }


    public void ChangeIntensity(Swords sword, SwordIntensity intensity)
    {
        if (sword == Swords.Player) playerSword.ChangeIntensity(intensity);
        else if (sword == Swords.Opponent) opponentSword.ChangeIntensity(intensity);
        else Debug.Log("Attempting to change intensity of a non-existent sword: " + sword.ToString());
    }


    private void HandleParry(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in HandleParry");

        ChangeIntensity(Swords.Player, SwordIntensity.Normal);
        ChangeIntensity(Swords.Opponent, SwordIntensity.Normal);
    }
}
