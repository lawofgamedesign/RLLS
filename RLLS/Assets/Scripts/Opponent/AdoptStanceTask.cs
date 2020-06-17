namespace Opponent
{
    using UnityEngine;

    public class AdoptStanceTask : Task
    {
        //This task moves the hands to a new stance.
        //Note that "Extended" is not understood as a stance here.


        private readonly OpponentStances.Stances destinationStance;
        private readonly Person opponent;



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="destinationStance">The stance the opponent intends to achieve.</param>
        /// <param name="opponent">The opponent.</param>
        public AdoptStanceTask(OpponentStances.Stances destinationStance, Person opponent)
        {
            this.destinationStance = destinationStance;
            this.opponent = opponent;
        }



        public override void Tick()
        {
            opponent.Rb.MovePosition(opponent.Rb.position + (MoveDirection() * opponent.MoveSpeed * Time.deltaTime)); 
        }


        protected Vector3 MoveDirection()
        {
            Debug.Assert(Services.Stance.stances != null, "No stances.");
            Debug.Assert(Services.Stance.stances[destinationStance] != null, "No intended stance.");
            Debug.Assert(opponent != null, "No opponent.");
            Debug.Assert(opponent.Rb != null, "No rigidbody.");
            return Services.Stance.stances[destinationStance].worldPosition - opponent.Rb.position;
        }
    }
}
