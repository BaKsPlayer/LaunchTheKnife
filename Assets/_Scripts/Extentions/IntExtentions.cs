using System;

public static class IntExtentions
{
    public static TimeSpan ToTimeSpan(this int timeInSeconds)
    {
        return TimeSpan.FromSeconds(timeInSeconds);
    }

    public static TimeSpan ToTimeSpan(this SafeInt timeInSeconds)
    {
        return TimeSpan.FromSeconds(timeInSeconds);
    }
}
