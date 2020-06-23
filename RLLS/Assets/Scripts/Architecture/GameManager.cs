using UnityEngine;

public class GameManager : MonoBehaviour {


    /// <summary>
    /// Fields
    /// </summary>

    private const string PLAYER = "Player 1";
    private const string OPPONENT_TAG = "Opponent";
    private Person opponent;



    private void Start()
    {
        Services.Stance = new OpponentStances();
        Services.Stance.EstablishStances();
        Services.Events = new EventManager();
        Services.Inputs = new InputManager();
        Services.Tasks = new TaskManager();
        Services.Speed = new SpeedManager();
        Services.Speed.Setup();
        GameObject.Find(PLAYER).GetComponent<Person>().Setup();
        opponent = GameObject.FindGameObjectWithTag(OPPONENT_TAG).GetComponent<Person>();
        opponent.Setup();
    }

    //the only Update() permitted in the game! This calls everything that needs to act each frame.
    //Other scripts are permitted to *re*act through subscribing to events, etc.
    private void Update()
    {
        Services.Inputs.Tick();
        Services.Tasks.Tick();
    }
}
