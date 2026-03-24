---
name: resource
description: "Hướng dẫn sử dụng DataBucketResource để track resource_earn và resource_spend."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, resource]
---

# DataBucketResource — Resource Earn/Spend

> **Mục tiêu:** Hướng dẫn track khi user nhận hoặc sử dụng tài nguyên (currency, item, booster).

---

## Khi nào sử dụng

- Khi user nhận resource (win level, daily reward, watch ads, purchase).
- Khi user tiêu resource (buy booster, exchange, use item).

## Khi KHÔNG sử dụng

- Khi track IAP purchase → dùng `databuckets-plugin-skills/iap`.
- Khi chỉ cần set resource balance hiện tại → dùng `DataBucketUserProperties.SetResourceBalance()`.

---

## Hướng dẫn thực hiện

### Method 1: Earn — User nhận resource

```csharp
DataBucketResource.Earn(
    string resourceType,      // "currency" | "item" | "booster" | "currency,booster"
    string resourceName,      // "gold" | "hammer" | "gold,hammer"
    object resourceAmount,    // int (1 loại) hoặc string "50,2" (nhiều loại)
    string placement,         // "level_win" | "daily_quest_reward"
    string reason,            // "reward" | "exchange" | "purchase" | "watch_ads"
    string placementDetail = null,
    object resourceBalance = null  // int hoặc string
);
```

### Method 2: Spend — User sử dụng resource

```csharp
DataBucketResource.Spend(
    string resourceType,
    string resourceName,
    object resourceAmount,
    string reason,            // "exchange" | "use"
    string placement,         // "buy_hammer" | "use_booster"
    string placementDetail = null,
    object resourceBalance = null
);
```

---

## Ví dụ

### Ví dụ 1: Earn 1 loại resource

```csharp
// User win level, nhận 50 gold
DataBucketResource.Earn("currency", "gold", 50, "level_win", "reward", resourceBalance: 5050);
```

### Ví dụ 2: Earn nhiều loại cùng lúc

```csharp
// User nhận daily reward: 100 gold + 2 hammer
DataBucketResource.Earn(
    "currency,booster", "gold,hammer", "100,2",
    "daily_quest_reward", "reward",
    resourceBalance: "5150,5"
);
```

### Ví dụ 3: Spend resource

```csharp
// User mua 1 hammer bằng 200 gold
DataBucketResource.Spend("currency", "gold", 200, "exchange", "buy_hammer", resourceBalance: 4950);
```

---

## Best Practices

- ✅ Nhiều resource cùng lúc: dùng format `"A,B"` (không dấu cách sau phẩy)
- ✅ Luôn truyền `resourceBalance` nếu có để phân tích economy
- ✅ `reason` phân biệt nguồn earn: "reward" vs "purchase" vs "watch_ads"
- ❌ KHÔNG truyền `resourceAmount` âm — luôn dùng số dương
- ❌ KHÔNG dùng Earn để track IAP (dùng `DataBucketIAP.PurchaseSuccess()`)

---

## Skill liên quan

- `databuckets-plugin-skills/level` — Level events trigger resource earn
- `databuckets-plugin-skills/iap` — IAP purchase cũng earn resource
- `databuckets-plugin-skills/user-properties` — Update resource balance
