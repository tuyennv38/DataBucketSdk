# Databuckets Unity SDK

Unity SDK for Databuckets analytics platform with automatic event tracking, sessions, and app lifecycle features.

## Installation

1. Import the DatabucketsSDK package into your Unity project

## Quick Start

```csharp
using Databuckets;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Required: Initialize SDK
        DatabucketsTracker.Init("api-endpoint-here","your-api-key-here");
        
        // Optional: Set user properties
        DatabucketsTracker.SetCommonProperty("user_level", 1);
    }
}
```

### ⚠️ Important Notes about Init Method

**Note #1**: The Init method must be called only once before using DatabucketsTracker. The Init method should be called after the scene has been loaded. You can use one of the following two approaches:

**Method 1: Use Coroutine to wait for 1 frame:**

```csharp
IEnumerator Start()
{
    yield return null; // Wait 1 frame
    DatabucketsTracker.Init("api-endpoint-here","your-api-key-here");
}
```

**Method 2: Use delay to wait for 100ms:**

```csharp
void Start()
{
    Invoke(nameof(InitSDK), 0.1f);
}

void InitSDK()
{
    DatabucketsTracker.Init("api-endpoint-here","your-api-key-here");
}
```

## Key Features

### Event Tracking

```csharp
// Simple event
DatabucketsTracker.Record("button_clicked", null);

// Event with parameters
var eventParams = new Dictionary<string, object>
{
    ["button_name"] = "play_button",
    ["level"] = 1,
    ["score"] = 1000
};
DatabucketsTracker.Record("button_clicked", eventParams);

// Event with timing
DatabucketsTracker.Record("level_started", null);
```

**⚠️ Important Notes about Events:**
- Events are automatically sent **every 10 seconds**
- Even if the game is paused, closed, or loses internet connection, events will be **backed up for resending** in subsequent sessions

**Example event with RecordWithTiming:**

```csharp
Dictionary<string, object> eventParams = new Dictionary<string, object>();
eventParams.Add("level", 50);
eventParams.Add("score", 9999);
eventParams.Add("timeToComplete", 300000);
eventParams.Add("difficulty", "Expert");

DatabucketsTracker.Record("CompleteLevel", eventParams);
// Or with timing from startLevel event
DatabucketsTracker.RecordWithTiming("CompleteLevel", eventParams, "level_duration", "startLevel");
// The event will have an additional levelDuration property, which is the time duration from startLevel event to CompleteLevel
```

### `app_exception_log` Tracking

The `app_exception_log` event is sent automatically whenever Unity logs an exception (LogType.Exception), allowing you to track unexpected runtime errors on user devices. 
This feature is optional and only works when EnableExceptionLogTracking() is enabled.

```csharp
DatabucketsTracker.EnableExceptionLogTracking();
```

You can disable this tracking with DisableExceptionLogTracking() when you no longer want the SDK to capture exception logs.

```csharp
DatabucketsTracker.DisableExceptionLogTracking();
```

## API Reference

### Core Methods

| Method | Description | Usage |
| --- | --- | --- |
| `Init(apiEndpoint,apiKey)` | **Required** - Initialize SDK | Call once at app start |
| `Record(eventName, params)` | Record events | Call when tracking events |
| `RecordWithTiming(event, params, timingProp, startEvent)` | Events with timing | Call to measure duration |
| `SetCommonProperty(key, value)` | Set common property | Call when user state changes |
| `SetCommonProperties(dict)` | Set multiple common properties | Call when setting multiple user states |
| `ForceEndCurrentSession()` | Manually end session | Rarely (logout, switch user) |


## Complete Example

```csharp
using UnityEngine;
using Databuckets;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        
        // 1. Initialize SDK
        DatabucketsTracker.Init("api-endpoint-here","your-api-key-here");

        // 2. Enable Exception Log Tracking

        DatabucketsTracker.EnableExceptionLogTracking();
        
        // 3. Configure (optional)
        DatabucketsTracker.SetCommonProperty("user_level", 1);
        DatabucketsTracker.SetCommonProperty("game_mode", "story");
        
        // Or set multiple properties at once
        DatabucketsTracker.SetCommonProperties(new Dictionary<string, object>
        {
            ["user_level"] = 1,
            ["game_mode"] = "story",
            ["user_type"] = "free"
        });

        // 4. Track event
        DatabucketsTracker.Record("app_started", null);
        OnLevelStart(1);
        OnLevelComplete(1, 5000, true);
        OnUserLevelUp(2);
        OnUserLogout();
    }
    
    public void OnLevelStart(int level)
    {
        var levelParams = new Dictionary<string, object>
        {
            ["level"] = level,
            ["difficulty"] = "normal"
        };
        DatabucketsTracker.Record("level_started", levelParams);
    }
    
    public void OnLevelComplete(int level, int score, bool success)
    {
        var completionParams = new Dictionary<string, object>
        {
            ["level"] = level,
            ["score"] = score,
            ["success"] = success
        };
        DatabucketsTracker.RecordWithTiming("level_completed", completionParams, "level_duration", "level_started");
    }
    
    public void OnUserLevelUp(int newLevel)
    {
        DatabucketsTracker.SetCommonProperty("user_level", newLevel);
        DatabucketsTracker.Record("user_level_up", new Dictionary<string, object> { ["new_level"] = newLevel });
    }
    
    public void OnUserLogout()
    {
        DatabucketsTracker.Record("user_logout", null);
        DatabucketsTracker.ForceEndCurrentSession();
    }
}
```

## Quick Reference

### Method Usage Guidelines

- **Init()**: Call once at app start (required)
- **Record()**: Call when tracking events
- **SetCommonProperty()**: Call when user state changes
- **SetCommonProperties()**: Call when setting multiple user states
- **ForceEndCurrentSession()**: Rarely (logout, switch user)

### Auto vs Manual

**Automatic:**
- Session tracking (session_start)
- App lifecycle tracking (app_focus_start, app_focus_end)
- User ID and geo info setup
- Device info collection
- App info collection

**Manual:**
- Business event tracking
- User property updates
- Configuration changes

### Auto-Injected Fields

The SDK automatically adds to every event:
- `user_pseudo_id`, `sdk_ver`, `session_id`, `app_id`, `platform`
- `geo`: Location info (country, region, city, timezone)
- `device`: Device information (model, OS, memory, etc.)
- `install_day`

## Default Event Information

After calling `DatabucketsTracker.Record()` or `RecordWithTiming()`, the event will be created with the following default values:

| Field | Explanation | Type | Example |
| --- | --- | --- | --- |
| `event_date` | Date of event occurrence (YYYYMMDD format) | Number | 20250505 |
| `event_local_day_of_week` | Day of the week in local timezone | String | “1 Monday” |
| `event_local_hour` | Hour of the day in local timezone (0-23) | Number | 11 |
| `event_local_hour_minute` | Hour and minute of the day, combined as HHMM | Number | 1106 |
| `install_day` | Application installation date (YYYYMMDD format) | Number | 20250415 |
| `retention_day` | Number of days from installation to event occurrence | Number | 1 |
| `retention_hour` | Number of hours from installation to event occurrence | Number | 24 |
| `retention_minute` | Number of minutes from installation to event occurrence | Number | 1440 |
| `session_id` | Session ID, consisting of user ID and timestamp | String | “test-user-pseudo-id:1746417808764” |
| `session_progress` | Number of seconds from session start to event occurrence | Number | 170 |
| `user_pseudo_id` | Anonymous ID representing the user, automatically generated using GUID | String | "test-user-pseudo-id" |
| `geo.country` | Country code | String | “VN” |
| `geo.loc` | Geographic coordinates (latitude, longitude) | String | “xx.xxxx,xx.xxxx” |
| `geo.city` | City | String | “Da Nang” |
| `geo.org` | Network provider | String | “AS45903 CMCTelecom Infrastructure Company” |
| `geo.timezone` | Local timezone | String | “Asia/Ho_Chi_Minh” |
| `geo.postal` | Postal code | String | “50250” |
| `geo.region` | Region / Province | String | “Da Nang” |
| `platform` | Application platform | String | “ANDROID” |
| `app_store` | App store | String | “com.android.vending” |
| `app_version` | Application version | String | “1.0” |
| `device.platform_version` | Operating system version | String | “16” |
| `device.user_default_language` | Device default language | String | “en-US” |
| `device.model` | Device model | String | “sdk_gphone64_x86_64” |
| `device.timezone_offset_seconds` | Timezone offset in seconds from UTC | Number | 25200 |
| `device.brand_name` | Device brand | String | “google” |
| `device.category` | Device category | String | “mobile” |
| `app_id` | Application ID | String | “com.mockapplication” |

## Auto-tracking event

| Event                 | When it is sent                                                | Notes                                                  |
| --------------------- | -------------------------------------------------------------- | ------------------------------------------------------ |
| **first_open**        | First time the app is opened within < 10 minutes after installation    | Sent only once                                         |
| **session_start**     | A new session is created: app launch, restart, timeout, resume | Automatically managed by SessionUtil                   |
| **app_focus_start**   | App gains focus                                                | Calculates foreground duration                         |
| **app_focus_end**     | App loses focus, minimized, crash, quit                        | Calculates foreground duration                         |
| **app_exception_log** | Unity `LogType.Exception`                                      | Only sent when `EnableExceptionLogTracking` is enabled |

## PlayerPrefs Note

If you want to clear local storage PlayerPrefs, backup and restore the following values to ensure accuracy of future events:

- `is_sent_first_open`
- `session_number`
- `user_pseudo_id`
- `current_session_start`
- `last_event_timestamp`

⚠️ **Important**: These keys contain critical information for accurate tracking. Deleting without backup will distort analytics data.