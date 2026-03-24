# DataBucketPlugin – Change Log

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
