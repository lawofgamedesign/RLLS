namespace Opponent
{
    using UnityEngine;

    public class BlockingOpponent : OpponentBehavior
    {

        protected Rigidbody player;
        protected Rigidbody playerSword;
        protected const string PLAYER_OBJ = "Player 1";
        protected const string SWORD_OBJ = " sword";


        //used to choose a parry in Parry()
        protected float SWORD_WIDE_POS_RIGHT = 0.5f;
        protected float SWORD_WIDE_POS_LEFT = -0.5f;



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


            //The logic used to choose a parry:
            //1. If the player is holding the sword vertically, parry high.
            //2. If the player is swinging from the side, are they swinging "wide" (starting from the side where the sword is pointed), or are they holding the sword across their body
            //and swinging mostly with wrist action?
            //2a. If the player is swinging wide, parry to the side.
            //2b. If the palyer is swinging across the body, parry centrally.


            //Calculate direction in which sword is being held
            Vector3 playerSwordDir = (playerSword.position - player.position).normalized;

            //high block
            if (playerSwordDir.y >= Mathf.Abs(playerSwordDir.x)) Services.Tasks.AddTask(new AdoptStanceTask(OpponentStances.Parries.High, this));

            //sword is held wide and coming in from the left
            else if (playerSwordDir.x < 0.0f && playerSword.position.x >= SWORD_WIDE_POS_LEFT)
            {
                Services.Tasks.AddTask(new AdoptStanceTask(OpponentStances.Parries.Right, this));
            }

            //sword is held wide and coming in from the right
            else if (playerSwordDir.x > 0.0f && playerSword.position.x <= SWORD_WIDE_POS_RIGHT)
            {
                Services.Tasks.AddTask(new AdoptStanceTask(OpponentStances.Parries.Left, this));
            }

            //sword is held across the body; parry center
            else Services.Tasks.AddTask(new AdoptStanceTask(OpponentStances.Parries.Center, this));
        }
    }
}
