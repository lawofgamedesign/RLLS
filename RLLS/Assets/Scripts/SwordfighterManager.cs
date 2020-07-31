using UnityEngine;

public class SwordfighterManager
{

    private HandControl player;
    public HandControl Player { get; private set; }



    private Person opponent;
    public Person Opponent { get; private set; }

    private const string PLAYER_OBJ = "Player 1";
    private const string OPPONENT_OBJ = "Player 2";


    public void Setup()
    {
        player = GameObject.Find(PLAYER_OBJ).GetComponent<HandControl>();
        Player = player;

        opponent = GameObject.Find(OPPONENT_OBJ).GetComponent<Person>();
        Opponent = opponent;
    }



    public void SetVulnerability(Person swordfighter, bool isVulnerable)
    {
        swordfighter.ChangeVulnerability(isVulnerable);
    }


    public void ChangeOpponentType<T>() where T : Person
    {
        Debug.Assert(typeof(T) != typeof(HandControl) && typeof(T) != typeof(MouseControl) && typeof(T) != typeof(KeyboardControl), "Trying to turn the opponent into a player.");

        MonoBehaviour.Destroy(opponent);
        opponent = GameObject.Find(OPPONENT_OBJ).AddComponent<T>();
        opponent.Setup();
    }
}
