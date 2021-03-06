﻿using UnityEngine;

public class TrainingInputManager : InputManager
{
    

    /// <summary>
    /// In addition to normal input functions, this special InputManager listens for the space bar, which instructs the training dummy to attack.
    /// </summary>
    public override void Tick()
    {
        base.Tick();
        if (Input.GetKeyDown(KeyCode.Space)) Services.Events.Fire(new KeypressEvent(InputManager.UsefulKeys.Space));
    }

}
