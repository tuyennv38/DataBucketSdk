---
name: user-properties
description: "Hướng dẫn sử dụng DataBucketUserProperties để set User Properties trong DataBucketPlugin."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, user-properties]
---

# DataBucketUserProperties — Set User Properties

> **Mục tiêu:** Hướng dẫn sử dụng `DataBucketUserProperties` để set các thuộc tính user-level, tự động đính kèm vào mọi event.

User Properties là thuộc tính mô tả đặc điểm/trạng thái user, tồn tại ở cấp độ người dùng. Hệ thống tự động đính kèm vào mọi event khi được ghi nhận.

---

## Khi nào sử dụng

- Khi cần set current_level, active_day, connection_type cho user.
- Khi cần set UA attribution data (network, campaign, adgroup, creative).
- Khi cần set resource balance (coin, ticket, freeze...).
- Khi cần set IAP user status hoặc Firebase A/B test variants.

## Khi KHÔNG sử dụng

- Khi cần ghi event cụ thể (level_start, iap_show...) → dùng class tương ứng.
- Khi cần set thuộc tính custom không có trong Data Tracking Plan → dùng `DataBucketWrapper.SetCommonProperty()`.

---

## Hướng dẫn thực hiện

### Bước 1: Đảm bảo Init

Gọi `DataBucketWrapper.Init()` trước khi dùng bất kỳ method nào.

### Bước 2: Gọi setter tương ứng

Tất cả methods đều static, gọi trực tiếp `DataBucketUserProperties.<Method>()`.

**Danh sách methods:**

| Method | Param | Type | Mô tả |
|--------|-------|------|-------|
| `SetCurrentLevel(level)` | level | int | Level hiện tại (>=0) |
| `SetUaAttribution(...)` | network, campaign, adgroup, creative, trackerName | string (nullable) | UA attribution từ MMP |
| `SetFirebaseExperiment(experiments)` | experiments | string[] | Firebase A/B test variants |
| `SetResourceBalance(resourceName, balance)` | resourceName, balance | string, int | Số resource hiện có (>=0) |
| `SetUserId(userId)` | userId | string | Client-generated UUID |
| `SetCurrentMode(mode)` | mode | string | Mode chơi: "normal", "challenge", "endless" |
| `SetCurrentEvent(eventName)` | eventName | string | Live ops event đang tham gia |
| `SetIsIapUser(isIapUser)` | isIapUser | bool | true sau lần mua IAP đầu tiên |
| `SetIapCount(count)` | count | int | Số lần mua IAP (>=0) |
| `SetActiveDay(day)` | day | int | Số ngày active (>=0) |
| `SetConnectionType(type)` | type | string | "offline", "wifi", "mobile_data", "unknown" |
| `SetWinStreak(streak)` | streak | int | Chuỗi thắng liên tiếp (>=0) |
| `SetLoseStreak(streak)` | streak | int | Chuỗi thua liên tiếp (>=0) |

---

## Ví dụ

### Ví dụ 1: Set properties khi user bắt đầu chơi (D0)

```csharp
using DataBucketPlugin;

// Sau khi Init SDK
DataBucketUserProperties.SetCurrentLevel(1);
DataBucketUserProperties.SetActiveDay(0);
DataBucketUserProperties.SetIsIapUser(false);
DataBucketUserProperties.SetConnectionType("wifi");
DataBucketUserProperties.SetResourceBalance("coin", 100);
DataBucketUserProperties.SetResourceBalance("ticket", 5);
```

### Ví dụ 2: Set UA attribution từ MMP callback

```csharp
DataBucketUserProperties.SetUaAttribution(
    network: "Facebook",
    campaign: "ROAS_US_Q1",
    adgroup: "lookalike_1pct",
    creative: "video_gameplay_30s"
);
```

### Ví dụ 3: Cập nhật sau khi user win level

```csharp
// Sau khi user win level 5
DataBucketUserProperties.SetCurrentLevel(6);  // Level TIẾP THEO
DataBucketUserProperties.SetWinStreak(3);
DataBucketUserProperties.SetLoseStreak(0);     // Reset lose streak
```

---

## Best Practices

- ✅ Gọi `SetCurrentLevel()` vào thời điểm user **bắt đầu chơi level sau**, không phải lúc hoàn thành level trước
- ✅ `SetActiveDay()` chỉ tính những ngày user thực sự mở game
- ✅ Set `SetIsIapUser(true)` **1 lần** sau lần mua IAP đầu tiên
- ✅ Firebase experiment: chỉ cần log khi bắn data trực tiếp lên Databuckets
- ❌ KHÔNG gọi trước `DataBucketWrapper.Init()`
- ❌ KHÔNG dùng `SetResourceBalance()` cho resource không quan trọng — chỉ track resource cần phân tích

---

## Skill liên quan

- `databuckets-plugin-skills` — Parent skill, overview
- `databuckets-plugin-skills/level` — Level events (cập nhật current_level sau level_end)
- `databuckets-skills/set-property` — Raw SetCommonProperty API
