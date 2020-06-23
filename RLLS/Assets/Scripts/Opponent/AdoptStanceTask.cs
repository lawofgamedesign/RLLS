namespace Opponent
{
    using UnityEngine;

    public class AdoptStanceTask : Task
    {
        //This task moves the hands to a new stance.
        //Note that "Extended" is not understood as a stance here.


        private readonly OpponentStances.Stances destinationStance;
        private readonly Person opponent;


        private readonly float distTolerance = 0.1f; //in Unity units
        private readonly float angleTolerance = 1.0f; //in degrees



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
            opponent.Rb.AddForce(ApplyMovement(MoveDirection()), ForceMode.VelocityChange);
            opponent.Rb.MoveRotation(Quaternion.RotateTowards(opponent.Rb.rotation, Services.Stance.stances[destinationStance].handRotation,
                opponent.RotSpeed * Services.Speed.OverallMultiplier * Time.deltaTime));
            if (CheckTolerances()) SetStatus(TaskStatus.Success);
        }


        private Vector3 MoveDirection()
        {
            return (Services.Stance.stances[destinationStance].worldPosition - opponent.Rb.position).normalized;
        }



        protected Vector3 ApplyMovement(Vector3 dir)
        {
            return dir * opponent.MoveSpeed * Time.deltaTime * Services.Speed.OverallMultiplier;
        }


        private bool CheckTolerances()
        {
            if (Vector3.Distance(opponent.Rb.position, Services.Stance.stances[destinationStance].worldPosition) <= distTolerance)
            {
                if (Vector3.Angle(opponent.Rb.rotation.eulerAngles, Services.Stance.stances[destinationStance].handRotation.eulerAngles) <= angleTolerance)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// If there's no established next thing to do, inform OpponentBehavior that the opponent needs to proceed.
        /// </summary>
        protected override void OnSuccess()
        {
            if (NextTask == null) Services.Events.Fire(new ReadyEvent());
        }
    }
}
