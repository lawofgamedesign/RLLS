using UnityEngine;

public class InputManager
{
    /////////////////////////////////////////////
    /// Fields
    /////////////////////////////////////////////

    public enum Directions { Up, Down, Left, Right, Diag_Up_Left, Diag_Up_Right, Diag_Down_Left, Diag_Down_Right, None }
    protected bool upPressed;
    protected bool downPressed;
    protected bool leftPressed;
    protected bool rightPressed;
    protected Directions currentDir;


    protected Vector3 lastFramePos = new Vector3(0.0f, 0.0f);
    protected Vector3 thisFramePos = new Vector3(0.0f, 0.0f);


    //Keys used by the training manager, etc.
    public enum UsefulKeys { Space }




    /////////////////////////////////////////////
    /// Functions
    /////////////////////////////////////////////

    ///Each frame, check for keyboard and mouse inputs
    public virtual void Tick()
    {
        GetDirection();
        CheckBothMouseButtons();
        GetMouseState();
    }


    /// <summary>
    /// Sends an event if the player is pressing a direction key
    /// </summary>
    protected virtual void GetDirection()
    {
        //reset states
        currentDir = Directions.None;
        upPressed = false;
        downPressed = false;
        leftPressed = false;
        rightPressed = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) upPressed = true;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) downPressed = true;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) leftPressed = true;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) rightPressed = true;

        if (upPressed && leftPressed) currentDir = Directions.Diag_Up_Left;
        else if (upPressed && rightPressed) currentDir = Directions.Diag_Up_Right;
        else if (upPressed) currentDir = Directions.Up;
        else if (downPressed && leftPressed) currentDir = Directions.Diag_Down_Left;
        else if (downPressed && rightPressed) currentDir = Directions.Diag_Down_Right;
        else if (downPressed) currentDir = Directions.Down;
        else if (leftPressed) currentDir = Directions.Left;
        else if (rightPressed) currentDir = Directions.Right;

        //if the player in inputting a direction, send out an appropriate event
        if (currentDir != Directions.None) Services.Events.Fire(new KeyDirectionEvent(currentDir));
    }

    /// <summary>
    /// Sends an event with how the mouse has moved, and which buttons--if any--are pressed.
    /// </summary>
    protected virtual void GetMouseState()
    {
        lastFramePos = thisFramePos;

        thisFramePos = Input.mousePosition;

        Services.Events.Fire(new MouseEvent(thisFramePos, thisFramePos - lastFramePos, Input.GetMouseButton(0), Input.GetMouseButton(1)));
    }

    /// <summary>
    /// If both mouse buttons are pressed (because either one is detected as *just* pressed while the other *is* pressed, to allow for non-frame-perfect inputs), send out an appropriate event.
    /// </summary>
    protected void CheckBothMouseButtons()
    {
        if ((Input.GetMouseButtonDown(0) && Input.GetMouseButton(1)) || Input.GetMouseButton(0) && Input.GetMouseButtonDown(1)) Services.Events.Fire(new BothMouseButtonsEvent());
    }
}
