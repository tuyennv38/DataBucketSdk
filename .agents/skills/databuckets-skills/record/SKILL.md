---
name: record
description: "Hướng dẫn gọi DatabucketsTracker.Record() để ghi nhận business events trong Unity."
version: "1.0.0"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, databuckets, event]
---

<!-- @implements task:create-skill-record-0003 -->
<!-- @implements feature:skill-record-0002 -->

# Databuckets Record — Ghi nhận Event

> **Mục tiêu:** Hướng dẫn gọi `DatabucketsTracker.Record()` để ghi nhận business events với params tùy chọn.

`Record()` là API chính để gửi custom event analytics. Events tự động gửi mỗi 10 giây và được backup khi mất mạng.

---

## Khi nào sử dụng

- Khi cần ghi nhận một business event (level started, purchase, button click...).
- Khi cần gửi event kèm thuộc tính bổ sung (Dictionary params).

## Khi KHÔNG sử dụng

- Khi cần đo thời gian giữa 2 events → dùng skill `record-timing`.
- Khi cần set thuộc tính chung cho mọi event → dùng skill `set-property` hoặc `set-properties`.

---

## Hướng dẫn thực hiện

### Bước 1: Xác định tên event

Đặt tên event theo `snake_case`. **KHÔNG** dùng tên trùng với auto-tracking events: `session_start`, `app_focus_start`, `app_focus_end`, `first_open`, `app_exception_log`.

### Bước 2: Chuẩn bị params (tùy chọn)

Tạo `Dictionary<string, object>` chứa các thuộc tính kèm theo. Chỉ dùng **primitive types** (string, int, float, bool). Truyền `null` nếu không cần params.

### Bước 3: Gọi Record

```csharp
DatabucketsTracker.Record("event_name", params);
```

**Tham số:**
- `eventName` (string): Tên event
- `params` (Dictionary\<string, object\> hoặc null): Thuộc tính kèm theo

---

## Ví dụ

### Ví dụ 1: Event đơn giản (không có params)

```csharp
using Databuckets;

DatabucketsTracker.Record("main_menu_opened", null);
```

### Ví dụ 2: Event với params Dictionary

```csharp
using Databuckets;
using System.Collections.Generic;
var levelParams = new Dictionary<string, object>
{
    ["level"] = 1,
    ["difficulty"] = "normal",
    ["lives_remaining"] = 3
};
DatabucketsTracker.Record("level_started", levelParams);
```

### Ví dụ 3: Event mua hàng

```csharp
var purchaseParams = new Dictionary<string, object>
{
    ["item_id"] = "gem_pack_100",
    ["price"] = 0.99f,
    ["currency"] = "USD"
};
DatabucketsTracker.Record("in_app_purchase", purchaseParams);
```

---

## Best Practices

- ✅ Đặt tên event theo `snake_case`: `level_started`, `item_purchased`
- ✅ Events tự động gửi mỗi 10 giây — không cần flush thủ công
- ✅ Game bị pause/đóng/mất mạng → events backup và gửi lại session sau
- ✅ Params có thể là `null` nếu event không cần thuộc tính
- ❌ KHÔNG dùng tên event trùng auto-tracking events
- ❌ KHÔNG dùng lẫn camelCase và snake_case → khó query analytics

---

## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: Gọi Record trước Init

```csharp
// SAI — SDK chưa sẵn sàng
DatabucketsTracker.Record("level_started", null);
// Init sau → event ở trên bị mất
DatabucketsTracker.Init("endpoint", "key");
```
**Tại sao sai:** Event bị mất vì SDK chưa init.
**Cách đúng:** Luôn gọi `Init()` trước bất kỳ `Record()` nào.

### ❌ Anti-Pattern 2: Truyền nested object vào params

```csharp
// SAI — nested Dictionary
var badParams = new Dictionary<string, object>
{
    ["nested"] = new Dictionary<string, object> { ["key"] = "value" }
};
DatabucketsTracker.Record("event", badParams);
```
**Tại sao sai:** SDK chỉ hỗ trợ primitive types.
**Cách đúng:** Flatten data thành key-value đơn giản.

---

## Skill liên quan

- `databuckets-skills/init` — Phải gọi Init trước khi Record
- `databuckets-skills/record-timing` — Đo thời gian giữa 2 events (dựa trên Record)
- `databuckets-skills/set-property` — Set thuộc tính chung cho mọi event
