---
name: notification
description: "Hướng dẫn sử dụng DataBucketNotification để track noti_send, noti_receive, noti_open."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, notification]
---

# DataBucketNotification — Notification Tracking

> **Mục tiêu:** Track vòng đời notification: send → receive → open.

---

## Khi nào sử dụng

- Khi game gửi, user nhận, hoặc user mở notification.

## Khi KHÔNG sử dụng

- Khi track hành vi sau khi mở noti (ví dụ: mua IAP) → dùng skill tương ứng.

---

## Hướng dẫn thực hiện

### Send — Game gửi noti

```csharp
DataBucketNotification.Send(string notiCate, string notiName);
// notiCate: "remind" | "event" | "daily_quest"
// notiName: headline hoặc tên tóm gọn nội dung
```

### Receive — User nhận noti

```csharp
DataBucketNotification.Receive(string notiCate, string notiName);
```

### Open — User mở noti

```csharp
DataBucketNotification.Open(string notiCate, string notiName);
```

---

## Ví dụ

### Ví dụ 1: Remind notification

```csharp
DataBucketNotification.Send("remind", "Come back and play!");
// ... user nhận noti
DataBucketNotification.Receive("remind", "Come back and play!");
// ... user mở noti
DataBucketNotification.Open("remind", "Come back and play!");
```

### Ví dụ 2: Event notification

```csharp
DataBucketNotification.Send("event", "Double reward event starts now!");
```

---

## Best Practices

- ✅ `notiCate` dùng nhất quán cho cùng loại notification
- ✅ `notiName` nên tóm gọn, dùng headline để phân biệt
- ❌ KHÔNG log Send cho system notifications (OS-level)

---

## Skill liên quan

- `databuckets-plugin-skills` — Parent skill
