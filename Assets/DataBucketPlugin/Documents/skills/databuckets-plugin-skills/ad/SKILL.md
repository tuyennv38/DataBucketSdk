---
name: ad
description: "Hướng dẫn sử dụng DataBucketAd để track ad request, impression, click, complete."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, ad, monetization]
---

# DataBucketAd — In-App Advertising

> **Mục tiêu:** Track vòng đời quảng cáo: request → impression → click → complete.

**KPI:** Đánh giá chất lượng kịch bản load/show quảng cáo, hiệu quả ad revenue.

---

## Khi nào sử dụng

- Khi load quảng cáo (request) và cần đo load time, success rate.
- Khi show quảng cáo (impression) và cần track ad value.
- Khi user click hoặc xem xong quảng cáo.

## Khi KHÔNG sử dụng

- Khi track IAP → dùng `databuckets-plugin-skills/iap`.

---

## Hướng dẫn thực hiện

### Request — Load quảng cáo

```csharp
DataBucketAd.Request(string adFormat, string adPlatform, string adNetwork, string placement, int isLoad, long loadTime, string adUnitId = null);
// isLoad: 1=Success, 0=Fail
// loadTime: msec (>=0)
```

### Impression — Show quảng cáo

```csharp
DataBucketAd.Impression(string adFormat, string adPlatform, string adNetwork, string placement, double value, string adUnitId = null, int? isShow = null);
// value: giá trị tiền (>0), từ mediation/Firebase
```

### Click — User click QC

```csharp
DataBucketAd.Click(string adFormat, string adPlatform, string adNetwork, string placement, string adUnitId = null);
```

### Complete — QC kết thúc

```csharp
DataBucketAd.Complete(string adFormat, string adPlatform, string adNetwork, string placement, string adUnitId = null, string endType = null, long? durationAd = null);
// endType (chỉ rewarded): "quit" | "done"
```

---

## Ví dụ

### Ví dụ 1: Rewarded video — user xem hết và nhận thưởng

```csharp
// 1. Request
DataBucketAd.Request("video_rewarded", "Admob", "Admob", "buy_booster", isLoad: 1, loadTime: 1200);

// 2. Impression
DataBucketAd.Impression("video_rewarded", "Admob", "Admob", "buy_booster", value: 0.05);

// 3. Complete — user xem hết
DataBucketAd.Complete("video_rewarded", "Admob", "Admob", "buy_booster", endType: "done", durationAd: 30000);
```

### Ví dụ 2: Interstitial — load fail

```csharp
DataBucketAd.Request("interstitial", "Max", "Mintegral", "level_end", isLoad: 0, loadTime: 5000);
// Không có Impression/Complete vì load fail
```

### Ví dụ 3: Banner impression

```csharp
DataBucketAd.Impression("banner", "Admob", "Admob", "home", value: 0.001);
```

---

## Best Practices

- ✅ `Request` bắn lên **khi kết thúc** request, không phải lúc bắt đầu
- ✅ `value` trong Impression lấy từ mediation/Firebase callback
- ✅ `endType` chỉ áp dụng cho rewarded — không cần cho interstitial
- ❌ KHÔNG log Impression nếu load fail (`isLoad=0`)

---

## Skill liên quan

- `databuckets-plugin-skills/resource` — Earn resource sau rewarded ad
- `databuckets-plugin-skills/level` — Ad thường show tại level_end
