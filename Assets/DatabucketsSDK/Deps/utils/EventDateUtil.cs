using System;
using System.Collections.Generic;
using System.Globalization;

public static class EventDateUtil
{
    private static readonly Dictionary<int, string> DayOfWeekMap = new Dictionary<int, string>
    {
        { 0, "Sunday" },
        { 1, "Monday" },
        { 2, "Tuesday" },
        { 3, "Wednesday" },
        { 4, "Thursday" },
        { 5, "Friday" },
        { 6, "Saturday" }
    };

    public static int GetUtcDate(long utcTimestampMillis)
    {
        try
        {
            var date = DateTimeOffset.FromUnixTimeMilliseconds(utcTimestampMillis).UtcDateTime;
            return int.Parse(date.ToString("yyyyMMdd"));
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Error getting UTC date: {e.Message}");
            return -1;
        }
    }

    public static string GetLocalDayOfWeek(long utcTimestampMillis, int timezoneOffsetSeconds)
    {
        try
        {
            var localTime = DateTimeOffset.FromUnixTimeMilliseconds(utcTimestampMillis)
                .ToOffset(TimeSpan.FromSeconds(timezoneOffsetSeconds));

            int dayIndex = (int)localTime.DayOfWeek; // Sunday = 0, Monday = 1, ..., Saturday = 6
            return $"{dayIndex} - {DayOfWeekMap[dayIndex]}";
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Error getting local day of week: {e.Message}");
            return "0 - Sunday";
        }
    }

    public static int GetLocalHour(long utcTimestampMillis, int timezoneOffsetSeconds)
    {
        try
        {
            var localTime = DateTimeOffset.FromUnixTimeMilliseconds(utcTimestampMillis)
                .ToOffset(TimeSpan.FromSeconds(timezoneOffsetSeconds));
            return localTime.Hour;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Error getting local hour: {e.Message}");
            return 0;
        }
    }

    public static int GetLocalHourMinute(long utcTimestampMillis, int timezoneOffsetSeconds)
    {
        try
        {
            var localTime = DateTimeOffset.FromUnixTimeMilliseconds(utcTimestampMillis)
                .ToOffset(TimeSpan.FromSeconds(timezoneOffsetSeconds));
            return int.Parse($"{localTime.Hour}{localTime.Minute:00}");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Error getting local hour minute: {e.Message}");
            return 0;
        }
    }

    public static int CalculateRetentionDate(int installDayUtc, long nowUtcMillis)
    {
        if (installDayUtc == -1) return -1;

        try
        {
            var installDate = DateTime.ParseExact(installDayUtc.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            var nowDate = DateTimeOffset.FromUnixTimeMilliseconds(nowUtcMillis).UtcDateTime;

            TimeSpan diff = nowDate - installDate;
            return (int)diff.TotalDays;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Error calculating retention date: {e.Message}");
            return -1;
        }
    }

    public static int CalculateRetentionHour(int installHourUtc, long nowUtcMillis)
    {
        if (installHourUtc == -1) return -1;

        try
        {
            var installDateTime = DateTime.ParseExact(installHourUtc.ToString(), "yyyyMMddHH", CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            var nowDateTime = DateTimeOffset.FromUnixTimeMilliseconds(nowUtcMillis).UtcDateTime;

            TimeSpan diff = nowDateTime - installDateTime;
            return (int)diff.TotalHours;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Error calculating retention hour: {e.Message}");
            return -1;
        }
    }
}
