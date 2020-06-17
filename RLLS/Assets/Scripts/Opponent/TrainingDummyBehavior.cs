namespace Opponent
{
    using UnityEngine;

    public class TrainingDummyBehavior : Person
    {
        /// <summary>
        /// Fields
        /// </summary>

        protected Vector3 handStartPos = new Vector3(0.0f, 0.0f, 1.0f);
        private const string SWORD_OBJ = "Player 2 sword";

        

        public override void Setup()
        {
            rb = GetComponent<Rigidbody>();
            transform.Find(SWORD_OBJ).GetComponent<SwordBehavior>().Setup();
        }


        public void Tick()
        {
            AutoStrike();
        }


        protected void AutoStrike()
        {
            if (Time.time <= 0.5f) return; //pause for the player to get oriented
            if (Time.time <= 1.5f)
            {
                rb.AddForce(Vector3.up * Time.deltaTime * moveSpeed, ForceMode.VelocityChange);
            }
            else if (Time.time < 2.5f) return; //pause for the player to get into position
            else if (Time.time <= 4.0f)
            {
                rb.AddForce(Vector3.down * Time.deltaTime * moveSpeed, ForceMode.VelocityChange);
                rb.MoveRotation(SwingOrReturn());
            }
        }
    }

}
