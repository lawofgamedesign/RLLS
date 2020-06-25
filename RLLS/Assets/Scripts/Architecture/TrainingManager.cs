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
        GameObject.Find(PLAYER).GetComponent<Person>().Setup();
        opponent = GameObject.FindGameObjectWithTag(OPPONENT_TAG).GetComponent<Person>();
        opponent.Setup();
    }
}
