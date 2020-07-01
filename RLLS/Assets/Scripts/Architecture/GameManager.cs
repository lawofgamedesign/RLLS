using UnityEngine;

public class GameManager : MonoBehaviour {


    /// <summary>
    /// Fields
    /// </summary>

    protected const string PLAYER = "Player 1";
    protected const string OPPONENT_TAG = "Opponent";
    protected Person opponent;



    protected virtual void Start()
    {
        Services.Stance = new OpponentStances();
        Services.Stance.EstablishStances();
        Services.Stance.EstablishParries();
        Services.Events = new EventManager();
        Services.Speed = new SpeedManager();
        Services.Speed.Setup();
        Services.UI = new UIManager();
        Services.UI.Setup();
        Services.Inputs = new InputManager();
        Services.Tasks = new TaskManager();
        GameObject.Find(PLAYER).GetComponent<Person>().Setup();
        opponent = GameObject.FindGameObjectWithTag(OPPONENT_TAG).GetComponent<Person>();
        opponent.Setup();
    }

    //the only Update() permitted in the game! This calls everything that needs to act each frame.
    //Other scripts are permitted to *re*act through subscribing to events, etc.
    protected virtual void Update()
    {
        Services.Inputs.Tick();
        Services.Tasks.Tick();
    }
}
