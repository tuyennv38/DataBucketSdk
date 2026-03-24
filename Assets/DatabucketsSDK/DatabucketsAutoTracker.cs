using System;
using UnityEngine;
using DatabucketsSDK.Utils;
using System.Collections;
using System.Collections.Generic;

namespace Databuckets
{
    /// <summary>
    /// MonoBehaviour component to automatically track app focus events
    /// Automatically created when SDK is initialized
    /// </summary>
    public class DatabucketsAutoTracker : MonoBehaviour
    {
        public enum AppFocusEndReason
        {
            None,
            AppLostFocus,
            AppQuitting,
            Crash,
            Unknown
        } 
        private bool isAppInFocus = true;

        public void Initialize()
        {
            try
            {
                // Get current open time
                long openTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // Initialize session tracking
                InitializeSession();
                // Send app_focus_start event
                SendAppFocusStartEvent();
                // Check and send first_open event if needed
                try
                {
                    long installTimestamp = AppInfoUtil.GetInstallTimestamp();
                    bool isSentFirstOpen = PlayerPrefs.GetInt("is_sent_first_open", 0) == 1;

                    Debug.Log($"First Open Check: isSentFirstOpen={isSentFirstOpen}, installTimestamp={installTimestamp}, openTime={openTime}");
                    
                    // if havent sent and within 10 minutes from install time
                    if (!isSentFirstOpen && (openTime - installTimestamp) <= 600000 && installTimestamp > 0)
                    {

                        Databuckets.DatabucketsTracker.AutoRecord("first_open", openTime, null);
                    }
                    
                    PlayerPrefs.SetInt("is_sent_first_open", 1);
                    PlayerPrefs.Save();
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Failed Sending first_open: " + e.Message);
                }

                // Ensure this object is not destroyed when loading new scenes
                DontDestroyOnLoad(gameObject);

                // Register event handlers
                RegisterEventHandlers();

                Debug.Log("DatabucketsAutoTracker: Initialized app focus tracking and session management");
            }
            catch (Exception e)
            {
                Debug.LogError($"DatabucketsAutoTracker Initialize failed: {e.Message}");
            }
        }

        private void RegisterEventHandlers()
        {
            try
            {
                // Track when app is quitting
                Application.quitting += OnApplicationQuitting;

                // Track when there's an unhandled exception
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            }
            catch (Exception e)
            {
                Debug.LogError($"RegisterEventHandlers failed: {e.Message}");
            }
        }

        private bool isExceptionLogTrackingEnabled = false;

        /// <summary>
        /// Enable tracking of Unity exception/error logs.
        /// Call this method to start receiving app_exception_log events.
        /// </summary>
        public void EnableExceptionLogTracking()
        {
            try
            {
                if (!isExceptionLogTrackingEnabled)
                {
                    Application.logMessageReceived += OnLogMessageReceived;
                    isExceptionLogTrackingEnabled = true;
                    Debug.Log("DatabucketsAutoTracker: Exception log tracking enabled");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"EnableExceptionLogTracking failed: {e.Message}");
            }
        }

        /// <summary>
        /// Disable tracking of Unity exception/error logs.
        /// </summary>
        public void DisableExceptionLogTracking()
        {
            try
            {
                if (isExceptionLogTrackingEnabled)
                {
                    Application.logMessageReceived -= OnLogMessageReceived;
                    isExceptionLogTrackingEnabled = false;
                    Debug.Log("DatabucketsAutoTracker: Exception log tracking disabled");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"DisableExceptionLogTracking failed: {e.Message}");
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            try
            {
                if (pauseStatus)
                {
                    // App is paused/minimized
                    OnAppLostFocus();
                }
                else
                {
                    // App is resumed
                    OnAppGainedFocus();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"OnApplicationPause failed: {e.Message}");
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            try
            {
                if (hasFocus)
                {
                    OnAppGainedFocus();
                }
                else
                {
                    OnAppLostFocus();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"OnApplicationFocus failed: {e.Message}");
            }
        }

        private void OnAppLostFocus()
        {
            try
            {
                if (isAppInFocus)
                {
                    isAppInFocus = false;
                    // Send app_focus_end event immediately when app is paused or backgrounded
                    SendAppFocusEndEvent(AppFocusEndReason.AppLostFocus);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"OnAppLostFocus failed: {e.Message}");
            }
        }

        private void OnAppGainedFocus()
        {
            try
            {
                if (!isAppInFocus)
                {
                    isAppInFocus = true;
                    // Send app_focus_start event
                    SendAppFocusStartEvent();
                    CheckAndRestartSessionIfNeeded();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"OnAppGainedFocus failed: {e.Message}");
            }
        }

        private void OnApplicationQuitting()
        {
            try
            {
                // Clear session when app is quitting (no session_end event)
                SessionUtil.EndCurrentSession("AppQuitting");
            }
            catch (Exception e)
            {
                Debug.LogError($"OnApplicationQuitting failed: {e.Message}");
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                // Send app_focus_end event with crash reason
                SendAppFocusEndEvent(AppFocusEndReason.Crash);
                SessionUtil.EndCurrentSession("AppCrash");
            }
            catch (Exception ex)
            {
                Debug.LogError($"OnUnhandledException failed: {ex.Message}");
            }
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            try
            {
                if (type == LogType.Exception)
                {
                    Dictionary<string, object> eventParams = new Dictionary<string, object>
                    {
                        ["log_condition"] = condition,
                    };
                    long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    DatabucketsTracker.AutoRecord("app_exception_log", currentTime, eventParams);
                    Debug.Log($"DatabucketsAutoTracker: Sent app_exception_log event");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"OnLogMessageReceived failed: {e.Message}");
            }
        }

        private void SendAppFocusEndEvent(AppFocusEndReason reason)
        {
            try
            {
                long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                Dictionary<string, object> eventParams = new Dictionary<string, object>
                {
                    ["app_focus_end_reason"] = reason.ToString(),
                };

                DatabucketsTracker.AutoRecord("app_focus_end", currentTime, eventParams);
                Debug.Log($"DatabucketsAutoTracker: Sent app_focus_end event with reason {reason}");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to send app_focus_end event: {e.Message}");
            }
        }

        private void SendAppFocusStartEvent()
        {
            try
            {
                long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                DatabucketsTracker.AutoRecord("app_focus_start", currentTime, null);
                Debug.Log("DatabucketsAutoTracker: Sent app_focus_start event");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to send app_focus_start event: {e.Message}");
            }
        }

        private void OnDestroy()
        {
            try
            {
                // Unregister event handlers to avoid memory leaks
                Application.quitting -= OnApplicationQuitting;
                AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
                if (isExceptionLogTrackingEnabled)
                {
                    Application.logMessageReceived -= OnLogMessageReceived;
                }
                if (SessionUtil.HasActiveSession())
                {
                    SessionUtil.EndCurrentSession("AppClosed");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"OnDestroy failed: {e.Message}");
            }
        }

        #region Session Management

        private void InitializeSession()
        {
            try
            {
                // Check if there's an existing session and if it's expired
                if (!SessionUtil.HasActiveSession())
                {
                    // No active session, start new one
                    Debug.Log("DatabucketsAutoTracker: No active session, starting new session");
                }
                else
                {
                    SessionUtil.EndCurrentSession("AppRestart");
                    // Stop current session and start a new one
                    Debug.Log("DatabucketsAutoTracker: Restarting session when app initializes");
                }
                long sessionStartTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                SessionUtil.StartSession(sessionStartTs);
            }
            catch (Exception e)
            {
                Debug.LogError($"InitializeSession failed: {e.Message}");
            }
        }

        private void CheckAndRestartSessionIfNeeded()
        {   
            try
            {
                long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                // Check if there's an existing session and if it's expired
                if (!SessionUtil.HasActiveSession())
                {
                    // No active session, start new one
                    Debug.Log("DatabucketsAutoTracker: No active session, starting new session");
                    SessionUtil.StartSession(currentTime);
                }
                else if (SessionUtil.IsSessionExpired(currentTime))
                {
                    // Existing session is expired, end it and start new one
                    Debug.Log("DatabucketsAutoTracker: Existing session expired, ending and starting new session");
                    SessionUtil.EndCurrentSession("SessionTimeout");
                    long sessionStartTs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    SessionUtil.StartSession(sessionStartTs);
                }
                else
                {
                    // Has active session and not expired
                    Debug.Log("DatabucketsAutoTracker: Continuing existing session");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"CheckAndRestartSessionIfNeeded failed: {e.Message}");
            }
        }

        #endregion
    }
}
