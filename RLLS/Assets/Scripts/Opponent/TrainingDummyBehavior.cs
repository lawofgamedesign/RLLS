namespace Opponent
{
    using UnityEngine;

    public class TrainingDummyBehavior : OpponentBehavior
    {



        protected override void ChooseStyle()
        {
            Services.Events.Register<KeypressEvent>(TrainingStrike);
        }


        /// <summary>
        /// The training dummy doesn't make the first move.
        /// </summary>
        protected override void MakeFirstMove()
        {
            return;
        }



        protected void TrainingStrike(global::Event e)
        {
            Debug.Assert(e.GetType() == typeof(KeypressEvent), "Non-KeypressEvent in TrainingStrike.");

            StrikeSequence(OpponentStances.Stances.High_Left);
        }



        /// <summary>
        /// This function has the opponent pull their sword back to the position it was in when they started the attack.
        /// </summary>
        /// <param name="e">A SwordContactEvent</param>
        protected override void WithdrawSequence(global::Event e)
        {
            Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in WithdrawSequence().");

            SwordContactEvent contactEvent = e as SwordContactEvent;

            //make sure the contacting sword is this opponent's sword, not the player's sword, etc.
            if (contactEvent.rb.transform.parent.gameObject.name == opponentObjName)
            {
                AdoptStanceTask withdrawTask = new AdoptStanceTask(startStance, this);
                withdrawTask.Then(new AdoptStanceTask(OpponentStances.Stances.Neutral, this));
                Services.Tasks.AddTaskExclusive(withdrawTask);
            }
        }
    }
}
