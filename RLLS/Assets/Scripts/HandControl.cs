using UnityEngine;

public class HandControl : MonoBehaviour
{

    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////


    //the rigidbody for the hands
    private Rigidbody rb;


    //hand movement
    private Vector3 prevLoc = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 newLoc = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 delta;
    private float moveSpeed = 100.0f;


    //hand rotation
    Quaternion deltaRotation;
    private const float BASE_ROT_SPEED = 50.0f; //starting wrist rotation speed
    private Vector3 baseRotation = new Vector3(0.0f, 0.0f, BASE_ROT_SPEED);
    private Vector3 baseSwing = new Vector3(BASE_ROT_SPEED, 0.0f, 0.0f);

    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////


    //register for mouse events and get the rigidbody so as to be able to respond to them
    public void Setup()
    {
        Services.Events.Register<KeyDirectionEvent>(MoveHands);
        Services.Events.Register<MouseEvent>(RotateHands);
        rb = GetComponent<Rigidbody>();
        newLoc = Input.mousePosition; //avoid a "jump" on the first frame of movement
    }


    //move hands based on player input
    private void MoveHands(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(KeyDirectionEvent), "Non-KeyDirectionEvent in MoveHands.");

        KeyDirectionEvent dirEvent = e as KeyDirectionEvent;

        //move hands
        InputManager.Directions dir = dirEvent.direction;

        switch (dir)
        {
            case InputManager.Directions.Up:
                rb.AddForce(Vector3.up * moveSpeed * Time.deltaTime, ForceMode.Force);
                break;
            case InputManager.Directions.Down:
                rb.AddForce(Vector3.down * moveSpeed * Time.deltaTime, ForceMode.Force);
                break;
            case InputManager.Directions.Left:
                rb.AddForce(Vector3.left * moveSpeed * Time.deltaTime, ForceMode.Force);
                break;
            case InputManager.Directions.Right:
                rb.AddForce(Vector3.right * moveSpeed * Time.deltaTime, ForceMode.Force);
                break;
            case InputManager.Directions.Diag_Up_Left:
                rb.AddForce(Vector3.up * moveSpeed * Time.deltaTime, ForceMode.Force);
                rb.AddForce(Vector3.left * moveSpeed * Time.deltaTime, ForceMode.Force);
                break;
            case InputManager.Directions.Diag_Up_Right:
                rb.AddForce(Vector3.up * moveSpeed * Time.deltaTime, ForceMode.Force);
                rb.AddForce(Vector3.right * moveSpeed * Time.deltaTime, ForceMode.Force);
                break;
            case InputManager.Directions.Diag_Down_Left:
                rb.AddForce(Vector3.down * moveSpeed * Time.deltaTime, ForceMode.Force);
                rb.AddForce(Vector3.left * moveSpeed * Time.deltaTime, ForceMode.Force);
                break;
            case InputManager.Directions.Diag_Down_Right:
                rb.AddForce(Vector3.down * moveSpeed * Time.deltaTime, ForceMode.Force);
                rb.AddForce(Vector3.right * moveSpeed * Time.deltaTime, ForceMode.Force);
                break;
        }
    }


    private void RotateHands(global::Event e)
    {
        Debug.Assert(e.GetType() == typeof(MouseEvent), "Non-MouseEvent in MoveHands.");

        MouseEvent mouseEvent = e as MouseEvent;

        //swing if both buttons are pressed
        if (mouseEvent.LMB && mouseEvent.RMB)
        {
            deltaRotation = Quaternion.Euler(baseSwing * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        //rotate hands if either LMB or RMB is pressed
        else if (mouseEvent.LMB || mouseEvent.RMB)
        {

            //set the amount of rotation for this frame
            if (mouseEvent.LMB) { deltaRotation = Quaternion.Euler(baseRotation * Time.deltaTime); } //rotate left for LMB
            else { deltaRotation = Quaternion.Euler(baseRotation * Time.deltaTime * -1.0f); } //rotate right for RMB

            //actually spin the hands
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
}



