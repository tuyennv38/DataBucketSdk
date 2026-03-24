using System;
using System.Collections.Generic;
using UnityEngine;

public static class SessionUtil
{
    private const string LastEventTimestampKey = "last_event_timestamp";
    private const string CurrentSessionStartKey = "current_session_start";
    private const string SessionNumberKey = "session_number";

    private static long sessionNumber = -1;

    private static readonly object prefsLock = new object();

    public static long GetSessionNumber()
    {
        try {
            if (sessionNumber == -1)
            {
                lock (prefsLock)
                {
                    string sessionNumberStr = PlayerPrefs.GetString(SessionNumberKey, "0");
                    return long.TryParse(sessionNumberStr, out long sessionNumberLong) ? sessionNumberLong : 0;
                }
            }
            return sessionNumber;
        } catch (Exception e) {
            Debug.LogError($"Error in GetSessionNumber: {e.Message}");
            return 0;
        }
    }

    public static void IncreaseSessionNumber()
    {
        try {
            lock (prefsLock)
            {
                if (sessionNumber == -1)
                {
                    string sessionNumberStr = PlayerPrefs.GetString(SessionNumberKey, "0");
                    sessionNumber = long.TryParse(sessionNumberStr, out long sessionNumberLong) ? sessionNumberLong : 0;
                }
                sessionNumber += 1;
                PlayerPrefs.SetString(SessionNumberKey, sessionNumber.ToString());
                PlayerPrefs.Save();
            }
        } catch (Exception e) {
            Debug.LogError($"Error in IncreaseSessionNumber: {e.Message}");
        }
    }

    public static long GetSessionTimeoutSeconds()
    {
        // Always return fixed session timeout of 1800 seconds (30 minutes)
        return 1800;
    }

    public static void SetSessionTimeoutSeconds(int seconds)
    {
        Debug.LogWarning("SetSessionTimeoutSeconds is disabled. Session timeout is fixed at 1800 seconds (30 minutes).");
    }

    public static long GetCurrentSessionStart()
    {   
        try {
            lock (prefsLock)
            {
                string startStr = PlayerPrefs.GetString(CurrentSessionStartKey, "-1");
                return long.TryParse(startStr, out long ts) ? ts : -1;
            }
        } catch (Exception e) {
            Debug.LogError($"Error in GetCurrentSessionStart: {e.Message}");
            return -1;
        }
    }

    public static void SetCurrentSessionStart(long ts)
    {
        try {
            lock (prefsLock)
            {
                PlayerPrefs.SetString(CurrentSessionStartKey, ts.ToString());
                PlayerPrefs.Save();
            }
        } catch (Exception e) {
            Debug.LogError($"Error in SetCurrentSessionStart: {e.Message}");
        }
    }

    public static long GetLastEventTimestamp()
    {
        try {
            lock (prefsLock)
            {
                if (!PlayerPrefs.HasKey(LastEventTimestampKey)) return 0;
                string lastTsStr = PlayerPrefs.GetString(LastEventTimestampKey);
                return long.TryParse(lastTsStr, out long ts) ? ts : 0;
            }
        } catch (Exception e) {
            Debug.LogError($"Error in GetLastEventTimestamp: {e.Message}");
            return 0;
        }
    }

    public static void SetLastEventTimestamp(long ts)
    {
        try {
            lock (prefsLock)
            {
                PlayerPrefs.SetString(LastEventTimestampKey, ts.ToString());
                PlayerPrefs.Save();
            }
        } catch (Exception e) {
            Debug.LogError($"Error in SetLastEventTimestamp: {e.Message}");
        }
    }

    public static string GetCurrentSessionId()
    {   
        try {
            lock (prefsLock)
            {
                return $"{StaticEventFieldsUtil.GetUserPseudoId()}:{GetCurrentSessionStart()}";
            }
        } catch (Exception e) {
            Debug.LogError($"Error in GetCurrentSessionId: {e.Message}");
            return "unknown:-1";
        }
    }

    public static void StartSession(long sessionStartTs)
    {
        try
        {
            // Prepare data inside lock (fast operations only)
            string sessionId;
            long currentSessionNumber;
            
            lock (prefsLock)
            {
                // Increase session number inline to avoid nested lock
                if (sessionNumber == -1)
                {
                    string sessionNumberStr = PlayerPrefs.GetString(SessionNumberKey, "0");
                    sessionNumber = long.TryParse(sessionNumberStr, out long sessionNumberLong) ? sessionNumberLong : 0;
                }
                sessionNumber += 1;
                PlayerPrefs.SetString(SessionNumberKey, sessionNumber.ToString());
                
                // Set session start
                PlayerPrefs.SetString(CurrentSessionStartKey, sessionStartTs.ToString());
                PlayerPrefs.Save();
                
                // Capture values to use outside lock
                currentSessionNumber = sessionNumber;
                sessionId = $"{StaticEventFieldsUtil.GetUserPseudoId()}:{sessionStartTs}";
            }
            
            // Network operation OUTSIDE lock to prevent ANR
            Dictionary<string, object> eventParams = new Dictionary<string, object>
            {
                ["session_id"] = sessionId,
                ["session_start_ts"] = sessionStartTs,
                ["session_number"] = currentSessionNumber
            };

            Databuckets.DatabucketsTracker.AutoRecord("session_start", sessionStartTs, eventParams);
            Debug.Log($"Session started: sessionId={sessionId}, sessionNumber={currentSessionNumber}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Start Session failed: {e}");
        }
    }

    public static bool IsSessionExpired(long currentTimestamp)
    {
        try {
            lock (prefsLock)
            {
                long lastEventTimestamp = GetLastEventTimestamp();
                long timeoutMs = GetSessionTimeoutSeconds() * 1000L;
                
                return PlayerPrefs.HasKey(LastEventTimestampKey) && (currentTimestamp - lastEventTimestamp) > timeoutMs;
            }
        } catch (Exception e) {
            Debug.LogError($"Error in IsSessionExpired: {e.Message}");
            return false;
        }
    }

    public static bool HasActiveSession()
    {
        try {
            lock (prefsLock)
            {
                return GetCurrentSessionStart() > 0;
            }
        } catch (Exception e) {
            Debug.LogError($"Error in HasActiveSession: {e.Message}");
            return false;
        }
    }



    public static void EndCurrentSession(string reason = "ApplicationExit")
    {
        try
        {
            long currentSessionStart = GetCurrentSessionStart();
            
            if (currentSessionStart > 0)
            {
                // Clear session without sending session_end event
                lock (prefsLock)
                {
                    PlayerPrefs.DeleteKey(CurrentSessionStartKey);
                    PlayerPrefs.Save();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"End Current Session failed: {e}");
        }
    }
}
