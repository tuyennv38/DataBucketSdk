---
name: set-property
description: "Hướng dẫn gọi DatabucketsTracker.SetCommonProperty() để set thuộc tính chung cho mọi event."
version: "1.0.0"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, databuckets, property]
---

<!-- @implements task:create-skill-set-property-0005 -->
<!-- @implements feature:skill-set-property-0004 -->

# Databuckets Set Common Property — Set thuộc tính chung

> **Mục tiêu:** Hướng dẫn gọi `SetCommonProperty()` để đặt 1 thuộc tính chung gắn tự động vào mọi event sau đó.

`SetCommonProperty()` cho phép set **1 key-value** sẽ được gắn tự động vào mọi event tiếp theo mà không cần truyền lại trong params mỗi lần `Record()`.

---

## Khi nào sử dụng

- Khi cần set **1 thuộc tính** chung (user level up, đổi game mode, mua premium).
- Khi thuộc tính thay đổi ít và áp dụng cho nhiều event.

## Khi KHÔNG sử dụng

- Khi cần set **nhiều thuộc tính** cùng lúc → dùng skill `set-properties` (hiệu quả hơn).
- Khi data thay đổi liên tục mỗi frame → không phù hợp.

---

## Hướng dẫn thực hiện

### Bước 1: Xác định key-value cần set

Chọn key rõ ràng, **KHÔNG** trùng với auto-injected fields (`user_pseudo_id`, `session_id`, `platform`...).

### Bước 2: Gọi SetCommonProperty

```csharp
DatabucketsTracker.SetCommonProperty(key, value);
```

**Tham số:**
- `key` (string): Tên property
- `value` (object): Giá trị property

Sau khi gọi, property này tự động gắn vào **mọi event** tiếp theo.

---

## Ví dụ

### Ví dụ 1: User level up

```csharp
// Khi user level up
DatabucketsTracker.SetCommonProperty("user_level", 5);
// Mọi event sau đó sẽ có field "user_level": 5
```

### Ví dụ 2: Đổi game mode

```csharp
DatabucketsTracker.SetCommonProperty("game_mode", "story");
```

### Ví dụ 3: User mua premium

```csharp
DatabucketsTracker.SetCommonProperty("user_type", "premium");
```

---

## Best Practices

- ✅ Gọi khi trạng thái user thay đổi (level up, đổi mode, mua premium)
- ✅ Property tự động gắn vào mọi event tiếp theo — không cần truyền lại trong params
- ✅ Dùng cho thuộc tính thay đổi ít và áp dụng cho nhiều event
- ❌ KHÔNG dùng cho data thay đổi liên tục (score realtime mỗi frame)
- ❌ KHÔNG dùng key trùng auto-injected fields (`user_pseudo_id`, `session_id`...)

---

## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: Set property quá thường xuyên

```csharp
// SAI — gọi mỗi frame
void Update()
{
    DatabucketsTracker.SetCommonProperty("score", currentScore);
}
```
**Tại sao sai:** Overhead không cần thiết. Score thay đổi liên tục, nên truyền qua params khi `Record()`.
**Cách đúng:** Dùng `Record("score_updated", params)` với score trong params.

### ❌ Anti-Pattern 2: Gọi nhiều lần thay vì dùng SetCommonProperties

```csharp
// SAI — gọi 5 lần liên tiếp
DatabucketsTracker.SetCommonProperty("user_level", 1);
DatabucketsTracker.SetCommonProperty("game_mode", "story");
DatabucketsTracker.SetCommonProperty("user_type", "free");
DatabucketsTracker.SetCommonProperty("country", "VN");
DatabucketsTracker.SetCommonProperty("language", "vi");
```
**Tại sao sai:** Code dài, kém hiệu quả.
**Cách đúng:** Dùng `SetCommonProperties()` với Dictionary. Xem skill `set-properties`.

---

## Skill liên quan

- `databuckets-skills/set-properties` — Dùng khi cần set **nhiều** properties cùng lúc (thay thế)
- `databuckets-skills/record` — Properties sẽ tự động gắn vào event khi Record
- `databuckets-skills/init` — Khởi tạo SDK
