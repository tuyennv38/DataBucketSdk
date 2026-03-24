---
name: end-session
description: "Hướng dẫn gọi DatabucketsTracker.ForceEndCurrentSession() để kết thúc session thủ công."
version: "1.0.0"
author: "Tuyen"
category: sdk
risk: moderate
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, databuckets, session]
---

<!-- @implements task:create-skill-end-session-0007 -->
<!-- @implements feature:skill-end-session-0006 -->

# Databuckets End Session — Kết thúc Session thủ công

> **Mục tiêu:** Hướng dẫn gọi `ForceEndCurrentSession()` để kết thúc session thủ công trong các trường hợp đặc biệt.

`ForceEndCurrentSession()` kết thúc session hiện tại. Session mới tự động tạo khi có event tiếp theo. **Chỉ dùng khi đặc biệt** — SDK tự quản lý session trong điều kiện bình thường.

---

## Khi nào sử dụng

- Khi user **logout** khỏi tài khoản.
- Khi user **chuyển đổi tài khoản** (switch account).
- Khi cần phân tách session rõ ràng giữa 2 người dùng khác nhau.

## Khi KHÔNG sử dụng

- Khi user chỉ đổi màn hình/scene → SDK tự quản lý session.
- Khi app bị minimize/đóng → SDK tự xử lý `app_focus_end`.
- Trong hầu hết các trường hợp bình thường → **KHÔNG cần gọi API này**.

---

## Hướng dẫn thực hiện

### Bước 1: Ghi nhận event trước khi kết thúc

Gọi `Record()` để ghi nhận lý do kết thúc session trước khi gọi `ForceEndCurrentSession()`.

### Bước 2: Gọi ForceEndCurrentSession

```csharp
DatabucketsTracker.ForceEndCurrentSession();
```

**Tham số:** Không có.

Session hiện tại kết thúc ngay. Session mới tự động tạo khi có event tiếp theo.

---

## Ví dụ

### Ví dụ 1: User logout

```csharp
// Ghi event trước khi kết thúc session
DatabucketsTracker.Record("user_logout", null);
DatabucketsTracker.ForceEndCurrentSession();
```

### Ví dụ 2: Chuyển đổi tài khoản

```csharp
// Ghi event switch account
DatabucketsTracker.Record("account_switched", new Dictionary<string, object>
{
    ["from_account"] = "user_A",
    ["to_account"] = "user_B"
});
DatabucketsTracker.ForceEndCurrentSession();

// Event tiếp theo sẽ tự động tạo session mới
DatabucketsTracker.Record("user_login", new Dictionary<string, object>
{
    ["account"] = "user_B"
});
```

---

## Best Practices

- ✅ Chỉ dùng khi **đặc biệt**: user logout, chuyển đổi tài khoản
- ✅ Nên gọi `Record()` trước để ghi nhận lý do kết thúc session
- ✅ Session mới tự động tạo khi có event tiếp theo
- ❌ KHÔNG dùng thường xuyên — SDK tự quản lý session
- ❌ KHÔNG gọi mỗi khi đổi scene/màn hình

---

## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: Gọi mỗi khi đổi màn hình

```csharp
// SAI — phá vỡ session tracking
void OnSceneChanged()
{
    DatabucketsTracker.ForceEndCurrentSession();
}
```
**Tại sao sai:** Session phải liên tục trong suốt phiên dùng app. Cắt session mỗi scene → dữ liệu analytics sai lệch.
**Cách đúng:** Chỉ gọi khi logout hoặc switch account.

### ❌ Anti-Pattern 2: Kết thúc session mà không Record event trước

```csharp
// SAI — mất context
DatabucketsTracker.ForceEndCurrentSession();
```
**Tại sao sai:** Không biết tại sao session kết thúc, khó phân tích sau.
**Cách đúng:** Gọi `Record("user_logout", null)` trước.

---

## Skill liên quan

- `databuckets-skills/record` — Ghi event trước khi kết thúc session
- `databuckets-skills/init` — Khởi tạo SDK
