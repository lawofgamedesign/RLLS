using UnityEngine;

public class GameManager : MonoBehaviour {


    /// <summary>
    /// Fields
    /// </summary>


    //used to end the game
    private const string SWORD_OBJ = "sword";
    private const string SCENERY_TAG = "Scenery";
    private const string OPPONENT_OBJ = "Player 2";



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
        Services.Inputs = new MouseInput();
        Services.Inputs.Setup();
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

        Debug.Log(contactEvent.collision.gameObject.name);
        Debug.Log(Services.Swords.GetIntensity(SwordManager.Swords.Player).ToString());

        if (contactEvent.collision.gameObject.name.Contains(SWORD_OBJ) ||
            contactEvent.collision.gameObject.tag == SCENERY_TAG ||
            (contactEvent.collision.gameObject.name.Contains (OPPONENT_OBJ) &&
            Services.Swords.GetIntensity(SwordManager.Swords.Player) == SwordManager.SwordIntensity.Normal)) return; //do nothing if the sword didn't hit a swordfighter, or if the sword isn't ready to strike

        SlowmoTask slowTask = new SlowmoTask();
        slowTask.Then(new EndGameTask());
        Services.Tasks.AddTaskExclusive(slowTask);

        Services.Events.Unregister<SwordContactEvent>(DeclareWinner);
    }
}
