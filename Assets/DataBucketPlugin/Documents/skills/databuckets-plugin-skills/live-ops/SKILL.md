---
name: live-ops
description: "Hướng dẫn sử dụng DataBucketLiveOps để track feature_first_show, feature_open, feature_close."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, live-ops, feature]
---

# DataBucketLiveOps — Live Ops Feature Tracking

> **Mục tiêu:** Track vòng đời tính năng live ops: first_show → open → close.

---

## Khi nào sử dụng

- Khi feature (daily reward, lucky wheel...) show lần đầu.
- Khi user mở hoặc đóng feature.

## Khi KHÔNG sử dụng

- Khi track IAP trong feature → dùng `databuckets-plugin-skills/iap`.
- Khi track resource earn từ feature → dùng `databuckets-plugin-skills/resource`.

---

## Hướng dẫn thực hiện

### FeatureFirstShow — Show lần đầu

```csharp
DataBucketLiveOps.FeatureFirstShow(string featureName, string placement = null);
// Không log với features mặc định show từ đầu
```

### FeatureOpen — User mở feature

```csharp
DataBucketLiveOps.FeatureOpen(string featureName, string placement = null, string openType = null, int? openIndex = null);
// openType: "popup" (tự động) | "click" (user bấm)
// openIndex: lần thứ mấy mở (>=1)
```

### FeatureClose — User đóng feature

```csharp
DataBucketLiveOps.FeatureClose(string featureName, string placement = null, int? openIndex = null, long? durationFeature = null);
// durationFeature: msec (>0)
```

---

## Ví dụ

### Ví dụ 1: Daily Reward flow

```csharp
// Icon daily reward xuất hiện lần đầu
DataBucketLiveOps.FeatureFirstShow("daily_reward", "home_icon");

// User mở daily reward
DataBucketLiveOps.FeatureOpen("daily_reward", placement: "home_icon", openType: "click", openIndex: 1);

// User đóng sau 5 giây
DataBucketLiveOps.FeatureClose("daily_reward", placement: "home_icon", openIndex: 1, durationFeature: 5000);
```

### Ví dụ 2: Lucky Wheel popup tự động

```csharp
DataBucketLiveOps.FeatureOpen("lucky_wheel", placement: "home_popup", openType: "popup", openIndex: 3);
DataBucketLiveOps.FeatureClose("lucky_wheel", placement: "home_popup", openIndex: 3, durationFeature: 12000);
```

---

## Best Practices

- ✅ `FeatureFirstShow` chỉ log **1 lần** — khi feature xuất hiện lần đầu
- ✅ `openIndex` tăng dần mỗi lần user mở cùng loại feature
- ✅ Dùng `durationFeature` để phân tích engagement với feature
- ❌ KHÔNG log FeatureFirstShow cho features mặc định hiển thị từ đầu

---

## Skill liên quan

- `databuckets-plugin-skills/resource` — Track resource earn từ feature
- `databuckets-plugin-skills/iap` — Track IAP show từ feature
