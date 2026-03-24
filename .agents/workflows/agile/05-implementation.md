---
description: Bước 5 - Implementation - Code task, user test, commit, cập nhật backlog
---
// turbo-all
# ⚡ Bước 5: Implementation

> **Mục tiêu:** Code task → User test → Commit finish → Cập nhật Sprint Plan & Backlog → Lặp lại cho task tiếp theo.

---

## Tổng quan luồng mỗi task

```
⬜ To Do
 → 🔄 In Progress (Code)
   → 📢 Báo User Test
     → ✅ User Duyệt → Cập nhật Sprint Plan & Backlog → Commit Finish
       → Chuyển sang task tiếp theo
     → ❌ User Yêu Cầu Sửa → Sửa Code → Báo User Test Lại
```

---

## Bước 5.1: Coding

> **Input:** Sprint Plan (`docs/sprints/sprint-{N}/sprint-plan.md`)
>
> **🔓 TỰ ĐỘNG HOÀN TOÀN:** Trong quá trình coding, Agent tự động thực hiện MỌI thao tác (tạo file, sửa file, chạy lệnh, cài đặt package...) mà KHÔNG cần hỏi user. User đã đồng ý mặc định. Chỉ dừng lại ở Bước 5.2 (Báo User Test).

Với mỗi task trong Sprint Backlog:

1. **Cập nhật trạng thái** trong Sprint Plan: `⬜ To Do` → `🔄 In Progress`
2. **Viết code** triển khai task:
   - Thêm comment truy vết vào đầu file/class/module:
     ```csharp
     // @implements task:{name}-{XXXX}
     // @implements feature:{name}-{XXXX}
     ```
   - Tuân thủ coding standards trong System Design Document
   - Đảm bảo code build thành công
3. **Self-review:** Chạy build/check, kiểm tra nhanh kết quả

---

## Bước 5.2: Báo User Test → Chờ Duyệt

> **⚠️ CRITICAL:** User là người test. Agent KHÔNG tự test thay user.
> **KHÔNG được commit code trước khi user duyệt.**

**Cách thực hiện:**
```
1. Dùng notify_user để báo user:
   - Mô tả ngắn gọn những gì đã code
   - Hướng dẫn user cách test/kiểm tra
   - BlockedOnUser: true
2. Chờ user phản hồi:
   a. ✅ User duyệt (PASS) → Chuyển sang Bước 5.3
   b. ❌ User yêu cầu sửa → Sửa code → Quay lại Bước 5.2
```

---

## Bước 5.3: Commit Finish + Cập nhật Sprint Plan & Backlog

> **🔓 QUYỀN TỰ ĐỘNG:** User đã duyệt test. Agent tự động commit, KHÔNG cần hỏi. `SafeToAutoRun = true`.

**Trước khi commit, cập nhật tài liệu:**

1. **Sprint Plan** (`docs/sprints/sprint-{N}/sprint-plan.md`):
   - Đổi trạng thái task: `🔄 In Progress` → `✅ Done`
2. **Product Backlog** (`docs/backlog/product-backlog.md`):
   - Nếu **tất cả task** của PBI đã `✅ Done` → PBI: `✅ Done`
   - Nếu chưa xong hết → giữ `🔄 In Progress`

**Commit TẤT CẢ cùng nhau (code + docs):**

```powershell
git add <code-files> docs/sprints/sprint-{N}/sprint-plan.md docs/backlog/product-backlog.md
```

```powershell
git commit -m "<type>(<scope>): <mo ta ngan>" -m "- <thay doi 1>" -m "- Update sprint plan: mark task as Done" -m "ID: commit:<name>-<XXXX>" -m "Implements: task:<name>-<XXXX>" -m "Refs: feature:<name>-<XXXX>"
```

```powershell
git status --short
```

**Sau đó → Quay lại Bước 5.1 cho task tiếp theo trong Sprint.**

---

## Lưu ý

- **Atomic commits:** Task lớn có thể chia nhiều commit nhỏ. Mỗi commit phải build được.
- **Fix commits:** Commit sửa lỗi sau user test vẫn PHẢI có đầy đủ `Implements: task:...` và `Refs: feature:...`.
- **Tự động:** Commit code, copy file, xóa file → tự động. Chỉ hỏi user khi báo test.

---

## Checklist mỗi task

- [ ] Cập nhật trạng thái task → In Progress (Bước 5.1)
- [ ] Code xong và build thành công (Bước 5.1)
- [ ] Báo user test và được duyệt (Bước 5.2)
- [ ] Cập nhật Sprint Plan & Backlog (Bước 5.3)
- [ ] Commit finish thành công (Bước 5.3)
