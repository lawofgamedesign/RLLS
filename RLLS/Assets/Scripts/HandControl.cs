using UnityEngine;

public class HandControl : MonoBehaviour
{

    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////


    //the rigidbody for the hands
    private Rigidbody rb;


    //hand movement
    Quaternion deltaRotation;
    private const float BASE_ROT_SPEED = 50.0f; //starting wrist rotation speed
    private Vector3 baseRotation = new Vector3(0.0f, 0.0f, BASE_ROT_SPEED);

    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////


    //register for mouse events and get the rigidbody so as to be able to respond to them
    public void Setup()
    {
        Services.Events.Register<MouseEvent>(MoveHands);
        rb = GetComponent<Rigidbody>();
    }


    //move hands based on player input
    private void MoveHands(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(MouseEvent), "Non-MouseEvent in MoveHands.");

        MouseEvent mouseEvent = e as MouseEvent;

        //move hands to appropriate position
        rb.MovePosition(Camera.main.ScreenToWorldPoint(new Vector3(mouseEvent.Pos.x, mouseEvent.Pos.y, 10.0f)));

        //rotate hands if either LMB or RMB is pressed
        if (mouseEvent.LMB || mouseEvent.RMB)
        {

            //set the amount of rotation for this frame
            if (mouseEvent.LMB) { deltaRotation = Quaternion.Euler(baseRotation * Time.deltaTime); } //rotate left for LMB
            else { deltaRotation = Quaternion.Euler(baseRotation * Time.deltaTime * -1.0f); } //rotate right for RMB
 
            //actually spin the hands
             rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}



