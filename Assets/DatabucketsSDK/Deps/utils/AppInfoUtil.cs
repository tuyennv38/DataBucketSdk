using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Runtime.InteropServices;

public static class AppInfoUtil
{
    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern long _GetInstallTimestampV2();
    #endif

    public static string GetNormalizedPlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "android";
            
            case RuntimePlatform.IPhonePlayer:
                return "ios";
            
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "windows";
            
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                return "macos";
            
            case RuntimePlatform.WebGLPlayer:
                return "web";
            
            default:
                return "unknown";
        }
    }

    public static Dictionary<string, object> GetAppInfo()
    {
        try {
            var info = new Dictionary<string, object>
            {
                ["app_id"] = Application.identifier,
                ["app_version"] = Application.version,
                ["app_platform"] = GetNormalizedPlatform(),
                ["app_name"] = Application.productName,
                ["build_number"] = Application.buildGUID
            };

        #if UNITY_ANDROID && !UNITY_EDITOR
            info["app_store"] = GetAndroidInstallerPackageName();
        #elif UNITY_IOS && !UNITY_EDITOR
            info["app_store"] = "apple_app_store";
        #else
            info["app_store"] = "editor_or_unknown";
        #endif

            return info;
        } catch (Exception e) {
            Debug.LogError($"Error in GetAppInfo: {e.Message}");
            return new Dictionary<string, object> {
                ["app_id"] = "error",
                ["app_version"] = "error",
                ["app_platform"] = "error",
                ["app_name"] = "error",
                ["build_number"] = "error",
                ["app_store"] = "error"
            };
        }
    }

    public static void AddInstallAndRetentionInfo(Dictionary<string, object> info)
    {
        try
        {
            long installTimestamp = GetInstallTimestamp();
            long nowTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (installTimestamp <= 0)
            {
                info["retention_error"] = $"Negative ts: {installTimestamp}";
                info["install_day"] = -1;
                SetFallbackRetentionInfo(info);
                return;
            }

            // installTimestamp is expected to be "milliseconds since Unix epoch (UTC)"
            var installDateOffset = DateTimeOffset.FromUnixTimeMilliseconds(installTimestamp);
            var nowDateOffset = DateTimeOffset.FromUnixTimeMilliseconds(nowTimestamp);

            info["install_day"] = long.Parse(
                installDateOffset.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
                CultureInfo.InvariantCulture
            );

            // Calculate diff using TimeSpan
            TimeSpan diff = nowDateOffset - installDateOffset;

            // if device clock is skewed backwards (negative diff), treat as invalid
            if (diff.TotalSeconds < 0)
            {
                info["retention_error"] = $"Negative diff. Install ts: {installTimestamp}, now ts: {nowTimestamp}";
                SetFallbackRetentionInfo(info);
                return;
            }

            long retentionMinute = (long)diff.TotalMinutes;
            long retentionHour = (long)diff.TotalHours;
            long retentionDay = (long)diff.TotalDays;

            info["retention_minute"] = retentionMinute;
            info["retention_hour"] = retentionHour;
            info["retention_day"] = retentionDay;
        }
        catch (Exception e)
        {
            info["retention_error"] = e.Message;
            info["install_day"] = -1;
            SetFallbackRetentionInfo(info);
        }
    }


    private static void SetFallbackRetentionInfo(Dictionary<string, object> info)
    {
        info["retention_day"] = -1;
        info["retention_hour"] = -1;
        info["retention_minute"] = -1;
    }

    public static long GetInstallTimestamp()
    {
        const string key = "install_timestamp";

        try
        {
            if (PlayerPrefs.HasKey(key))
            {
                string saved = PlayerPrefs.GetString(key, "-1");
                if (long.TryParse(saved, out long cachedTs) && cachedTs > 0)
                    return cachedTs;
            }

            long ts = -1;

        #if UNITY_ANDROID && !UNITY_EDITOR
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var packageManager = context.Call<AndroidJavaObject>("getPackageManager"))
            using (var packageInfo = packageManager.Call<AndroidJavaObject>(
                "getPackageInfo", context.Call<string>("getPackageName"), 0))
            {
                long firstInstallTime = packageInfo.Get<long>("firstInstallTime"); // ms since epoch
                ts = firstInstallTime;
            }
        #elif UNITY_IOS && !UNITY_EDITOR
            ts = _GetInstallTimestampV2(); // ms since epoch
        #else
            ts = -1;
        #endif

            if (ts > 0)
            {
                PlayerPrefs.SetString(key, ts.ToString());
                PlayerPrefs.Save();
            }

            return ts;
        }
        catch
        {
            return -1;
        }
    }

    #if UNITY_ANDROID && !UNITY_EDITOR
    private static string GetAndroidInstallerPackageName()
    {
        try
        {
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var pm = context.Call<AndroidJavaObject>("getPackageManager"))
            {
                string packageName = context.Call<string>("getPackageName");
                string installerPackageName = pm.Call<string>("getInstallerPackageName", packageName);
                return installerPackageName ?? "manual_install";
            }
        }
        catch
        {
            return "unknown";
        }
    }
    #endif
}
