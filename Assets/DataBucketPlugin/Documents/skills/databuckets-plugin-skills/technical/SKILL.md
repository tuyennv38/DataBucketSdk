---
name: technical
description: "Hướng dẫn sử dụng DataBucketTechnical để track loading_start và loading_finish."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, technical, performance]
---

# DataBucketTechnical — Technical Performance

> **Mục tiêu:** Track hiệu năng tải dữ liệu (API, CDN, SDK) — đo time to data và success rate.

**KPI:** Time to data, Success rate.

---

## Khi nào sử dụng

- Khi app bắt đầu gọi dữ liệu (API, CDN, SDK, store, remote_config).
- Khi nhận kết quả request (success, fail, timeout, offline).

## Khi KHÔNG sử dụng

- Khi track ad request → dùng `databuckets-plugin-skills/ad`.
- Khi track business events → dùng class tương ứng.

---

## Hướng dẫn thực hiện

### LoadingStart — Bắt đầu tải

```csharp
DataBucketTechnical.LoadingStart(
    string dataSource,       // "api" | "cdn" | "sdk" | "store" | "remote_config"
    string resourceType,     // "feed" | "content_detail" | "search" | "user_profile" | "paywall"
    string loadContext,      // "app_open" | "home" | "content_detail"
    string triggerSource = null,  // "screen_appear" | "user_action" | "background" | "prefetch"
    string loadId = null,    // UUID (cần nếu nhiều request song song)
    string priority = null   // "critical" | "high" | "normal" | "low" | "prefetch"
);
```

### LoadingFinish — Kết thúc tải

```csharp
DataBucketTechnical.LoadingFinish(
    string dataSource, string resourceType, string loadContext,
    string result,           // "success" | "fail" | "canceled" | "timeout" | "offline"
    long loadTime,           // msec (>=0)
    string triggerSource = null, string loadId = null, string priority = null,
    string errorCode = null,     // "network_timeout" | "server_error" | "http_404_not_found"
    long? responseBytes = null,  // kích thước response
    int? retryCount = null       // 0=thành công lần đầu
);
```

---

## Ví dụ

### Ví dụ 1: API call thành công

```csharp
DataBucketTechnical.LoadingStart("api", "feed", "home", triggerSource: "screen_appear");

// ... API response sau 350ms
DataBucketTechnical.LoadingFinish("api", "feed", "home", "success", 350, triggerSource: "screen_appear", responseBytes: 15360);
```

### Ví dụ 2: CDN timeout

```csharp
var loadId = System.Guid.NewGuid().ToString();
DataBucketTechnical.LoadingStart("cdn", "content_detail", "content_detail", loadId: loadId, priority: "high");

// ... timeout sau 10 giây
DataBucketTechnical.LoadingFinish("cdn", "content_detail", "content_detail", "timeout", 10000, loadId: loadId, priority: "high", errorCode: "network_timeout", retryCount: 3);
```

### Ví dụ 3: Prefetch trong background

```csharp
DataBucketTechnical.LoadingStart("api", "feed", "home", triggerSource: "prefetch", priority: "low");
DataBucketTechnical.LoadingFinish("api", "feed", "home", "success", 800, triggerSource: "prefetch", priority: "low");
```

---

## Best Practices

- ✅ Dùng `loadId` khi có nhiều request song song — giúp match start/finish
- ✅ `loadTime` tính từ start đến finish bao gồm retry time
- ✅ `retryCount=0` nghĩa là thành công lần đầu
- ✅ `errorCode` format: `{category}_{detail}` (VD: `http_404_not_found`)
- ❌ KHÔNG log cho mọi API call — chỉ log API quan trọng cần đo performance

---

## Skill liên quan

- `databuckets-plugin-skills` — Parent skill
