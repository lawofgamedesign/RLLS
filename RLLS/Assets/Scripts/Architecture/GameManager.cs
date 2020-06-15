using UnityEngine;

public class GameManager : MonoBehaviour {


    private const string P1_HANDS = "Player 1";
    private const string OPPONENT_TAG = "Opponent";



    private void Start()
    {
        Services.Events = new EventManager();
        Services.Inputs = new InputManager();
        Services.Tasks = new TaskManager();
        GameObject.Find(P1_HANDS).GetComponent<HandControl>().Setup();
        GameObject.FindGameObjectWithTag(OPPONENT_TAG).GetComponent<HandControl>().Setup();
    }

    //the only Update() permitted in the game! This calls everything that needs to act each frame.
    //Other scripts are permitted to *re*act through subscribing to events, etc.
    private void Update()
    {
        Services.Inputs.Tick();
        Services.Tasks.Tick();
        GameObject.FindGameObjectWithTag(OPPONENT_TAG).GetComponent<Opponent.TrainingDummyBehavior>().Tick();
    }
}
