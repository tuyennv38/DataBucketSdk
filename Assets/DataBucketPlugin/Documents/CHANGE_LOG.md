# DataBucketPlugin – Change Log

---

## DataBucketPlugin - v1.0.1 (24th 03 2026)

### Change

- **[NEW] DataBucketUserProperties:** Static class cung cấp 13 typed setter methods cho User Properties (current_level, ua_attribution, resource_balance, is_iap_user, active_day, connection_type, win/lose_streak...).
- **[NEW] DataBucketLevel:** Static class cho Level analytics events — `LevelStart()`, `LevelEnd()`, `LevelExit()`, `LevelReopen()`.
- **[NEW] DataBucketResource:** Static class cho Resource events — `Earn()`, `Spend()`.
- **[NEW] DataBucketIAP:** Static class cho IAP events — `Show()`, `Click()`, `PurchaseSuccess()`, `PurchaseFailed()`, `Close()`.
- **[NEW] DataBucketAd:** Static class cho IAA events — `Request()`, `Impression()`, `Click()`, `Complete()`.
- **[NEW] DataBucketNotification:** Static class cho Notification events — `Send()`, `Receive()`, `Open()`.
- **[NEW] DataBucketLiveOps:** Static class cho Live Ops events — `FeatureFirstShow()`, `FeatureOpen()`, `FeatureClose()`.
- **[NEW] DataBucketMetrics:** Static class cho Other metrics — `TutorialAction()`, `ButtonClick()`, `ScreenShow()`, `ScreenExit()`.
- **[NEW] DataBucketTechnical:** Static class cho Technical performance — `LoadingStart()`, `LoadingFinish()`.
- **[NEW] DATA_TRACKING_GUIDE.md:** Tài liệu chi tiết Event Definition, Trigger, KPI, Value Requirement cho từng event.
- **README.md:** Cập nhật thêm hướng dẫn DataBucket Tracking Classes.

---

## DataBucketPlugin - v1.0.0 (24th 03 2026)

### Change

- **DataBucketWrapper:** Tạo static wrapper class bọc toàn bộ 7 API của Databuckets Unity SDK v1.0.6:
  - `Init()` — Khởi tạo SDK với kiểm tra trùng lặp (log warning nếu gọi lại)
  - `Record()` — Ghi nhận business event
  - `RecordWithTiming()` — Ghi event kèm đo thời gian giữa 2 events
  - `SetCommonProperty()` — Set 1 thuộc tính chung cho mọi event
  - `SetCommonProperties()` — Set nhiều thuộc tính chung cùng lúc
  - `ForceEndSession()` — Kết thúc session thủ công
  - `EnableExceptionTracking()` / `DisableExceptionTracking()` — Bật/tắt theo dõi Unity exceptions
- **DataBucketWrapper:** Tự động kiểm tra Init state (`_isInitialized`) trước khi gọi bất kỳ API nào. Log `Debug.LogError` khi gọi API trước `Init()`.
- **DataBucketWrapper:** Thêm property `IsInitialized` để kiểm tra trạng thái SDK.
- **README.md:** Tạo tài liệu hướng dẫn sử dụng từng API với code examples.
- **DataBucketWrapperSample.cs:** Tạo sample script MonoBehaviour test tất cả 7 API.
