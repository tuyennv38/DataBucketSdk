---
name: set-properties
description: "Hướng dẫn gọi DatabucketsTracker.SetCommonProperties() để set nhiều thuộc tính chung cùng lúc."
version: "1.0.0"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, databuckets, property, batch]
---

<!-- @implements task:create-skill-set-properties-0006 -->
<!-- @implements feature:skill-set-properties-0005 -->

# Databuckets Set Common Properties — Set nhiều thuộc tính cùng lúc

> **Mục tiêu:** Hướng dẫn gọi `SetCommonProperties()` để đặt nhiều thuộc tính chung cùng lúc bằng Dictionary.

`SetCommonProperties()` cho phép set **nhiều key-value** cùng lúc. Hiệu quả hơn gọi `SetCommonProperty()` nhiều lần liên tiếp.

---

## Khi nào sử dụng

- Khi cần set **nhiều thuộc tính** cùng lúc (sau login, load user profile).
- Khi có 3+ properties cần set liên tiếp.

## Khi KHÔNG sử dụng

- Khi chỉ cần set **1 thuộc tính** → dùng skill `set-property` (đơn giản hơn).

---

## Hướng dẫn thực hiện

### Bước 1: Tạo Dictionary chứa các properties

Tạo `Dictionary<string, object>` với các cặp key-value. **KHÔNG** dùng key trùng auto-injected fields.

### Bước 2: Gọi SetCommonProperties

```csharp
DatabucketsTracker.SetCommonProperties(dict);
```

**Tham số:**
- `dict` (Dictionary\<string, object\>): Các cặp key-value

Sau khi gọi, tất cả properties tự động gắn vào **mọi event** tiếp theo.

---

## Ví dụ

### Ví dụ 1: Sau khi login

```csharp
DatabucketsTracker.SetCommonProperties(new Dictionary<string, object>
{
    ["user_level"] = 1,
    ["game_mode"] = "story",
    ["user_type"] = "free"
});
```

### Ví dụ 2: Khi user nâng cấp premium

```csharp
DatabucketsTracker.SetCommonProperties(new Dictionary<string, object>
{
    ["user_type"] = "premium",
    ["subscription_plan"] = "monthly",
    ["premium_since"] = "2026-03-24"
});
```

---

## Best Practices

- ✅ Dùng khi cần set nhiều properties cùng lúc (sau login, load user profile)
- ✅ Hiệu quả hơn gọi `SetCommonProperty()` nhiều lần liên tiếp
- ✅ Properties tự động gắn vào mọi event tiếp theo
- ❌ KHÔNG truyền dictionary rỗng
- ❌ KHÔNG dùng key trùng auto-injected fields (`user_pseudo_id`, `session_id`...)

---

## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: Gọi SetCommonProperty nhiều lần thay vì dùng batch

```csharp
// SAI — code dài, kém hiệu quả
DatabucketsTracker.SetCommonProperty("user_level", 1);
DatabucketsTracker.SetCommonProperty("game_mode", "story");
DatabucketsTracker.SetCommonProperty("user_type", "free");
DatabucketsTracker.SetCommonProperty("country", "VN");
DatabucketsTracker.SetCommonProperty("language", "vi");
```
**Tại sao sai:** 5 lần gọi API thay vì 1 lần.
**Cách đúng:**
```csharp
DatabucketsTracker.SetCommonProperties(new Dictionary<string, object>
{
    ["user_level"] = 1,
    ["game_mode"] = "story",
    ["user_type"] = "free",
    ["country"] = "VN",
    ["language"] = "vi"
});
```

### ❌ Anti-Pattern 2: Dùng key trùng auto-injected fields

```csharp
// SAI — "user_pseudo_id" là auto-injected field
DatabucketsTracker.SetCommonProperties(new Dictionary<string, object>
{
    ["user_pseudo_id"] = "custom-id" // Bị SDK ghi đè
});
```
**Tại sao sai:** Auto-injected fields bị SDK ghi đè, giá trị custom sẽ mất.
**Cách đúng:** Dùng key khác, ví dụ `custom_user_id`.

---

## Skill liên quan

- `databuckets-skills/set-property` — Dùng khi chỉ cần set **1** property (thay thế)
- `databuckets-skills/record` — Properties sẽ tự động gắn vào event khi Record
- `databuckets-skills/init` — Khởi tạo SDK
