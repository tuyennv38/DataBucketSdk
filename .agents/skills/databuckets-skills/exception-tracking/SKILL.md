---
name: exception-tracking
description: "Hướng dẫn bật/tắt DatabucketsTracker.EnableExceptionLogTracking() để theo dõi Unity exceptions."
version: "1.0.0"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, databuckets, exception, logging]
---

<!-- @implements task:create-skill-exception-tracking-0008 -->
<!-- @implements feature:skill-exception-tracking-0007 -->

# Databuckets Exception Tracking — Theo dõi Unity Exceptions

> **Mục tiêu:** Hướng dẫn bật/tắt tự động theo dõi Unity exception logs bằng `EnableExceptionLogTracking()` và `DisableExceptionLogTracking()`.

Khi bật, mỗi khi Unity log exception (`LogType.Exception`), SDK tự động gửi event `app_exception_log`. Tính năng tùy chọn, hữu ích cho production builds.

---

## Khi nào sử dụng

- Khi cần theo dõi runtime errors trên thiết bị thực (production).
- Khi muốn tự động gửi exception logs lên analytics dashboard.

## Khi KHÔNG sử dụng

- Trong development/debug builds — đã có Unity Console.
- Khi không có kế hoạch giám sát dashboard.

---

## Hướng dẫn thực hiện

### Bật Exception Tracking

```csharp
DatabucketsTracker.EnableExceptionLogTracking();
```

Sau khi bật, mọi `LogType.Exception` trong Unity sẽ tự động gửi event `app_exception_log`.

### Tắt Exception Tracking

```csharp
DatabucketsTracker.DisableExceptionLogTracking();
```

Dừng theo dõi exceptions. Dùng khi không cần nữa hoặc chuyển sang debug mode.

---

## Ví dụ

### Ví dụ 1: Bật ngay sau Init

```csharp
DatabucketsTracker.Init("https://api.databuckets.io/v1", "your-api-key");
DatabucketsTracker.EnableExceptionLogTracking();
// Từ đây, mọi Unity exception tự động gửi lên analytics
```

### Ví dụ 2: Chỉ bật cho production

```csharp
DatabucketsTracker.Init("endpoint", "key");

#if !UNITY_EDITOR
    DatabucketsTracker.EnableExceptionLogTracking();
#endif
```

### Ví dụ 3: Tắt khi chuyển sang debug mode

```csharp
public void ToggleDebugMode(bool isDebug)
{
    if (isDebug)
    {
        DatabucketsTracker.DisableExceptionLogTracking();
    }
    else
    {
        DatabucketsTracker.EnableExceptionLogTracking();
    }
}
```

---

## Best Practices

- ✅ Hữu ích cho production — theo dõi runtime errors trên thiết bị thực
- ✅ Tính năng tùy chọn (optional), không bắt buộc
- ✅ Dùng `#if !UNITY_EDITOR` để chỉ bật trong build thực
- ❌ KHÔNG cần bật trong development/debug builds (đã có Console)
- ❌ KHÔNG bật mà không giám sát dashboard — lãng phí tracking

---

## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: Bật nhưng không giám sát

```csharp
// SAI — bật tracking nhưng không ai xem dashboard
DatabucketsTracker.EnableExceptionLogTracking();
// → Lãng phí bandwidth gửi data mà không hành động
```
**Tại sao sai:** Tracking chỉ có giá trị khi có quy trình xem và xử lý exceptions.
**Cách đúng:** Chỉ bật khi đã setup monitoring/alerting trên dashboard.

### ❌ Anti-Pattern 2: Bật trong Editor/Development

```csharp
// SAI — bật cả trong Editor
void Start()
{
    DatabucketsTracker.Init("endpoint", "key");
    DatabucketsTracker.EnableExceptionLogTracking(); // Editor exceptions cũng gửi
}
```
**Tại sao sai:** Editor exceptions không phản ánh lỗi thực tế, làm nhiễu data.
**Cách đúng:** Dùng `#if !UNITY_EDITOR` guard.

---

## Skill liên quan

- `databuckets-skills/init` — Khởi tạo SDK
- `databuckets-skills/record` — Ghi nhận event thủ công
