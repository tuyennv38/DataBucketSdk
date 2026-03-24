using ElRaccoone.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using DatabucketsSDK.Utils;

namespace Databuckets
{

    public class DatabucketsTracker {
        private const string SDK_VERSION = "1.0.6";
        
        private class Tracker : DatabucketsTrackerInternal {
            private const string geoInfoKey = "GeoInfo";
            public Dictionary<string, object> geoInfo;
            private string baseUrl;
            private string apiKey;
            public enum ReservedEventKeys {
                appid, platform
            }

            public Tracker(string baseUrl, string apiKey) : base(10) {
                this.baseUrl = baseUrl;
                this.apiKey = apiKey;
                LoadCachedGeoInfo();
            }

            private void LoadCachedGeoInfo() {
                try {
                    string storedJson = PlayerPrefs.GetString(geoInfoKey, null);
                    if (!string.IsNullOrEmpty(storedJson)) {
                        geoInfo = Json.Deserialize(storedJson) as Dictionary<string, object>;
                    }
                } catch (Exception e) {
                    Debug.LogWarning($"Error loading cached geo info: {e.Message}");
                }
            }

            internal IEnumerator GetCountry(Action<bool> onComplete = null) {
                bool success = false;
                using (UnityWebRequest webRequest = UnityWebRequest.Get("https://ipinfo.io/json")) {
                    webRequest.timeout = 5;
                    
                    yield return webRequest.SendWebRequest();
                    try {
                        if (webRequest.result == UnityWebRequest.Result.Success) {
                            string json = webRequest.downloadHandler.text;
                            var dict = Json.Deserialize(json) as Dictionary<string, object>;

                            if (dict != null) {
                                geoInfo = dict;
                                success = true;
                                try {
                                    PlayerPrefs.SetString(geoInfoKey, Json.Serialize(dict));
                                    PlayerPrefs.Save();
                                } catch (Exception saveEx) {
                                    Debug.LogWarning($"Error saving geo info: {saveEx.Message}");
                                }
                            }
                        } else {
                            Debug.Log($"Cannot get country: {webRequest.error}");
                        }
                    } catch (Exception e) {
                        Debug.LogWarning($"Error in GetCountry: {e.Message}");
                    }
                }
                
                // Call callback after completion with success status
                try {
                    onComplete?.Invoke(success);
                } catch (Exception e) {
                    Debug.LogError($"Error in GetCountry callback: {e.Message}");
                }
            }

            protected override IEnumerator Send(string payload, long ts) {
                UnityWebRequest req = null;
                try {
                    req = new UnityWebRequest(baseUrl, "POST");
                    
                    req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(payload));
                    req.downloadHandler = new DownloadHandlerBuffer();
                    req.SetRequestHeader("Content-Type", "application/json");
                    req.SetRequestHeader("X-API-KEY", apiKey);
                    req.SetRequestHeader("X-TIMESTAMP-MILLIS", ts.ToString());
                } catch (Exception e) {
                    Debug.LogError($"Error creating request in Send: {e.Message}");
                    sentOk = false;
                    req?.Dispose();
                    yield break;
                }

                yield return req.SendWebRequest();

                try {
                    if (req.result != UnityWebRequest.Result.Success) {
                        Debug.Log("Sent events failed: " + req.error);
                        sentOk = false;
                    } else {
                        Debug.Log("Sent events successfully");
                        sentOk = true;
                    }
                } catch (Exception e) {
                    Debug.LogError($"Error processing response in Send: {e.Message}");
                    sentOk = false;
                } finally {
                    req?.Dispose();
                }
            }
        }

        private static DatabucketsTracker instance = null;
        private Tracker tracker = null;
        private static bool isUserIdReady = false;
        private static bool isAutoTrackerReady = false;
        private static bool isGeoInfoReady = false;
        private static Queue<Action> pendingActions = new Queue<Action>();
        private static List<string> pendingEventNames = new List<string>();
        private static DatabucketsAutoTracker autoTracker = null;
        private static readonly int[] retryDelays = { 2000, 5000, 10000, 20000, 50000 };

        private DatabucketsTracker(string baseUrl, string apiKey) {
            tracker = new Tracker(baseUrl, apiKey);
        }

        public static void Init(string baseUrl, string apiKey) {
            try {
                long initTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                if (instance == null) {
                    instance = new DatabucketsTracker(baseUrl, apiKey);
                    instance.tracker.SetCommonProperty(Tracker.ReservedEventKeys.appid.ToString(), Application.identifier);
                    instance.tracker.SetCommonProperty(Tracker.ReservedEventKeys.platform.ToString(), AppInfoUtil.GetNormalizedPlatform());
                    
                    // Fetch geo info after initialization with retry mechanism
                    // Runs in background without affecting UI or other logic
                    Action<int> fetchGeoInfoWithRetry = null;
                    fetchGeoInfoWithRetry = (attemptNumber) => {
                        try {
                            Timers.StartCoroutine(instance.tracker.GetCountry((success) => {
                                if (success) {
                                    // Success - set geo info to common properties
                                    SetGeoInfoToCommonProperties();
                                    isGeoInfoReady = true;
                                } else {
                                    // Failed - check if we should retry
                                    if (attemptNumber < retryDelays.Length) {
                                        int delay = retryDelays[attemptNumber];
                                        Debug.Log($"DatabucketsTracker: Geo info fetch failed, retrying in {delay}ms (attempt {attemptNumber + 1}/{retryDelays.Length})...");
                                        
                                        // Schedule retry with delay
                                        Timers.SetTimeout(delay, () => {
                                            fetchGeoInfoWithRetry(attemptNumber + 1);
                                        });
                                    } else {
                                        Debug.LogWarning($"DatabucketsTracker: Geo info fetch failed after {retryDelays.Length} attempts, giving up");
                                    }
                                }
                            }));
                        } catch (Exception e) {
                            Debug.LogError($"Error in fetchGeoInfoWithRetry: {e.Message}");
                        }
                    };
                    
                    // Start initial fetch (attempt 0)
                    fetchGeoInfoWithRetry(0);
                    
                    Timers.SetInterval(instance.tracker.getFlushInterval() * 1000, () => {
                        try {
                            Timers.StartCoroutine(instance.tracker.Flush());
                        } catch (Exception e) {
                            Debug.LogError($"Error in Flush timer: {e.Message}");
                        }
                    });
                }

                CreateUserPseudoId(() =>
                {
                    try {
                        TryInitCommonProperties(initTime);
                        
                        InitializeAutoTracker();
                    } catch (Exception e) {
                        Debug.LogError($"Error in CreateUserPseudoId callback: {e.Message}");
                    }
                });
            } catch (Exception e) {
                Debug.LogError($"Error in Init: {e.Message}");
            }
        }

        private static void InitializeAutoTracker()
        {
            try {
                if (autoTracker == null)
                {
                    // Create GameObject to contain DatabucketsAutoTracker
                    GameObject autoTrackerObject = new GameObject("DatabucketsAutoTracker");
                    autoTracker = autoTrackerObject.AddComponent<DatabucketsAutoTracker>();
                    autoTracker.Initialize();
                }
            } catch (Exception e) {
                Debug.LogError($"Error initializing auto tracker: {e.Message}");
            } finally {
                isAutoTrackerReady = true;
                Debug.Log("DatabucketsTracker: AutoTracker initialization completed");
                // Process pending actions after autotracker is ready
                ProcessPendingActions();
            }
        }

        private static void SetGeoInfoToCommonProperties() {
            try {
                Dictionary<string, object> geoInfo = instance.tracker.geoInfo;
                HashSet<string> excludeKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "readme", "ip", "hostname" };
                if (geoInfo != null)
                {
                    Dictionary<string, object> geo = new Dictionary<string, object>();
                    foreach (KeyValuePair<string, object> kv in geoInfo)
                    {
                        if (!excludeKeys.Contains(kv.Key))
                        {
                            geo[kv.Key] = kv.Value;
                        }
                    }

                    instance.tracker.SetCommonProperty("geo", geo);
                    Debug.Log("DatabucketsTracker: Geo info successfully set");
                }
            } catch (Exception e) {
                Debug.LogWarning($"Error processing geo info: {e.Message}");
            }
        }

        // Only execute when user pseudo ID is ready
        private static void TryInitCommonProperties(long initTime) {
            try {
                Dictionary<string, object> properties = new Dictionary<string, object>();

                // Device Info Section
                try {
                    Dictionary<string, object> deviceInfo = DeviceUtil.GetDeviceInfo();
                    properties["device"] = deviceInfo;
                } catch (Exception e) {
                    Debug.LogWarning($"Error getting device info: {e.Message}");
                }

                // App Info Section
                try {
                    Dictionary<string, object> appInfo = AppInfoUtil.GetAppInfo();
                    foreach (var kv in appInfo)
                    {
                        properties[kv.Key] = kv.Value;
                    }
                } catch (Exception e) {
                    Debug.LogWarning($"Error getting app info: {e.Message}");
                }

                // Set static field values
                properties["user_pseudo_id"] = StaticEventFieldsUtil.GetUserPseudoId();
                properties["sdk_ver"] = SDK_VERSION; // Add SDK version

                instance.tracker.SetCommonProperties(properties);
            } catch (Exception e) {
                Debug.LogError($"Error in TryInitCommonProperties: {e.Message}");
            }
        }

        private static void ProcessPendingActions()
        {
            try {
                // Only process pending actions when both user ID and autotracker are ready
                if (!isUserIdReady || !isAutoTrackerReady) return;

                int pendingCount = pendingActions.Count;
                if (pendingCount > 0)
                {
                    Debug.Log($"DatabucketsTracker: Processing {pendingCount} pending events...");
                }
                
                while (pendingActions.Count > 0)
                {
                    try {
                        var action = pendingActions.Dequeue();
                        var eventName = pendingEventNames.Count > 0 ? pendingEventNames[0] : "unknown";
                        if (pendingEventNames.Count > 0) pendingEventNames.RemoveAt(0);
                        
                        action?.Invoke();
                        Debug.Log($"DatabucketsTracker: Sent previously queued event: {eventName}");
                    } catch (Exception e) {
                        Debug.LogError($"Error processing pending action: {e.Message}");
                    }
                }
            } catch (Exception e) {
                Debug.LogError($"Error in ProcessPendingActions: {e.Message}");
            }
        }

        public static void SetCommonProperties(Dictionary<string, object> commonProperties) {
            try {
                void action() {
                    try {
                        commonProperties = commonProperties ?? new Dictionary<string, object>() { };
                        string userPseudoId = StaticEventFieldsUtil.GetUserPseudoId();
                        if (!string.IsNullOrEmpty(userPseudoId))
                            commonProperties["user_pseudo_id"] = userPseudoId;
                        commonProperties["sdk_ver"] = SDK_VERSION; // Make sure sdk_ver is always present
                        instance.tracker.SetCommonProperties(commonProperties);
                    } catch (Exception e) {
                        Debug.LogError($"Error in SetCommonProperties action: {e.Message}");
                    }
                }
                if (isUserIdReady && isAutoTrackerReady) action();
                else {
                    pendingActions.Enqueue(action);
                    pendingEventNames.Add("SetCommonProperties");
                }
            } catch (Exception e) {
                Debug.LogError($"Error in SetCommonProperties: {e.Message}");
            }
        }

        public static void SetCommonProperty(string key, object value) {
            try {
                void action() {
                    try {
                        if (key == "user_pseudo_id" && !isUserIdReady) return;
                        if (key == "sdk_ver") return; // Do not allow setting sdk_ver directly
                        instance.tracker.SetCommonProperty(key, value);
                    } catch (Exception e) {
                        Debug.LogError($"Error in SetCommonProperty action: {e.Message}");
                    }
                }
                if (isUserIdReady && isAutoTrackerReady) action();
                else {
                    pendingActions.Enqueue(action);
                    pendingEventNames.Add($"SetCommonProperty({key})");
                }
            } catch (Exception e) {
                Debug.LogError($"Error in SetCommonProperty: {e.Message}");
            }
        }

        public static void Record(string eventName, Dictionary<string, object> eventParams) {
            try {
                void action() {
                    try {
                        eventParams = eventParams ?? new Dictionary<string, object>() { };
                        string userPseudoId = StaticEventFieldsUtil.GetUserPseudoId();
                        if (!string.IsNullOrEmpty(userPseudoId))
                            eventParams["user_pseudo_id"] = userPseudoId;
                        eventParams["sdk_ver"] = SDK_VERSION; // Add sdk_ver to every event
                        instance.tracker.Record(eventName, eventParams);
                    } catch (Exception e) {
                        Debug.LogError($"Error in Record action: {e.Message}");
                    }
                }
                if (isUserIdReady && isAutoTrackerReady) action();
                else {
                    pendingActions.Enqueue(action);
                    pendingEventNames.Add(eventName);
                }
            } catch (Exception e) {
                Debug.LogError($"Error in Record: {e.Message}");
            }
        }

        internal static void AutoRecord(string eventName, long eventTs, Dictionary<string, object> eventParams)
        {
            try {
                void action() {
                    try {
                        eventParams = eventParams ?? new Dictionary<string, object>() { };
                        string userPseudoId = StaticEventFieldsUtil.GetUserPseudoId();
                        if (!string.IsNullOrEmpty(userPseudoId))
                            eventParams["user_pseudo_id"] = userPseudoId;
                        eventParams["sdk_ver"] = SDK_VERSION; // Add sdk_ver to every auto-recorded event
                        if (eventName == "session_start" && !isGeoInfoReady)
                        {
                            // Wait 3 seconds before recording session_start to allow geo info to load
                            Timers.SetTimeout(3000, () => {
                                try {
                                    instance.tracker.AutoRecord(eventName, eventTs, eventParams);
                                } catch (Exception e) {
                                    Debug.LogError($"Error in delayed AutoRecord for session_start: {e.Message}");
                                }
                            });
                        }
                        else
                        {
                            instance.tracker.AutoRecord(eventName, eventTs, eventParams);
                        }
                    } catch (Exception e) {
                        Debug.LogError($"Error in AutoRecord action: {e.Message}");
                    }
                }
                if (isUserIdReady && isAutoTrackerReady) action();
                else {
                    pendingActions.Enqueue(action);
                    pendingEventNames.Add(eventName);
                }
            } catch (Exception e) {
                Debug.LogError($"Error in AutoRecord: {e.Message}");
            }
        }

        public static void RecordWithTiming(
            string eventName, Dictionary<string, object> eventParams,
            string timingProp, string startEvent)
        {
            try {
                void action() {
                    try {
                        eventParams = eventParams ?? new Dictionary<string, object>() { };
                        string userPseudoId = StaticEventFieldsUtil.GetUserPseudoId();
                        if (!string.IsNullOrEmpty(userPseudoId))
                            eventParams["user_pseudo_id"] = userPseudoId;
                        eventParams["sdk_ver"] = SDK_VERSION; // Add sdk_ver to timing events
                        instance.tracker.RecordWithTiming(eventName, eventParams, timingProp, startEvent);
                    } catch (Exception e) {
                        Debug.LogError($"Error in RecordWithTiming action: {e.Message}");
                    }
                }
                if (isUserIdReady && isAutoTrackerReady) action();
                else {
                    pendingActions.Enqueue(action);
                    pendingEventNames.Add(eventName);
                }
            } catch (Exception e) {
                Debug.LogError($"Error in RecordWithTiming: {e.Message}");
            }
        }

        public static void ForceEndCurrentSession()
        {
            try {
                SessionUtil.EndCurrentSession("ManualEnd");
                SessionUtil.StartSession(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            } catch (Exception e) {
                Debug.LogError($"Error in ForceEndCurrentSession: {e.Message}");
            }
        }

        /// <summary>
        /// Enable tracking of Unity exception/error logs.
        /// Call this method to start receiving app_exception_log events.
        /// </summary>
        public static void EnableExceptionLogTracking()
        {
            try {
                if (autoTracker != null)
                {
                    autoTracker.EnableExceptionLogTracking();
                }
                else
                {
                    Debug.LogWarning("DatabucketsTracker: AutoTracker not initialized yet. Call Init() first.");
                }
            } catch (Exception e) {
                Debug.LogError($"Error in EnableExceptionLogTracking: {e.Message}");
            }
        }

        /// <summary>
        /// Disable tracking of Unity exception/error logs.
        /// </summary>
        public static void DisableExceptionLogTracking()
        {
            try {
                if (autoTracker != null)
                {
                    autoTracker.DisableExceptionLogTracking();
                }
                else
                {
                    Debug.LogWarning("DatabucketsTracker: AutoTracker not initialized yet. Call Init() first.");
                }
            } catch (Exception e) {
                Debug.LogError($"Error in DisableExceptionLogTracking: {e.Message}");
            }
        }

        private static void CreateUserPseudoId(Action onReady = null)
        {
            try {
                string cachedId = PlayerPrefs.GetString("user_pseudo_id", null);
                if (!string.IsNullOrEmpty(cachedId))
                {
                    isUserIdReady = true;
                    try
                    {
                        onReady?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error in CreateUserPseudoId callback: {e.Message}");
                    }
                    return;
                }
            } catch (Exception e) {
                Debug.LogWarning($"Error reading cached user_pseudo_id: {e.Message}");
            }

            try {
                string newId = "db-" + Guid.NewGuid().ToString();
                Debug.Log("Generated user_pseudo_id: " + newId);

                try {
                    PlayerPrefs.SetString("user_pseudo_id", newId);
                    PlayerPrefs.Save();
                } catch (Exception saveEx) {
                    Debug.LogWarning($"Error saving user_pseudo_id: {saveEx.Message}");
                }
                
                StaticEventFieldsUtil.SetUserPseudoId(newId);
                isUserIdReady = true;
                
                try
                {
                    onReady?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error in CreateUserPseudoId callback: {e.Message}");
                }
            } catch (Exception e) {
                Debug.LogError($"Error in CreateUserPseudoId: {e.Message}");
            }
        }
    }
}