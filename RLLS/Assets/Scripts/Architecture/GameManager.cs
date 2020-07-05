using UnityEngine;

public class GameManager : MonoBehaviour {


    /// <summary>
    /// Fields
    /// </summary>



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
        Services.Swords = new SwordManager();
        Services.Swords.Setup();
        Services.Swordfighters = new SwordfighterManager();
        Services.Swordfighters.Setup();
        Services.Swordfighters.Player.Setup();
        Services.Swordfighters.Opponent.Setup();
    }

    //the only Update() permitted in the game! This calls everything that needs to act each frame.
    //Other scripts are permitted to *re*act through subscribing to events, etc.
    protected virtual void Update()
    {
        Services.Inputs.Tick();
        Services.Tasks.Tick();
    }
}
