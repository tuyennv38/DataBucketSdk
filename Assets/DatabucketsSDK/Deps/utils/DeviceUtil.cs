using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class DeviceUtil
{
    public static Dictionary<string, object> GetDeviceInfo()
    {
        try {
            var info = new Dictionary<string, object>
            {
                ["model"] = SystemInfo.deviceModel,
                ["brand_name"] = SystemInfo.deviceName,
                ["platform_version"] = SystemInfo.operatingSystem,
                ["category"] = GetDeviceCategory(),
                ["timezone_offset_seconds"] = GetTimezoneOffsetSeconds(),
                ["user_default_language"] = GetUserDefaultLanguage()
            };

            return info;
        } catch (Exception e) {
            Debug.LogError($"Error in GetDeviceInfo: {e.Message}");
            return new Dictionary<string, object>
            {
                ["model"] = "unknown",
                ["brand_name"] = "unknown",
                ["platform_version"] = "unknown",
                ["category"] = "mobile",
                ["timezone_offset_seconds"] = 0,
                ["user_default_language"] = "en"
            };
        }
    }

    private static string GetDeviceCategory()
    {
        try
        {
            float dpi = Screen.dpi;
            if (dpi <= 0)
            {
                Debug.LogWarning("Screen.dpi is zero or negative, defaulting to mobile category");
                return "mobile";
            }
            float widthInches = Screen.width / dpi;
            float heightInches = Screen.height / dpi;
            double diagonalInches = Math.Sqrt(widthInches * widthInches + heightInches * heightInches);

            return diagonalInches >= 7.0 ? "tablet" : "mobile";
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error getting device category: {e.Message}");
            return "mobile";
        }
    }

    private static int GetTimezoneOffsetSeconds()
    {
        try {
            TimeSpan offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
            return (int)offset.TotalSeconds;
        } catch (Exception e) {
            Debug.LogWarning($"Error getting timezone offset: {e.Message}");
            return 0;
        }
    }

    private static string GetUserDefaultLanguage()
    {
        try
        {
            var culture = CultureInfo.CurrentCulture;
            string[] nameParts = culture.Name.Split('-');
            if (nameParts.Length > 1)
            {
                return $"{culture.TwoLetterISOLanguageName}-{nameParts[1]}";
            }
            else
            {
                return culture.TwoLetterISOLanguageName;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Error getting user default language: {e.Message}");
            return "en";
        }
    }
}
