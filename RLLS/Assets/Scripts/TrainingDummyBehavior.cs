namespace Opponent
{
    using UnityEngine;

    public class TrainingDummyBehavior : OpponentBehavior
    {
        /// <summary>
        /// Fields
        /// </summary>

        protected Vector3 handStartPos = new Vector3(0.0f, 0.0f, 1.0f);

        

        public override void Setup()
        {
            Debug.Log("Setup called");
            rb = GetComponent<Rigidbody>();
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
                Debug.Log("Raising sword");
                rb.AddForce(Vector3.up * Time.deltaTime * moveSpeed, ForceMode.VelocityChange);
            }
            else if (Time.time < 2.5f) return; //pause for the player to get into position
            else if (Time.time <= 4.0f)
            {
                Debug.Log("Lowering sword");
                rb.AddForce(Vector3.down * Time.deltaTime * moveSpeed, ForceMode.VelocityChange);
                rb.MoveRotation(SwingOrReturn());
            }
        }
    }

}
