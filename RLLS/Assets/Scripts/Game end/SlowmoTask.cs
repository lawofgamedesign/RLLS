using UnityEngine;

public class SlowmoTask : Task
{
    private const float SLOWMO_SPEED = 0.1f;
    private const float SLOWMO_DURATION = 1.0f;
    private float timer = 0.0f;

    public SlowmoTask() { }


    protected override void Init()
    {
        Time.timeScale = SLOWMO_SPEED;
    }

    public override void Tick()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= SLOWMO_DURATION) SetStatus(TaskStatus.Success);
    }
}
