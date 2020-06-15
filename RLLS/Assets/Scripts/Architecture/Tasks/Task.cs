public class Task
{
    public enum TaskStatus : byte { Detached, Pending, Working, Success, Fail, Aborted }

    public TaskStatus Status { get; private set; }

    public bool IsDetached { get { return Status == TaskStatus.Detached; } } //"Detached" here means that the task is not in the list of tasks TaskManager will run each frame
    public bool IsAttached { get { return Status != TaskStatus.Detached; } } //"Attached" means included in TaskManager's list
    public bool IsPending { get { return Status == TaskStatus.Pending; } }
    public bool IsWorking { get { return Status == TaskStatus.Working; } }
    public bool IsSuccessful { get { return Status == TaskStatus.Success; } }
    public bool IsFailed { get { return Status == TaskStatus.Fail; } }
    public bool IsAborted { get { return Status == TaskStatus.Aborted; } }
    public bool IsFinished { get { return Status == TaskStatus.Success || Status == TaskStatus.Fail || Status == TaskStatus.Aborted; } }

    internal void SetStatus(TaskStatus newStatus)
    {
        if (Status == newStatus) return;

        switch (newStatus)
        {
            case TaskStatus.Working:
                Init();
                break;
            case TaskStatus.Success:
                OnSuccess();
                Cleanup();
                break;
            case TaskStatus.Fail:
                OnFail();
                Cleanup();
                break;
            case TaskStatus.Detached:
            case TaskStatus.Pending:
                break;
            default:
                break;
        }
    }


    protected virtual void Init() { } //called when task begins working
    public virtual void Tick() { }
    protected virtual void Cleanup() { } //always called when a task completes, whether through success, failure, or abort

    protected virtual void OnSuccess() { } //called when task ends through setting Succeed status
    protected virtual void OnFail() { } //called when task ends through setting Fail status
    protected virtual void OnAbort() { } //called when task ends through setting Abort status


    public Task NextTask { get; private set; }


    public Task Then(Task task)
    {
        NextTask = task;
        return task;
    }
}
