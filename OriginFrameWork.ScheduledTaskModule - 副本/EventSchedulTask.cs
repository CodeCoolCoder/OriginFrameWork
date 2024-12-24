namespace OriginFrameWork.ScheduledTaskModule;

public abstract class EventSchedulTask
{
    public EventSchedulTask(int timehour, int timeminute)
    {
        this.timehour = timehour;
        this.timeminute = timeminute;
    }
    public int timehour { get; set; }
    public int timeminute { get; set; }
    public virtual void DesignatedSchedulTaskEvent(object obj)
    {
        DateTime now = DateTime.Now;
        TimerCallback taskactionss = new TimerCallback(DesignatedSchedulTaskEvent);
        // 计算时间差
        DateTime oneTenOClock = DateTime.Today.AddHours(timehour).AddMinutes(timeminute);
        if (now > oneTenOClock)
        {
            oneTenOClock = oneTenOClock.AddDays(1);
        }
        int msUntilOneTen = (int)((oneTenOClock - now).TotalMilliseconds);
        Timer timer = new Timer(taskactionss, null, msUntilOneTen, Timeout.Infinite);
        
    }

    public virtual void TimingOperation(Object state)
    {
    }
}
