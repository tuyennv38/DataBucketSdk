---
name: metrics
description: "Hướng dẫn sử dụng DataBucketMetrics để track tutorial, button click, screen show/exit."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, metrics, ux]
---

# DataBucketMetrics — Other Metrics

> **Mục tiêu:** Track các metrics phụ: tutorial, button click, screen navigation, và app exit.

---

## Khi nào sử dụng

- Khi user thực hiện action trong tutorial.
- Khi user bấm button quan trọng (không có event riêng).
- Khi user đến một màn mới hoặc thoát game.

## Khi KHÔNG sử dụng

- Khi button đã có event riêng (IAP, Ad...) → dùng skill tương ứng.
- Khi screen đã có event riêng (iap_show, ad_impression) → không cần ScreenShow.

---

## Hướng dẫn thực hiện

### TutorialAction

```csharp
DataBucketMetrics.TutorialAction(string actionName, int actionIndex, string actionCate = null);
// actionName: "start" | "finish" | "click_1" | "use_booster_A"
// actionIndex: thứ tự (>=0, 0=start)
```

### ButtonClick

```csharp
DataBucketMetrics.ButtonClick(string buttonName, string screenName);
```

### ScreenShow

```csharp
DataBucketMetrics.ScreenShow(string screenName, string buttonName = null, string prevScreenName = null, long? durationPrevScreen = null);
```

### ScreenExit

```csharp
DataBucketMetrics.ScreenExit(string prevScreenName, long? durationPrevScreen = null);
```

---

## Ví dụ

### Ví dụ 1: Tutorial flow

```csharp
DataBucketMetrics.TutorialAction("start", 0);
DataBucketMetrics.TutorialAction("click_1", 1, actionCate: "click");
DataBucketMetrics.TutorialAction("use_booster_A", 2, actionCate: "use_booster");
DataBucketMetrics.TutorialAction("finish", 10);
```

### Ví dụ 2: User bấm Quit khi thua

```csharp
DataBucketMetrics.ButtonClick("Quit", "lose_confirm");
```

### Ví dụ 3: Screen navigation và exit

```csharp
DataBucketMetrics.ScreenShow("setting", buttonName: "Setting", prevScreenName: "home", durationPrevScreen: 30000);
DataBucketMetrics.ScreenExit("setting", durationPrevScreen: 5000);
```

---

## Best Practices

- ✅ Tutorial: "start" ở index 0, "finish" ở index cuối
- ✅ ButtonClick chỉ cho button **quan trọng** cần phân tích
- ✅ ScreenShow không trigger với màn đã có event riêng
- ❌ KHÔNG log mọi button click — chỉ log button cần phân tích hành vi

---

## Skill liên quan

- `databuckets-plugin-skills/level` — Level events thường đi kèm screen navigation
