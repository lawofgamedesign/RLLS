namespace Opponent
{
    using UnityEngine;

    public class AdoptStanceTask : Task
    {
        //This task moves the hands to a new stance.
        //Note that "Extended" is not understood as a stance here.


        //stance
        private readonly OpponentStances.Stances destinationStance;

        //if, alternatively, this task is going to be used to parry, fill these in
        private readonly OpponentStances.Parries destinationParry;


        //delegate functions, depending on whether adopting a stance or parrying
        private delegate Vector3 movementFunction();
        private readonly movementFunction moveFunction;

        private delegate Quaternion rotationFunction();
        private readonly rotationFunction rotFunction;


        //delegate function for determining whether it is time to move on to the next position
        private delegate bool endFunction();
        private readonly endFunction doneFunction;


        //delegate function for registering for events, if necessary
        private delegate void registerFunction();
        private readonly registerFunction regFunction;


        //the opponent
        private readonly Person opponent;


        //margins of error; used to determine when the opponent is "close enough" to where they want to be
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
            moveFunction = new movementFunction(MoveToStance);
            rotFunction = new rotationFunction(RotateToStance);
            doneFunction = new endFunction(CheckStanceTolerances);
            regFunction = new registerFunction(StanceRegister);
        }


        /// <summary>
        /// Constructor for use when parrying.
        /// </summary>
        /// <param name="destinationParry">The intended parry.</param>
        /// <param name="opponent">The opponent.</param>
        public AdoptStanceTask(OpponentStances.Parries destinationParry, Person opponent)
        {
            this.destinationParry = destinationParry;
            this.opponent = opponent;
            moveFunction = new movementFunction(MoveToParry);
            rotFunction = new rotationFunction(RotateToParry);
            doneFunction = new endFunction(CheckParryComplete);
            regFunction = new registerFunction(ParryRegister);
        }


        /// <summary>
        /// Start listening for the opponent to want to parry, in which case this task should stop.
        /// </summary>
        protected override void Init()
        {
            Services.Events.Register<ParryEvent>(Stop);
            regFunction();
        }


        /// <summary>
        /// If the opponent wants to parry, stop this task so that the opponent can adopt a parrying stance.
        /// </summary>
        /// <param name="e">A ParryEvent</param>
        public virtual void Stop(global::Event e)
        {
            SetStatus(TaskStatus.Aborted);
        }



        protected void StanceRegister()
        {
            return;
        }



        protected void ParryRegister()
        {
            Services.Events.Register<BothMouseButtonsUpEvent>(ReleaseFromParry);
        }



        public override void Tick()
        {
            opponent.Rb.AddForce(ApplyMovement(moveFunction()), ForceMode.VelocityChange);
            opponent.Rb.MoveRotation(rotFunction());
            if (doneFunction()) SetStatus(TaskStatus.Success); //only really used when adopting a stance
        }


        private Vector3 MoveToStance()
        {
            return (Services.Stance.stances[destinationStance].worldPosition - opponent.Rb.position).normalized;
        }



        private Vector3 MoveToParry()
        {
            return (Services.Stance.parries[destinationParry].worldPosition - opponent.Rb.position).normalized;
        }



        protected Vector3 ApplyMovement(Vector3 dir)
        {
            return dir * opponent.MoveSpeed * Time.deltaTime * Services.Speed.OverallMultiplier;
        }


        private Quaternion RotateToStance()
        {
            return Quaternion.RotateTowards(opponent.Rb.rotation, Services.Stance.stances[destinationStance].handRotation, opponent.RotSpeed * Services.Speed.OverallMultiplier * Time.deltaTime);
        }


        private Quaternion RotateToParry()
        {
            return Quaternion.RotateTowards(opponent.Rb.rotation, Services.Stance.parries[destinationParry].handRotation, opponent.RotSpeed * Services.Speed.OverallMultiplier * Time.deltaTime);
        }


        /// <summary>
        /// Adopting a stance is complete when the opponent has reached the stance.
        /// </summary>
        /// <returns>True if the opponent's position and rotation are within tolerances of the intended position, false otherwise.</returns>
        private bool CheckStanceTolerances()
        {
            if (Vector3.Distance(opponent.Rb.position, Services.Stance.stances[destinationStance].worldPosition) <= distTolerance)
            {
                if (Vector3.Angle(opponent.Rb.rotation.eulerAngles, Services.Stance.stances[destinationStance].handRotation.eulerAngles) <= angleTolerance)
                {
                    Services.Swords.ChangeIntensity(SwordManager.Swords.Opponent, SwordManager.SwordIntensity.Full);
                    Services.Swordfighters.SetVulnerability(Services.Swordfighters.Player, true);
                    return true;
                }
            }

            return false;
        }



        private bool CheckParryComplete()
        {
            return false;
        }


        /// <summary>
        /// If the player has stopped swinging, stop parrying.
        /// </summary>
        /// <param name="e">A BothMouseButtonsUpEvent</param>
        private void ReleaseFromParry(global::Event e)
        {
            Debug.Assert(e.GetType() == typeof(BothMouseButtonsUpEvent), "Non-BothMouseButtonsUpEvent in ReleaseFromParry.");

            Services.Events.Unregister<BothMouseButtonsUpEvent>(ReleaseFromParry);
            SetStatus(TaskStatus.Success);
        }


        /// <summary>
        /// If there's no established next thing to do, inform OpponentBehavior that the opponent needs to proceed.
        /// </summary>
        protected override void OnSuccess()
        {
            if (NextTask == null) Services.Events.Fire(new ReadyEvent());
        }



        /// <summary>
        /// No matter what, be sure to unregister for ParryEvents.
        /// </summary>
        protected override void Cleanup()
        {
            Services.Events.Unregister<ParryEvent>(Stop);
        }




    }
}
