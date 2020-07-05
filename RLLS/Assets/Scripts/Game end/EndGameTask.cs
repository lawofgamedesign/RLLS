using UnityEngine;

public class EndGameTask :Task
{
    private const float ZERO = 0.0f;
    private const string RESET_MSG = "Enter restarts\nESC quits";

    public EndGameTask() { }

    protected override void Init()
    {
        Time.timeScale = ZERO;
        Services.UI.ChangeExplanatoryMessage(RESET_MSG);
    }
}
