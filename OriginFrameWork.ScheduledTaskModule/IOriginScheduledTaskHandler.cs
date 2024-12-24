namespace OriginFrameWork.ScheduledTaskModule;

public interface IOriginScheduledTaskHandler
{
    void ScheduledTaskStart(int timehour, int timeminute, ScheduledTaskType scheduledTaskType, EventSchedulTask eventSchedulTask);
}
