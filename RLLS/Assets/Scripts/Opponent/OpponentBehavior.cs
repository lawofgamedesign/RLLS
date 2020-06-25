namespace Opponent {
    using System.Collections;
    using System.Collections.Generic;
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

        //this gameobject's name
        protected string opponentObjName;


        //the stance the opponent started an attack from. Note that this is *not* necessarily the stance it was in when it started the match!
        protected OpponentStances.Stances startStance;
        



        ///
        /// Functions
        ///

        
        public override void Setup()
        {
            base.Setup();
            opponentObjName = gameObject.name;
            transform.Find(OPPONENT_SWORD).GetComponent<SwordBehavior>().Setup();
            Services.Events.Register<SwordContactEvent>(WithdrawSequence);

            ChooseStyle();
            MakeFirstMove();
        }



        /// <summary>
        /// Sets up opponent's swordfighting: how do they respond when they have finished an attack, etc.
        /// </summary>
        protected virtual void ChooseStyle()
        {
            Services.Events.Register<ReadyEvent>(RandomAttack);
        }


        /// <summary>
        /// Do whatever the opponent should do first. For a basic opponent, this is a high attack.
        /// </summary>
        protected virtual void MakeFirstMove()
        {
            StrikeSequence(OpponentStances.Stances.High);
        }


        #region attacks

        /// <summary>
        /// Initiate a random attack: adopt a randomly-chosen stance, and then swing.
        /// </summary>
        /// <param name="e">A ReadyEvent, such as those sent by AdoptStanceTask when withdrawing</param>
        protected void RandomAttack(global::Event e)
        {
            Debug.Assert(e.GetType() == typeof(ReadyEvent), "Non-ReadyEvent in RandomAttack().");
    
            StrikeSequence(OpponentStances.GetRandomStance());
        }



        /// <summary>
        /// This function sets up a basic attack: adopt a given stance, and then attack.
        /// </summary>
        /// <param name="newTask">The starting stance</param>
        protected void StrikeSequence(OpponentStances.Stances newStance)
        {
            startStance = newStance;

            AdoptStanceTask stanceTask = new AdoptStanceTask(newStance, this);
            stanceTask.Then(new StrikeTask(this));

            Services.Tasks.AddTask(stanceTask);
        }


        /// <summary>
        /// This function has the opponent pull their sword back to the position it was in when they started the attack.
        /// </summary>
        /// <param name="e">A SwordContactEvent</param>
        protected virtual void WithdrawSequence(global::Event e)
        {
            Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in WithdrawSequence().");

            SwordContactEvent contactEvent = e as SwordContactEvent;

            //make sure the contacting sword is this opponent's sword, not the player's sword, etc.
            if (contactEvent.rb.transform.parent.gameObject.name == opponentObjName)
            {
                Services.Tasks.AddTaskExclusive(new AdoptStanceTask(startStance, this));
            }
        }
    }


    #endregion
}
