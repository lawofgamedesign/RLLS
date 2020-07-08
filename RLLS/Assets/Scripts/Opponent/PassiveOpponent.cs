namespace Opponent
{

    using UnityEngine;

    public class PassiveOpponent : OpponentBehavior
    {
        /// <summary>
        /// PassiveOpponent doesn't do anything.
        /// </summary>
        protected override void ChooseStyle()
        {
            return;
        }


        /// <summary>
        /// PassiveOpponent never makes a move.
        /// </summary>
        protected override void MakeFirstMove()
        {
            return;
        }
    }
}
