# DataBucketPlugin — Hướng dẫn sử dụng

> **Version:** 1.0.1  
> **SDK gốc:** Databuckets Unity SDK v1.0.6  
> **Namespace:** `DataBucketPlugin`

Plugin gồm 2 layer:
1. **Tracking Classes** (9 static classes) — Typed methods cho từng event trong Data Tracking Plan
2. **DataBucketWrapper** — Init guard + raw Record API

### 📚 Tài liệu liên quan

| Tài liệu | Mô tả |
|-----------|-------|
| [DATA_TRACKING_GUIDE.md](DATA_TRACKING_GUIDE.md) | Chi tiết Event Definition, Trigger, KPI, Value Requirement, ví dụ code cho từng event |
| [CHANGE_LOG.md](CHANGE_LOG.md) | Lịch sử thay đổi theo version |

---

## Cài đặt

1. Đã cài Databuckets Unity SDK v1.0.6 vào project
2. Copy thư mục `DataBucketPlugin/` vào `Assets/`
3. Đảm bảo namespace `Databuckets` khả dụng

---

## Quick Start

```csharp
using DataBucketPlugin;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // 1. Khởi tạo SDK (BẮT BUỘC gọi đầu tiên)
        DataBucketWrapper.Init("api-endpoint-here", "your-api-key-here");

        // 2. Bật Exception Tracking (tùy chọn)
        DataBucketWrapper.EnableExceptionTracking();

        // 3. Set user properties (typed)
        DataBucketUserProperties.SetCurrentLevel(1);
        DataBucketUserProperties.SetActiveDay(0);

        // 4. Ghi event (typed — khuyến nghị)
        DataBucketLevel.LevelStart(level: 1, durationTotalStart: 60000, playType: "home");

        // 5. Hoặc ghi event raw (vẫn hỗ trợ)
        DataBucketWrapper.Record("custom_event", null);
    }
}
```

---

## DataBucket Tracking Classes (v1.0.1)

9 static classes cung cấp **typed methods** — developer không cần nhớ tên event hay key params.

| Class | Events | Mô tả |
|-------|--------|-------|
| `DataBucketUserProperties` | — (SetCommonProperty) | Setter cho user properties |
| `DataBucketLevel` | level_start, level_end, level_exit, level_reopen | Level analytics |
| `DataBucketResource` | resource_earn, resource_spend | Resource earn/spend |
| `DataBucketIAP` | iap_show, iap_click, iap_purchase_success, iap_purchase_failed, iap_close | IAP flow |
| `DataBucketAd` | ad_request, ad_impression, ad_click, ad_complete | In-App Advertising |
| `DataBucketNotification` | noti_send, noti_receive, noti_open | Push notifications |
| `DataBucketLiveOps` | feature_first_show, feature_open, feature_close | Live Ops features |
| `DataBucketMetrics` | tutorial_action, button_click, screen_show, screen_exit | Other metrics |
| `DataBucketTechnical` | loading_start, loading_finish | Technical performance |

> 📖 **Chi tiết Trigger, KPI, Value Requirement:** Xem [DATA_TRACKING_GUIDE.md](DATA_TRACKING_GUIDE.md)

### Ví dụ sử dụng

```csharp
// User Properties
DataBucketUserProperties.SetCurrentLevel(15);
DataBucketUserProperties.SetResourceBalance("coin", 5000);

// Level analytics
DataBucketLevel.LevelStart(level: 5, durationTotalStart: 60000, playType: "home");
DataBucketLevel.LevelEnd(level: 5, result: "win", durationPlay: 45000);

// Resource
DataBucketResource.Earn("currency", "gold", 50, "level_win", "reward");

// IAP
DataBucketIAP.Show("home_shop", "shop", "click", new[] { "null" });
DataBucketIAP.PurchaseSuccess("home_shop", "pack", "click", "starterpack", 4.99, "USD");

// Ad
DataBucketAd.Impression("video_rewarded", "Admob", "Admob", "buy_booster", value: 0.05);

// Notification
DataBucketNotification.Send("remind", "Come back!");

// Live Ops
DataBucketLiveOps.FeatureOpen("daily_reward", placement: "home_icon");

// Technical
DataBucketTechnical.LoadingStart("api", "feed", "home");
```

---

## API Reference

### 1. Init — Khởi tạo SDK

```csharp
DataBucketWrapper.Init(string apiEndpoint, string apiKey);
```

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| `apiEndpoint` | string | URL endpoint của Databuckets API |
| `apiKey` | string | API key xác thực |

**Lưu ý:**
- ✅ BẮT BUỘC gọi **1 lần duy nhất** trước mọi API khác
- ✅ Gọi trong `Start()` hoặc sau `yield return null`
- ❌ KHÔNG gọi trong `Awake()` — scene chưa loaded
- ❌ Gọi lần 2 → log warning, không gọi lại SDK

**Ví dụ:**

```csharp
void Start()
{
    DataBucketWrapper.Init("https://api.databuckets.io/v1", "your-api-key");
}
```

---

### 2. Record — Ghi nhận Event

```csharp
DataBucketWrapper.Record(string eventName, Dictionary<string, object> eventParams);
```

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| `eventName` | string | Tên event (dùng `snake_case`) |
| `eventParams` | Dictionary\<string, object\> hoặc `null` | Thuộc tính kèm theo |

**Lưu ý:**
- Events tự động gửi mỗi **10 giây**
- Game pause/đóng/mất mạng → events **backup** và gửi lại
- KHÔNG dùng tên trùng auto-tracking events (`session_start`, `app_focus_start`...)

**Ví dụ:**

```csharp
// Event đơn giản
DataBucketWrapper.Record("main_menu_opened", null);

// Event với params
var levelParams = new Dictionary<string, object>
{
    ["level"] = 1,
    ["difficulty"] = "normal",
    ["lives_remaining"] = 3
};
DataBucketWrapper.Record("level_started", levelParams);
```

---

### 3. RecordWithTiming — Đo thời gian giữa 2 Events

```csharp
DataBucketWrapper.RecordWithTiming(string eventName, Dictionary<string, object> eventParams, string timingProperty, string startEvent);
```

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| `eventName` | string | Tên event kết thúc |
| `eventParams` | Dictionary\<string, object\> | Thuộc tính kèm theo |
| `timingProperty` | string | Tên property chứa giá trị timing |
| `startEvent` | string | Tên event bắt đầu (đã Record trước đó) |

**Lưu ý:**
- ✅ PHẢI gọi `Record()` cho `startEvent` **trước** khi gọi method này
- ✅ Tên `startEvent` phải **khớp chính xác** với event đã Record

**Ví dụ:**

```csharp
// Bước 1: Ghi event bắt đầu
DataBucketWrapper.Record("level_started", new Dictionary<string, object>
{
    ["level"] = 1
});

// Bước 2: Khi kết thúc, ghi event với timing
var completionParams = new Dictionary<string, object>
{
    ["level"] = 1,
    ["score"] = 5000,
    ["success"] = true
};
DataBucketWrapper.RecordWithTiming(
    "level_completed",     // event kết thúc
    completionParams,      // params
    "level_duration",      // property chứa timing
    "level_started"        // event bắt đầu
);
// → SDK tự tính thời gian và lưu vào "level_duration"
```

---

### 4. SetCommonProperty — Set 1 thuộc tính chung

```csharp
DataBucketWrapper.SetCommonProperty(string key, object value);
```

| Tham số | Kiểu | Mô tả |
|---------|------|-------|
| `key` | string | Tên property |
| `value` | object | Giá trị (primitive types) |

Sau khi gọi, property tự động gắn vào **mọi event** tiếp theo.

**Ví dụ:**

```csharp
DataBucketWrapper.SetCommonProperty("user_level", 5);
DataBucketWrapper.SetCommonProperty("game_mode", "story");
```

---

### 5. SetCommonProperties — Set nhiều thuộc tính cùng lúc

```csharp
DataBucketWrapper.SetCommonProperties(Dictionary<string, object> properties);
```

Hiệu quả hơn gọi `SetCommonProperty()` nhiều lần.

**Ví dụ:**

```csharp
DataBucketWrapper.SetCommonProperties(new Dictionary<string, object>
{
    ["user_level"] = 1,
    ["game_mode"] = "story",
    ["user_type"] = "free"
});
```

---

### 6. ForceEndSession — Kết thúc Session thủ công

```csharp
DataBucketWrapper.ForceEndSession();
```

- ✅ Chỉ dùng khi: **user logout**, **chuyển đổi tài khoản**
- ❌ KHÔNG gọi khi đổi scene/màn hình — SDK tự quản lý session
- ✅ Nên gọi `Record()` event trước khi kết thúc session

**Ví dụ:**

```csharp
DataBucketWrapper.Record("user_logout", null);
DataBucketWrapper.ForceEndSession();
```

---

### 7. Exception Tracking — Bật/Tắt theo dõi Exceptions

```csharp
// Bật
DataBucketWrapper.EnableExceptionTracking();

// Tắt
DataBucketWrapper.DisableExceptionTracking();
```

Khi bật, mỗi `LogType.Exception` trong Unity sẽ gửi event `app_exception_log`.

**Ví dụ:**

```csharp
// Chỉ bật cho production
#if !UNITY_EDITOR
    DataBucketWrapper.EnableExceptionTracking();
#endif
```

---

## Kiểm tra Init State

```csharp
// Kiểm tra SDK đã Init chưa
if (DataBucketWrapper.IsInitialized)
{
    // SDK sẵn sàng
}
```

---

## Bảng tổng hợp API

| Method | Mô tả | Guard |
|--------|-------|-------|
| `Init(endpoint, key)` | Khởi tạo SDK (1 lần) | Warning nếu đã Init |
| `Record(name, params)` | Ghi event | Error nếu chưa Init |
| `RecordWithTiming(...)` | Ghi event + timing | Error nếu chưa Init |
| `SetCommonProperty(key, val)` | Set 1 property chung | Error nếu chưa Init |
| `SetCommonProperties(dict)` | Set nhiều properties | Error nếu chưa Init |
| `ForceEndSession()` | Kết thúc session | Error nếu chưa Init |
| `EnableExceptionTracking()` | Bật exception tracking | Error nếu chưa Init |
| `DisableExceptionTracking()` | Tắt exception tracking | Error nếu chưa Init |

---

## Auto-tracking (SDK tự động, KHÔNG cần code)

| Event | Khi nào gửi |
|-------|-------------|
| `first_open` | Mở app lần đầu trong 10 phút sau cài |
| `session_start` | Session mới: app launch, restart, timeout, resume |
| `app_focus_start` | App được focus |
| `app_focus_end` | App mất focus, minimize, crash, quit |
| `app_exception_log` | Unity LogType.Exception (khi bật ExceptionTracking) |
