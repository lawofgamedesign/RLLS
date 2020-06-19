namespace Opponent {
    using UnityEngine;

    public class OpponentBehavior : Person
    {
        /// <summary>
        /// This basic opponent makes no attempt to block. It moves its hands to a given position, rotates its wrists into a striking position, and then attempts to strike the player.
        /// If the player blocks, this basic opponent pulls the sword back, repositions its hands, and strikes again.
        /// </summary>



        ///
        /// Fields
        /// 

        private string opponentObjName; //this gameobject's name


        private OpponentStances.Stances startStance;
        

        ///
        /// Functions
        ///

        
        
        public override void Setup()
        {
            base.Setup();
            opponentObjName = gameObject.name;
            transform.Find(OPPONENT_SWORD).GetComponent<SwordBehavior>().Setup();
            Services.Events.Register<SwordContactEvent>(WithdrawSequence);

            StrikeSequence(OpponentStances.Stances.High);
        }



        /// <summary>
        /// This function sets up a basic attack: adopt a given stance, and then attack.
        /// </summary>
        /// <param name="newTask">The starting stance</param>
        protected void StrikeSequence(OpponentStances.Stances newStance)
        {
            AdoptStanceTask stanceTask = new AdoptStanceTask(newStance, this);
            stanceTask.Then(new StrikeTask(this));

            Services.Tasks.AddTask(stanceTask);
        }


        /// <summary>
        /// This function has the opponent pull their sword back to the position it was in when they started the attack.
        /// </summary>
        /// <param name="e">A SwordContactEvent</param>
        protected void WithdrawSequence(global::Event e)
        {
            Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in WithdrawSequence().");

            SwordContactEvent contactEvent = e as SwordContactEvent;

            //make sure the contacting sword is this opponent's sword, not the player's sword, etc.
            if (contactEvent.rb.transform.parent.gameObject.name == opponentObjName)
            {
                Services.Tasks.AddTask(new AdoptStanceTask(startStance, this));
            }
        }
    }
}
