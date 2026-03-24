---
name: record-timing
description: "Hướng dẫn gọi DatabucketsTracker.RecordWithTiming() để đo thời gian giữa 2 events."
version: "1.0.0"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, databuckets, timing]
---

<!-- @implements task:create-skill-record-timing-0004 -->
<!-- @implements feature:skill-record-timing-0003 -->

# Databuckets Record With Timing — Đo thời gian giữa 2 Events

> **Mục tiêu:** Hướng dẫn gọi `RecordWithTiming()` để đo thời gian từ event bắt đầu đến event kết thúc.

`RecordWithTiming()` tự động tính khoảng thời gian giữa 2 events và lưu vào property chỉ định. Phù hợp cho: level duration, loading time, tutorial completion.

---

## Khi nào sử dụng

- Khi cần đo thời gian chơi 1 level (start → complete/fail).
- Khi cần đo loading time hoặc tutorial duration.
- Khi cần đo bất kỳ khoảng thời gian nào giữa 2 events.

## Khi KHÔNG sử dụng

- Khi chỉ cần ghi 1 event đơn lẻ → dùng skill `record`.
- Khi event không có cặp start/end tương ứng.

---

## Hướng dẫn thực hiện

### Bước 1: Gọi Record() cho event bắt đầu

```csharp
DatabucketsTracker.Record("level_started", startParams);
```

PHẢI gọi `Record()` cho startEvent trước. SDK lưu timestamp của event này.

### Bước 2: Gọi RecordWithTiming() khi kết thúc

```csharp
DatabucketsTracker.RecordWithTiming(
    eventName,        // Tên event kết thúc
    params,           // Thuộc tính kèm theo
    timingProperty,   // Tên property chứa giá trị timing
    startEvent        // Tên event bắt đầu (đã Record ở Bước 1)
);
```

**Tham số:**
- `eventName` (string): Tên event kết thúc
- `params` (Dictionary\<string, object\>): Thuộc tính kèm theo
- `timingProperty` (string): Tên property chứa giá trị thời gian đo được
- `startEvent` (string): Tên event bắt đầu (đã gọi `Record` trước đó)

SDK tự tính: `timingProperty = timestamp(eventName) - timestamp(startEvent)`.

---

## Ví dụ

### Ví dụ: Đo thời gian chơi level

```csharp
// === Khi BẮT ĐẦU level ===
DatabucketsTracker.Record("level_started", new Dictionary<string, object>
{
    ["level"] = 1
});

// === Khi KẾT THÚC level (vài phút sau) ===
var completionParams = new Dictionary<string, object>
{
    ["level"] = 1,
    ["score"] = 5000,
    ["success"] = true
};
DatabucketsTracker.RecordWithTiming(
    "level_completed",    // event kết thúc
    completionParams,     // params
    "level_duration",     // property chứa timing
    "level_started"       // event bắt đầu
);
// → SDK tự tính thời gian giữa "level_started" và "level_completed"
// → Lưu vào property "level_duration"
```

---

## Best Practices

- ✅ PHẢI gọi `Record()` cho startEvent **trước** khi gọi `RecordWithTiming()`
- ✅ Dùng cho cặp event bắt đầu-kết thúc: level, tutorial, loading
- ✅ Đặt tên `timingProperty` rõ ràng: `level_duration`, `loading_time`, `tutorial_duration`
- ✅ Tên `startEvent` phải **khớp chính xác** với event đã `Record()`
- ❌ KHÔNG dùng cho event không có cặp start/end tương ứng

---

## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: Quên Record startEvent trước

```csharp
// SAI — chưa Record "level_started"
DatabucketsTracker.RecordWithTiming(
    "level_completed", params, "level_duration", "level_started"
);
```
**Tại sao sai:** SDK không tìm được timestamp bắt đầu → timing = 0 hoặc sai.
**Cách đúng:** Luôn gọi `Record("level_started", ...)` trước.

### ❌ Anti-Pattern 2: Tên startEvent không khớp

```csharp
DatabucketsTracker.Record("levelStarted", null); // camelCase
DatabucketsTracker.RecordWithTiming(
    "level_completed", params, "duration", "level_started" // snake_case
);
```
**Tại sao sai:** `"levelStarted"` != `"level_started"` → không match.
**Cách đúng:** Dùng cùng một tên chính xác.

---

## Skill liên quan

- `databuckets-skills/init` — Phải Init trước
- `databuckets-skills/record` — Dùng để ghi startEvent trước khi gọi RecordWithTiming
