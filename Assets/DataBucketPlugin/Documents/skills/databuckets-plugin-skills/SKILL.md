---
name: databuckets-plugin-skills
description: "Tổng quan DataBucketPlugin v1.0.1 — hướng dẫn chọn skill con cho từng tracking class."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, databuckets, tracking]
tools: [claude, cursor, gemini]
---

# DataBucket Plugin Skills — Tổng quan và Hướng dẫn chọn Skill

> **Mục tiêu:** Giúp AI agent và developer chọn đúng skill con cho từng use case tracking trong DataBucketPlugin.

DataBucketPlugin v1.0.1 cung cấp 9 static classes (typed methods) bọc `DataBucketWrapper.Record()`. Mỗi class có 1 skill riêng hướng dẫn chi tiết cách sử dụng.

---

## Khi nào sử dụng

- Khi cần tổng quan về các tracking classes trước khi gọi method cụ thể.
- Khi không biết nên dùng class/skill nào cho use case hiện tại.
- Khi cần tra cứu mapping giữa event name và class.

## Khi KHÔNG sử dụng

- Khi đã biết chính xác class cần dùng → đọc trực tiếp skill con.
- Khi cần API raw (Init, Record, SetProperty) → dùng `databuckets-skills`.

---

## Hướng dẫn chọn Skill con

| Tôi muốn... | Dùng Class | Dùng Skill |
|-------------|------------|------------|
| Set user properties (current_level, ua_network...) | `DataBucketUserProperties` | `databuckets-plugin-skills/user-properties` |
| Track level start/end/exit/reopen | `DataBucketLevel` | `databuckets-plugin-skills/level` |
| Track resource earn/spend | `DataBucketResource` | `databuckets-plugin-skills/resource` |
| Track IAP show/click/purchase/close | `DataBucketIAP` | `databuckets-plugin-skills/iap` |
| Track ad request/impression/click/complete | `DataBucketAd` | `databuckets-plugin-skills/ad` |
| Track notification send/receive/open | `DataBucketNotification` | `databuckets-plugin-skills/notification` |
| Track live ops feature show/open/close | `DataBucketLiveOps` | `databuckets-plugin-skills/live-ops` |
| Track tutorial, button click, screen show/exit | `DataBucketMetrics` | `databuckets-plugin-skills/metrics` |
| Track loading start/finish performance | `DataBucketTechnical` | `databuckets-plugin-skills/technical` |

---

## Kiến trúc

```
Game Code → Tracking Classes (Typed Methods) → DataBucketWrapper.Record() → Databuckets SDK
```

- Tất cả classes đều **static**, không cần instantiate.
- `DataBucketWrapper.Init()` phải được gọi trước khi dùng bất kỳ tracking class nào.
- Optional params mặc định `null` — không gửi nếu không truyền.

---

## Tài liệu liên quan

| Tài liệu | Mô tả |
|-----------|-------|
| `DATA_TRACKING_GUIDE.md` | Chi tiết Trigger, KPI, Value Requirement cho từng event |
| `README.md` | Hướng dẫn sử dụng tổng quan |
| `CHANGE_LOG.md` | Lịch sử thay đổi |

---

## Skill liên quan

- `databuckets-skills` — Raw SDK API (Init, Record, SetProperty...)
- `databuckets-plugin-skills/user-properties` — User Properties
- `databuckets-plugin-skills/level` — Level Analytics
- `databuckets-plugin-skills/resource` — Resource Earn/Spend
- `databuckets-plugin-skills/iap` — IAP Flow
- `databuckets-plugin-skills/ad` — In-App Advertising
- `databuckets-plugin-skills/notification` — Notifications
- `databuckets-plugin-skills/live-ops` — Live Ops Features
- `databuckets-plugin-skills/metrics` — Other Metrics
- `databuckets-plugin-skills/technical` — Technical Performance
