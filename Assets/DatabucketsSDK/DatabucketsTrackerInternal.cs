using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Databuckets
{
    public class DatabucketsTrackerInternal
    {
        private const string evTsList = "EvTsList";
        private int flushIntervalSecs = 10;
        private int storageIndex = 0;
        private Dictionary<string, object> commonProperties = new Dictionary<string, object>();
        private Dictionary<string, long> eventTimestamps = new Dictionary<string, long>();
        private long startAppFocusTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        protected bool sentOk = false;
        private bool flushing = false;

        public DatabucketsTrackerInternal(int flushIntervalSecs)
        {
            this.flushIntervalSecs = flushIntervalSecs;
        }

        public void SetCommonProperties(Dictionary<string, object> commonProperties)
        {
            try {
                foreach (var prop in commonProperties)
                {
                    SetCommonProperty(prop.Key, prop.Value);
                }
            } catch (Exception e) {
                Debug.LogError($"Error in SetCommonProperties: {e.Message}");
            }
        }

        public void SetCommonProperty(string key, object value)
        {
            try {
                commonProperties[key] = value;
            } catch (Exception e) {
                Debug.LogError($"Error in SetCommonProperty: {e.Message}");
            }
        }

        public void RecordWithTiming(
            string eventName, Dictionary<string, object> eventParams,
            string timingProp, string startEvent)
        {
            try {
                long startEventTs = 0;

                if (eventTimestamps.TryGetValue(startEvent, out startEventTs))
                {
                    eventParams[timingProp] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - startEventTs;
                }
                else
                {
                    eventParams[timingProp] = 0;
                }
                Record(eventName, eventParams);
            } catch (Exception e) {
                Debug.LogError($"Error in RecordWithTiming: {e.Message}");
            }
        }

        public void Record(string eventName, Dictionary<string, object> eventParams)
        {
            try {
                Dictionary<string, object> _event = new Dictionary<string, object>();

                var tsMillis = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                eventTimestamps[eventName] = tsMillis;
                Debug.Log($"DatabucketsTrackerInternal: Recorded event '{eventName}' at {tsMillis}");
                // Build event
                _event["event_name"] = eventName;
                _event["_ts"] = tsMillis;

                // Set common properties
                foreach (var prop in commonProperties)
                {
                    _event[prop.Key] = prop.Value;
                }

                // Time Section
                if (commonProperties.TryGetValue("device", out var deviceObj) &&
                        deviceObj is Dictionary<string, object> device)
                {
                    int timezoneOffsetSeconds = 0;
                    try
                    {
                        timezoneOffsetSeconds = device != null && device.ContainsKey("timezone_offset_seconds")
                            ? Convert.ToInt32(device["timezone_offset_seconds"])
                            : (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalSeconds;
                    }
                    catch (Exception)
                    {
                        timezoneOffsetSeconds = (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalSeconds;
                    }

                    // Event time fields
                    _event["event_local_day_of_week"] = EventDateUtil.GetLocalDayOfWeek(tsMillis, timezoneOffsetSeconds);
                    _event["event_local_hour"] = EventDateUtil.GetLocalHour(tsMillis, timezoneOffsetSeconds);
                    _event["event_local_hour_minute"] = EventDateUtil.GetLocalHourMinute(tsMillis, timezoneOffsetSeconds);

                }

                _event["session_id"] = SessionUtil.GetCurrentSessionId();
                _event["session_progress"] = (tsMillis - SessionUtil.GetCurrentSessionStart()) / 1000;
                _event["session_number"] = SessionUtil.GetSessionNumber();
                _event["event_date"] = EventDateUtil.GetUtcDate(tsMillis);

                AppInfoUtil.AddInstallAndRetentionInfo(_event);
                
                // Calculate app_focus time
                _event["app_focus_duration"] = tsMillis - startAppFocusTimestamp;
                startAppFocusTimestamp = tsMillis;
                
                // Set passed params
                foreach (var eventParam in eventParams)
                {
                    _event[eventParam.Key] = eventParam.Value;
                }
                SessionUtil.SetLastEventTimestamp(tsMillis);
                // Serialization
                try
                {
                    var serialized = JsonConvert.SerializeObject(_event) + "\n";
                    var ts = tsMillis / 1000 / flushIntervalSecs;
                    var curTs = "[" + ts.ToString() + "]";

                    var storageIndexStr = storageIndex.ToString();
                    var tsList = PlayerPrefs.GetString(evTsList + storageIndexStr, "");
                    if (tsList.Length == 0)
                    {
                        tsList = curTs;
                    }
                    else if (!tsList.Contains(curTs))
                    {
                        tsList = curTs + "," + tsList;
                    }

                    SetString(evTsList + storageIndexStr, tsList);

                    // events are stored in buckets corresponding to flushIntervalSecs
                    SetString(curTs + storageIndexStr, serialized + PlayerPrefs.GetString(curTs + storageIndexStr, ""));
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Error when serializing and saving event: " + e.Message);
                }
            } catch (Exception e) {
                Debug.LogError($"Error in Record: {e.Message}");
            }

        }

        internal void AutoRecord(string eventName, long eventTimestampMillis, Dictionary<string, object> eventParams)
        {
            try {
                Dictionary<string, object> _event = new Dictionary<string, object>();

                var tsMillis = eventTimestampMillis;
                eventTimestamps[eventName] = tsMillis;
                Debug.Log($"DatabucketsTrackerInternal: Auto-Recorded event '{eventName}' at {tsMillis}");
                // Build event
                _event["event_name"] = eventName;
                _event["_ts"] = tsMillis;

                // Set common properties
                foreach (var prop in commonProperties)
                {
                    _event[prop.Key] = prop.Value;
                }

                // Time Section
                if (commonProperties.TryGetValue("device", out var deviceObj) &&
                        deviceObj is Dictionary<string, object> device)
                {
                    int timezoneOffsetSeconds = 0;
                    try
                    {
                        timezoneOffsetSeconds = device != null && device.ContainsKey("timezone_offset_seconds")
                            ? Convert.ToInt32(device["timezone_offset_seconds"])
                            : (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalSeconds;
                    }
                    catch (Exception)
                    {
                        timezoneOffsetSeconds = (int)TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalSeconds;
                    }

                    // Event time fields
                    _event["event_local_day_of_week"] = EventDateUtil.GetLocalDayOfWeek(tsMillis, timezoneOffsetSeconds);
                    _event["event_local_hour"] = EventDateUtil.GetLocalHour(tsMillis, timezoneOffsetSeconds);
                    _event["event_local_hour_minute"] = EventDateUtil.GetLocalHourMinute(tsMillis, timezoneOffsetSeconds);

                }

                // Session values
                _event["session_id"] = SessionUtil.GetCurrentSessionId();
                _event["session_progress"] = (tsMillis - SessionUtil.GetCurrentSessionStart()) / 1000;
                _event["session_number"] = SessionUtil.GetSessionNumber();
                _event["event_date"] = EventDateUtil.GetUtcDate(tsMillis);

                AppInfoUtil.AddInstallAndRetentionInfo(_event);

                var appFocusTimeMs = tsMillis - startAppFocusTimestamp;
                // Calculate app_focus time
                if (eventName == "session_start" || eventName == "first_open" || eventName == "app_focus_start" || appFocusTimeMs < 0)
                {
                    _event["app_focus_duration"] = 0;
                }
                else
                {
                    _event["app_focus_duration"] = appFocusTimeMs;
                }

                if (eventName != "session_start" && eventName != "first_open") 
                {
                    startAppFocusTimestamp = tsMillis;
                }

                // Set passed params
                foreach (var eventParam in eventParams)
                {
                    _event[eventParam.Key] = eventParam.Value;
                }
                SessionUtil.SetLastEventTimestamp(tsMillis);
                // Serialization
                try
                {
                    var serialized = JsonConvert.SerializeObject(_event) + "\n";
                    var ts = tsMillis / 1000 / flushIntervalSecs;
                    var curTs = "[" + ts.ToString() + "]";

                    var storageIndexStr = storageIndex.ToString();
                    var tsList = PlayerPrefs.GetString(evTsList + storageIndexStr, "");
                    if (tsList.Length == 0)
                    {
                        tsList = curTs;
                    }
                    else if (!tsList.Contains(curTs))
                    {
                        tsList = curTs + "," + tsList;
                    }

                    SetString(evTsList + storageIndexStr, tsList);

                    // events are stored in buckets corresponding to flushIntervalSecs
                    SetString(curTs + storageIndexStr, serialized + PlayerPrefs.GetString(curTs + storageIndexStr, ""));
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Error when serializing and saving event: " + e.Message);
                }
            } catch (Exception e) {
                Debug.LogError($"Error in AutoRecord: {e.Message}");
            }

        }

        public IEnumerator Flush()
        {
            if (!flushing)
            {
                flushing = true;

                var storageIndexStr = storageIndex.ToString();
                storageIndex = (storageIndex + 1) % 2;

                string[] tsList = null;
                try {
                    tsList = PlayerPrefs.GetString(evTsList + storageIndexStr, "").Split(',');
                } catch (Exception e) {
                    Debug.LogError($"Error getting tsList in Flush: {e.Message}");
                    flushing = false;
                    yield break;
                }

                string remainedTs = "";
                var tsCount = 0;

                foreach (var ts in tsList)
                {
                    var key = ts + storageIndexStr;
                    var shouldDeleteKey = true;

                    // just send the 500 recent buckets of events and discard the rest (too old events)
                    if (tsCount < 500)
                    {
                        string payload = "";
                        try {
                            payload = PlayerPrefs.GetString(key, "");
                        } catch (Exception e) {
                            Debug.LogWarning($"Error getting payload for key {key}: {e.Message}");
                        }

                        if (payload.Length > 0)
                        {
                            long parsedTs = 0;
                            bool parseSuccess = false;
                            try
                            {
                                sentOk = false;
                                Debug.Log($"{payload}");
                                parsedTs = long.Parse(ts.Substring(1, ts.Length - 2)) * 1000 * flushIntervalSecs;
                                parseSuccess = true;
                            }
                            catch (Exception e)
                            {
                                Debug.LogWarning($"Error parsing event bucket {ts}: {e.Message}");
                                shouldDeleteKey = true;
                            }

                            if (parseSuccess)
                            {
                                yield return Send(payload, parsedTs);

                                if (!sentOk)
                                {
                                    // back up events to retry later
                                    if (remainedTs.Length == 0)
                                    {
                                        remainedTs = ts;
                                    }
                                    else
                                    {
                                        remainedTs += "," + ts;
                                    }
                                    shouldDeleteKey = false;
                                }
                            }
                        }
                    }

                    if (shouldDeleteKey)
                    {
                        DeleteKey(key);
                    }

                    tsCount++;
                }

                // save remained events to retry later
                try {
                    if (remainedTs.Length > 0)
                    {
                        SetString(evTsList + storageIndexStr, remainedTs);
                    }
                    else
                    {
                        DeleteKey(evTsList + storageIndexStr);
                    }
                } catch (Exception e) {
                    Debug.LogError($"Error saving remained events in Flush: {e.Message}");
                }

                flushing = false;
            }
        }

        public int getFlushInterval()
        {
            return flushIntervalSecs;
        }

        protected virtual void SetString(string key, string value)
        {
            try {
                PlayerPrefs.SetString(key, value);
            } catch (Exception e) {
                Debug.LogError($"Error in SetString for key {key}: {e.Message}");
            }
        }

        protected virtual void DeleteKey(string key)
        {
            try {
                PlayerPrefs.DeleteKey(key);
            } catch (Exception e) {
                Debug.LogError($"Error in DeleteKey for key {key}: {e.Message}");
            }
        }

        protected virtual IEnumerator Send(string payload, long ts)
        {
            sentOk = true;
            yield return null;
        }
    }
}
