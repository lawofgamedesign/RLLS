using UnityEngine;

public class GameManager : MonoBehaviour {
   
    private void Start()
    {
        Services.Events = new EventManager();
        Services.Inputs = new InputManager();
    }

    // Update is called once per frame
    private void Update()
    {
        Services.Inputs.Tick();
    }
}
