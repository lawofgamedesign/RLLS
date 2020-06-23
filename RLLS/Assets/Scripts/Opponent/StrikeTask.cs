namespace Opponent
{
    using UnityEngine;

    public class StrikeTask : Task
    {
        //This task moves the extended, "strike" position.
        //It is, effectively, a specialized AdoptStanceTask, separated out for clarity.


        private readonly Person opponent;


        private readonly float distTolerance = 0.1f; //in Unity units
        private readonly float angleTolerance = 1.0f; //in degrees



        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="opponent">The Person script for the opponent.</param>
        public StrikeTask(Person opponent)
        {
            this.opponent = opponent;
            Services.Events.Register<SwordContactEvent>(DetectContact);
        }


        public override void Tick()
        {
            opponent.Rb.AddForce(ApplyMovement(MoveDirection()), ForceMode.VelocityChange);
            opponent.Rb.MoveRotation(Quaternion.RotateTowards(opponent.Rb.rotation, Services.Stance.stances[OpponentStances.Stances.Extended].handRotation,
                opponent.RotSpeed * Services.Speed.OverallMultiplier * Time.deltaTime));
            if (CheckTolerances()) SetStatus(TaskStatus.Success);
        }


        /// <summary>
        /// Move the opponent's hands toward the center, "extended" position.
        /// </summary>
        /// <returns>A vector toward the center position.</returns>
        private Vector3 MoveDirection()
        {
            return (Services.Stance.stances[OpponentStances.Stances.Extended].worldPosition - opponent.Rb.position).normalized;
        }



        protected Vector3 ApplyMovement(Vector3 dir)
        {
            return dir * opponent.MoveSpeed * Time.deltaTime * Services.Speed.OverallMultiplier;
        }




        private bool CheckTolerances()
        {
            if (Vector3.Distance(opponent.Rb.position, Services.Stance.stances[OpponentStances.Stances.Extended].worldPosition) <= distTolerance)
            {
                if (Vector3.Angle(opponent.Rb.rotation.eulerAngles, Services.Stance.stances[OpponentStances.Stances.Extended].handRotation.eulerAngles) <= angleTolerance)
                {
                    return true;
                }
            }

            return false;
        }


        private void DetectContact(global::Event e)
        {
            Debug.Assert(e.GetType() == typeof(SwordContactEvent), "Non-SwordContactEvent in DetectBlock().");

            SetStatus(TaskStatus.Aborted);
        }
    }
}
