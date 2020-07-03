using UnityEngine;

public class TrainingManager : GameManager
{
    protected override void Start()
    {
        Services.Stance = new OpponentStances();
        Services.Stance.EstablishStances();
        Services.Events = new EventManager();
        Services.Speed = new SpeedManager();
        Services.Speed.Setup();
        Services.UI = new UIManager();
        Services.UI.Setup();
        Services.Inputs = new TrainingInputManager();
        Services.Tasks = new TaskManager();
        Services.Swordfighters = new SwordfighterManager();
        Services.Swordfighters.Setup();
        Services.Swordfighters.Player.Setup();
        Services.Swordfighters.Opponent.Setup();
    }
}
