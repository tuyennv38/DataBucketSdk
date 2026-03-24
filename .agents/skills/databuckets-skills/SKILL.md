---
name: databuckets-skills
description: "Tổng quan Databuckets Unity SDK v1.0.6 — hướng dẫn chọn skill con phù hợp cho từng use case."
version: "1.0.0"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, databuckets]
tools: [claude, cursor, gemini]
---

<!-- @implements task:create-skill-overview-0001 -->

# Databuckets SDK Skills — Tổng quan và Hướng dẫn chọn Skill

> **Mục tiêu:** Giúp AI agent hiểu tổng quan Databuckets Unity SDK v1.0.6 và chọn đúng skill con cho từng yêu cầu.

Databuckets là nền tảng analytics tự động thu thập event, session, và app lifecycle trong Unity. SDK cung cấp 7 API chính, mỗi API có skill riêng hướng dẫn chi tiết.

---

## Khi nào sử dụng

- Khi cần tổng quan về Databuckets SDK trước khi gọi API cụ thể.
- Khi không biết nên dùng skill nào cho use case hiện tại.
- Khi cần tra cứu auto-injected fields hoặc PlayerPrefs keys.

## Khi KHÔNG sử dụng

- Khi đã biết chính xác API cần dùng → đọc trực tiếp skill con tương ứng.
- Khi triển khai API không thuộc Databuckets SDK.

---

## Hướng dẫn chọn Skill con

| Tôi muốn... | Dùng Skill |
|-------------|------------|
| Khởi tạo SDK lần đầu | `databuckets-skills/init` |
| Ghi nhận business event | `databuckets-skills/record` |
| Đo thời gian giữa 2 events | `databuckets-skills/record-timing` |
| Set 1 thuộc tính chung cho mọi event | `databuckets-skills/set-property` |
| Set nhiều thuộc tính chung cùng lúc | `databuckets-skills/set-properties` |
| Kết thúc session thủ công | `databuckets-skills/end-session` |
| Bật/tắt theo dõi Unity exceptions | `databuckets-skills/exception-tracking` |

---

## SDK tự động làm gì (KHÔNG cần code)

### Auto-Tracking Events

| Event | Khi nào gửi |
|-------|-------------|
| `first_open` | Mở app lần đầu trong 10 phút sau cài |
| `session_start` | Session mới: app launch, restart, timeout, resume |
| `app_focus_start` | App được focus |
| `app_focus_end` | App mất focus, minimize, crash, quit |
| `app_exception_log` | Unity LogType.Exception (khi bật ExceptionLogTracking) |

### Auto-Injected Fields (gắn tự động vào MỌI event)

| Nhóm | Trường | Mô tả |
|------|--------|-------|
| Time | `event_date`, `event_local_day_of_week`, `event_local_hour`, `event_local_hour_minute`, `install_day` | Thời gian event |
| Retention | `retention_day`, `retention_hour`, `retention_minute` | Thời gian giữ chân |
| Session | `session_id`, `session_progress` | Thông tin session |
| SDK | `sdk_ver` | Phiên bản SDK |
| User | `user_pseudo_id` | Anonymous user ID |
| Geo | `geo.country`, `geo.loc`, `geo.city`, `geo.org`, `geo.timezone`, `geo.postal`, `geo.region` | Vị trí |
| Device | `platform`, `app_store`, `app_version`, `device.platform_version`, `device.user_default_language`, `device.model`, `device.timezone_offset_seconds`, `device.brand_name`, `device.category` | Thiết bị |
| App | `app_id` | Application ID |

### PlayerPrefs Keys (KHÔNG xóa)

| Key | Mô tả |
|-----|-------|
| `is_sent_first_open` | Đánh dấu đã gửi first_open |
| `session_number` | Số session hiện tại |
| `user_pseudo_id` | Anonymous user ID |
| `current_session_start` | Thời điểm bắt đầu session |
| `last_event_timestamp` | Timestamp event cuối cùng |

> ⚠️ Xóa các PlayerPrefs keys này sẽ làm sai lệch dữ liệu analytics.

---

## Skill liên quan

- `databuckets-skills/init` — Khởi tạo SDK (bắt buộc gọi đầu tiên)
- `databuckets-skills/record` — Ghi nhận event
- `databuckets-skills/record-timing` — Đo thời gian giữa 2 events
- `databuckets-skills/set-property` — Set 1 common property
- `databuckets-skills/set-properties` — Set nhiều common properties
- `databuckets-skills/end-session` — Kết thúc session thủ công
- `databuckets-skills/exception-tracking` — Bật/tắt exception tracking
