using System.Security.Cryptography.X509Certificates;

namespace OriginFrameWork.ScheduledTaskModule;
public class OriginScheduledTaskHandler : IOriginScheduledTaskHandler
{
    public void ScheduledTaskStart(int timehour, int timeminute, ScheduledTaskType scheduledTaskType, EventSchedulTask eventSchedulTask)
    {
        //定点执行任务
        if (scheduledTaskType == ScheduledTaskType.Designated)
        {
            DateTime now = DateTime.Now;
            TimerCallback taskcallback = new TimerCallback(eventSchedulTask.DesignatedSchedulTaskEvent);
            // 计算时间差
            DateTime oneTenOClock = DateTime.Today.AddHours(timehour).AddMinutes(timeminute);
            if (now > oneTenOClock)
            {
                oneTenOClock = oneTenOClock.AddDays(1);
            }
            int msUntilOneTen = (int)((oneTenOClock - now).TotalMilliseconds);
            Timer timer = new Timer(taskcallback, null, msUntilOneTen, Timeout.Infinite);
        }
        else
        {
            //循环执行任务
            Timer timer = new Timer(eventSchedulTask.TimingOperation, null, timehour, timeminute);
        }
    }

    public void ScheduledTaskEnd(Timer timer)
    {
        //结束任务
        // timer.Dispose();
    }

}
