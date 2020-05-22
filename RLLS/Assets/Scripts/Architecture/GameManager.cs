using UnityEngine;

public class GameManager : MonoBehaviour {


    private const string P1_HANDS = "Player 1 hands";



    private void Start()
    {
        Services.Events = new EventManager();
        Services.Inputs = new InputManager();
        GameObject.Find(P1_HANDS).GetComponent<HandControl>().Setup();
    }

    // Update is called once per frame
    private void Update()
    {
        Services.Inputs.Tick();
    }
}
