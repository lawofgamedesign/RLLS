﻿namespace Opponent
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
        }


        public override void Tick()
        {
            opponent.Rb.AddForce(MoveDirection() * opponent.MoveSpeed * Time.deltaTime, ForceMode.VelocityChange);
            opponent.Rb.MoveRotation(Quaternion.RotateTowards(opponent.Rb.rotation, Services.Stance.stances[OpponentStances.Stances.Extended].handRotation,
                opponent.CurrentRotSpeed * Time.deltaTime));
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
    }
}
