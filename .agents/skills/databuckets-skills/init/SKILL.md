---
name: init
description: "Hướng dẫn gọi DatabucketsTracker.Init() để khởi tạo SDK đúng cách trong Unity."
version: "1.0.0"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, databuckets, init]
---

<!-- @implements task:create-skill-init-0002 -->
<!-- @implements feature:skill-init-0001 -->

# Databuckets Init — Khởi tạo SDK

> **Mục tiêu:** Hướng dẫn gọi `DatabucketsTracker.Init()` đúng cách — bắt buộc gọi 1 lần duy nhất trước mọi API khác.

`Init()` là API khởi tạo SDK. Gọi 1 lần duy nhất trong app lifecycle.

---

## Khi nào sử dụng

- Khi tích hợp Databuckets SDK vào project Unity mới.
- Khi cần khởi tạo SDK trong app lifecycle.

## Khi KHÔNG sử dụng

- Khi SDK đã được init rồi — KHÔNG gọi lại lần nữa.
- Khi cần ghi event → dùng skill `record`.

---

## Điều kiện tiên quyết

1. Đã cài Databuckets Unity SDK v1.0.6 vào project.
2. Có `apiEndpoint` và `apiKey` hợp lệ.

---

## Hướng dẫn thực hiện

### Bước 1: Xác định vị trí gọi Init

Gọi `Init()` trong `Start()` hoặc sau `yield return null` trong Coroutine. **KHÔNG gọi trong `Awake()`** — scene chưa loaded đầy đủ.

### Bước 2: Gọi Init

```csharp
DatabucketsTracker.Init("api-endpoint-here", "your-api-key-here");
```

**Tham số:**
- `apiEndpoint` (string): URL endpoint của Databuckets API
- `apiKey` (string): API key xác thực

### Bước 3: Verify

Sau khi gọi `Init()`, SDK tự động:
- Tạo `user_pseudo_id` (lưu PlayerPrefs)
- Gửi `first_open` event (lần đầu tiên)
- Bắt đầu session tracking
- Thu thập device/app info

---

## Ví dụ

### Ví dụ 1: Init trong Start() (Khuyến nghị)

```csharp
using Databuckets;

public class AnalyticsManager : MonoBehaviour
{
    void Start()
    {
        DatabucketsTracker.Init(
            "https://api.databuckets.io/v1",
            "your-api-key-here"
        );
    }
}
```

### Ví dụ 2: Init bằng Coroutine (chờ 1 frame)

```csharp
using Databuckets;

public class AnalyticsManager : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null; // Chờ scene loaded hoàn toàn
        DatabucketsTracker.Init(
            "https://api.databuckets.io/v1",
            "your-api-key-here"
        );
    }
}
```

### Ví dụ 3: Init bằng Invoke delay 100ms

```csharp
using Databuckets;

public class AnalyticsManager : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(InitSDK), 0.1f);
    }

    void InitSDK()
    {
        DatabucketsTracker.Init(
            "https://api.databuckets.io/v1",
            "your-api-key-here"
        );
    }
}
```

---

## Best Practices

- ✅ Gọi trong `Start()`, sau `yield return null`, hoặc bằng `Invoke(nameof(InitSDK), 0.1f)`
- ✅ Chỉ gọi **1 lần duy nhất** trong toàn bộ app lifecycle
- ✅ Gọi **trước** mọi API khác của SDK (`Record`, `SetCommonProperty`...)
- ❌ KHÔNG gọi trong `Awake()` — scene chưa loaded
- ❌ KHÔNG gọi nhiều lần — hành vi không xác định

---

## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: Init trong Awake()

```csharp
// SAI — scene chưa sẵn sàng
void Awake()
{
    DatabucketsTracker.Init("endpoint", "key");
}
```
**Tại sao sai:** `Awake()` chạy trước khi scene loaded đầy đủ, SDK init có thể thất bại.
**Cách đúng:** Dùng `Start()` hoặc Coroutine.

### ❌ Anti-Pattern 2: Gọi Init nhiều lần

```csharp
// SAI — gọi Init mỗi khi vào scene mới
void Start()
{
    DatabucketsTracker.Init("endpoint", "key"); // Lần 2, 3, 4...
}
```
**Tại sao sai:** Init chỉ cần gọi 1 lần. Gọi lại gây hành vi không xác định.
**Cách đúng:** Dùng singleton pattern hoặc kiểm tra đã init chưa.

### ❌ Anti-Pattern 3: Gọi API khác trước Init

```csharp
// SAI — Record trước khi Init
DatabucketsTracker.Record("level_started", null);
DatabucketsTracker.Init("endpoint", "key");
```
**Tại sao sai:** Event bị mất vì SDK chưa sẵn sàng.
**Cách đúng:** Luôn Init trước, Record sau.

---

## Skill liên quan

- `databuckets-skills` — Tổng quan SDK, chọn skill phù hợp
- `databuckets-skills/record` — Ghi nhận event (gọi sau Init)
- `databuckets-skills/exception-tracking` — Bật exception tracking (gọi ngay sau Init)
