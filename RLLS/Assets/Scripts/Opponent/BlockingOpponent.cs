namespace Opponent
{
    using UnityEngine;

    public class BlockingOpponent : OpponentBehavior
    {

        protected Rigidbody player;
        protected Rigidbody playerSword;
        protected const string PLAYER_OBJ = "Player 1";
        protected const string SWORD_OBJ = " sword";



        /// <summary>
        /// In addition to the normal setup, a blocking opponent listens for the need to parry, and to otherwise set up to perform parries.
        /// </summary>
        public override void Setup()
        {
            base.Setup();

            Services.Events.Register<StrikingEvent>(Parry);

            player = GameObject.Find(PLAYER_OBJ).GetComponent<Rigidbody>();
            playerSword = GameObject.Find(PLAYER_OBJ + SWORD_OBJ).GetComponent<Rigidbody>();
        }


        protected virtual void Parry(global::Event e)
        {
            Debug.Assert(e.GetType() == typeof(StrikingEvent), "Non-StrikingEvent in Parry.");

            //don't attempt to parry if currently striking
            if (Services.Tasks.CheckForTaskOfType<StrikeTask>()) return;

            //Announce that the opponent is going to parry; important for AdoptStanceTask to know that it needs to abort so that a parrying stance can be adopted 
            Services.Events.Fire(new ParryEvent());


            //Calculate direction in which sword is being held, as a hacky way to decide which way to parry
            Vector3 playerSwordDir = (playerSword.position - player.position).normalized;

            if (playerSwordDir.y >= Mathf.Abs(playerSwordDir.x)) Services.Tasks.AddTask(new AdoptStanceTask(OpponentStances.Parries.High, this));
            else if (playerSwordDir.x < 0.0f) Services.Tasks.AddTask(new AdoptStanceTask(OpponentStances.Parries.Right, this));
            else Services.Tasks.AddTask(new AdoptStanceTask(OpponentStances.Parries.Left, this));
        }
    }
}
