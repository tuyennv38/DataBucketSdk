---
name: iap
description: "Hướng dẫn sử dụng DataBucketIAP để track IAP flow: show, click, purchase, close."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, iap, monetization]
---

# DataBucketIAP — In-App Purchase Tracking

> **Mục tiêu:** Track toàn bộ IAP flow từ show → click → purchase success/fail → close.

**KPI:** Tỉ lệ purchase/show, click/show của từng gói IAP tại từng vị trí.

---

## Khi nào sử dụng

- Khi hiển thị màn/banner/popup IAP cho user.
- Khi user click mua, mua thành công, mua thất bại, hoặc đóng màn IAP.

## Khi KHÔNG sử dụng

- Khi track resource nhận từ IAP → dùng `databuckets-plugin-skills/resource`.
- Khi track quảng cáo → dùng `databuckets-plugin-skills/ad`.

---

## Hướng dẫn thực hiện

### Show — Hiển thị IAP

```csharp
DataBucketIAP.Show(string placement, string showType, string triggerType, string[] packName);
// showType: "shop" (cả shop) | "pack" (riêng gói)
// triggerType: "popup" (tự show) | "click" (user bấm)
// packName: ["null"] nếu shop, ["removeads","starterpack"] nếu nhiều gói
```

### Click — User click mua

```csharp
DataBucketIAP.Click(string placement, string showType, string triggerType, string packName);
```

### PurchaseSuccess — Mua thành công

```csharp
DataBucketIAP.PurchaseSuccess(string placement, string showType, string triggerType, string packName, double price, string currency);
```

### PurchaseFailed — Mua thất bại

```csharp
DataBucketIAP.PurchaseFailed(string placement, string triggerType, string packName, double price, string currency, string failReason, string errorCode = null);
```

### Close — Đóng màn IAP

```csharp
DataBucketIAP.Close(string placement, string showType, string triggerType, string packName, long durationIap);
```

---

## Ví dụ

### Ví dụ 1: Full IAP flow — user mua thành công

```csharp
// 1. Show shop
DataBucketIAP.Show("home_shop", "shop", "click", new[] { "null" });

// 2. User click mua starter pack
DataBucketIAP.Click("home_shop", "shop", "click", "starterpack");

// 3. Mua thành công
DataBucketIAP.PurchaseSuccess("home_shop", "shop", "click", "starterpack", 4.99, "USD");

// 4. User đóng shop (ở shop 15 giây)
DataBucketIAP.Close("home_shop", "shop", "click", "null", 15000);
```

### Ví dụ 2: Popup show riêng 1 gói, user cancel

```csharp
DataBucketIAP.Show("home_popup", "pack", "popup", new[] { "removeads" });
DataBucketIAP.Click("home_popup", "pack", "popup", "removeads");
DataBucketIAP.PurchaseFailed("home_popup", "popup", "removeads", 2.99, "USD", "user_cancelled");
DataBucketIAP.Close("home_popup", "pack", "popup", "removeads", 8000);
```

### Ví dụ 3: Show nhiều gói cùng lúc

```csharp
DataBucketIAP.Show("ingame_popup", "pack", "popup", new[] { "doublepack", "starterpack", "bundle1" });
```

---

## Best Practices

- ✅ Luôn log Show trước Click, Click trước Purchase
- ✅ `packName` trong Show là **array** (có thể nhiều gói), trong Click/Purchase là **string** (1 gói)
- ✅ Log Close để tính `durationIap` cho IAP prediction
- ❌ KHÔNG log PurchaseSuccess cho server-validated purchase — đây là client-side

---

## Skill liên quan

- `databuckets-plugin-skills/resource` — Earn resource sau purchase
- `databuckets-plugin-skills/user-properties` — Update is_iap_user, iap_count
