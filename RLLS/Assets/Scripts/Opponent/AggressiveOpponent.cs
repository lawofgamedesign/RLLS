namespace Opponent {
    using UnityEngine;

    public class AggressiveOpponent : Person
    {
        /// <summary>
        /// AggressiveOpponent makes no attempt to block. It moves its hands to a given position, rotates its wrists into a striking position, and then attempts to strike the player.
        /// If the player blocks, AggressiveOpponent pulls the sword back, repositions its hands, and strikes again.
        /// </summary>








        public override void Setup()
        {
            base.Setup();
            transform.Find(OPPONENT_SWORD).GetComponent<SwordBehavior>().Setup();

            AdoptStanceTask raiseTask = new AdoptStanceTask(OpponentStances.Stances.High, this);
            raiseTask.Then(new StrikeTask(this));

            Services.Tasks.AddTask(raiseTask);
        }
    }
}
