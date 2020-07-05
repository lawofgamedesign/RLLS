using UnityEngine;

public class GameManager : MonoBehaviour {


    /// <summary>
    /// Fields
    /// </summary>


    //used to end the game
    private const float SLOWMO_SPEED = 0.1f;
    private const string SWORD_OBJ = "sword";



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

        Services.Events.Register<SwordContactEvent>(DeclareWinner);
    }

    //the only Update() permitted in the game! This calls everything that needs to act each frame.
    //Other scripts are permitted to *re*act through subscribing to events, etc.
    protected virtual void Update()
    {
        Services.Inputs.Tick();
        Services.Tasks.Tick();
    }


    public void DeclareWinner(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in DeclareWinner");

        SwordContactEvent contactEvent = e as SwordContactEvent;

        if (contactEvent.collision.gameObject.name.Contains(SWORD_OBJ)) return; //do nothing if the swords clash

        Time.timeScale = SLOWMO_SPEED;

        SlowmoTask slowTask = new SlowmoTask();
        slowTask.Then(new EndGameTask());
        Services.Tasks.AddTaskExclusive(slowTask);

        Services.Events.Unregister<SwordContactEvent>(DeclareWinner);
    }
}
