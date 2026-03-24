---
description: Bước 1 - Thu thập yêu cầu, tạo PRD với Universal ID, và commit tài liệu
---
// turbo-all
# 📝 Bước 1: Tạo PRD (Product Requirements Document)

> **Mục tiêu:** Thu thập và làm rõ yêu cầu từ user → Tạo tài liệu PRD có gán Universal ID → User duyệt → Commit code.

---

## Tổng quan quy trình

```
User đưa yêu cầu → Agent hỏi làm rõ → Tạo PRD + Gán ID → User review → Điều chỉnh (nếu có) → User duyệt → Commit
```

---

## Bước 1.1: Thu thập và làm rõ yêu cầu

> **⚠️ HƯỚNG DẪN CHO AGENT:** KHÔNG được bắt tay vào viết PRD ngay. PHẢI hỏi user để làm rõ yêu cầu trước.

Khi user đưa ra yêu cầu ban đầu, agent **BẮT BUỘC** phải đặt câu hỏi làm rõ trước khi viết PRD. Các câu hỏi nên xoay quanh:

1. **Phạm vi (Scope):** Yêu cầu này bao gồm những gì? Có gì nằm ngoài phạm vi không?
2. **Đối tượng sử dụng (Target Users):** Ai là người dùng chính?
3. **Nền tảng / Tech stack:** Dự án sử dụng công nghệ gì? (nếu chưa rõ)
4. **Ràng buộc / Yêu cầu phi chức năng:** Hiệu năng, bảo mật, tương thích?
5. **Ưu tiên:** Tính năng nào quan trọng nhất? Có deadline không?
6. **Tham chiếu:** Có mockup, tài liệu, hoặc sản phẩm mẫu nào không?

> 💡 **Nguyên tắc:** Hỏi đúng, hỏi đủ, KHÔNG hỏi thừa. Gom các câu hỏi liên quan thành 1 lần hỏi. Tránh hỏi từng câu một.

**Cách thực hiện:**
```
1. Đọc yêu cầu ban đầu của user
2. Xác định những điểm chưa rõ
3. Gom tất cả câu hỏi vào 1 message duy nhất, đánh số rõ ràng
4. Dùng notify_user để gửi câu hỏi cho user
5. Chờ user trả lời
6. Nếu vẫn còn điểm chưa rõ → hỏi thêm (tối đa 2 lượt hỏi)
```

---

## Bước 1.2: Tạo PRD và gán Universal ID

> **⚠️ HƯỚNG DẪN CHO AGENT:** Sau khi đã có đủ thông tin, tạo PRD theo các bước dưới đây.

### 1.2.1: Chuẩn bị ID Convention

1. Đọc file `resources/ID-CONVENTION.md` từ skill `assign-universal-id`:
   ```
   Đường dẫn: .agents/skills/assign-universal-id/resources/ID-CONVENTION.md
   ```
2. Kiểm tra dự án đã có `docs/ID-CONVENTION.md` chưa:
   - **Chưa có** → Copy từ skill vào `docs/ID-CONVENTION.md`
   - **Đã có** → Dùng file của dự án (có thể đã tùy biến)

### 1.2.2: Viết PRD

1. Đọc template PRD từ skill:
   ```
   Đường dẫn: .agents/skills/assign-universal-id/resources/PRD-TEMPLATE.md
   ```
2. Tạo file PRD tại `docs/PRDs/PRD-XXX.md` (XXX = số thứ tự, ví dụ `PRD-001.md`)
3. Điền nội dung PRD dựa trên yêu cầu đã làm rõ ở Bước 1.1

### 1.2.3: Gán Universal ID

> **⚠️ HƯỚNG DẪN CHO AGENT:** Đọc và thực hiện đầy đủ **6 bước** trong skill `assign-universal-id` (file `.agents/skills/assign-universal-id/SKILL.md`).

Tóm tắt 6 bước:
1. **Đọc và phân tích** tài liệu PRD vừa viết
2. **Phân loại thành phần** — chọn đúng granularity (prd / feature / task)
3. **Gán ID** theo format `<doc-type>:<component-name-XXXX>`
3b. **Tạo Anchor** — mỗi ID khai báo lần đầu PHẢI có `<a id="..."></a>`
4. **Gán tham chiếu ngược (Implements)** — feature → prd, task → feature
5. **Tạo bảng tổng hợp (Phụ lục)** — cuối tài liệu, tất cả ID phải clickable
6. **Tự kiểm tra (Self-QA Checklist)**

---

## Bước 1.3: User review PRD

> **⚠️ HƯỚNG DẪN CHO AGENT:** Sau khi tạo xong PRD, PHẢI hỏi user duyệt.

**Cách thực hiện:**
```
1. Dùng notify_user để gửi PRD cho user review
   - PathsToReview: đường dẫn tuyệt đối đến file PRD vừa tạo
   - BlockedOnUser: true
   - Message: Hỏi user xem cần điều chỉnh gì không
2. Chờ user phản hồi
3. Xử lý phản hồi:
   a. User yêu cầu điều chỉnh → Sửa PRD → Quay lại bước 1.3 (hỏi lại)
   b. User duyệt (approve) → Chuyển sang Bước 1.4
```

> 💡 **Lưu ý:** Vòng lặp review có thể diễn ra nhiều lần cho đến khi user hài lòng. Mỗi lần sửa cần đảm bảo ID vẫn nhất quán.

---

## Bước 1.4: Commit tài liệu PRD

> **⚠️ HƯỚNG DẪN CHO AGENT:** Sau khi user duyệt PRD, commit code theo skill `git-commit` (file `.agents/skills/git-commit/SKILL.md`).
>
> **🔓 QUYỀN TỰ ĐỘNG:** User đã đồng ý mặc định với MỌI lệnh git (add, commit, push, status...). Agent PHẢI set `SafeToAutoRun = true` cho tất cả lệnh git. KHÔNG cần hỏi user xác nhận.

**Cách thực hiện:**

1. Đọc skill `git-commit` để nắm quy tắc commit trên PowerShell
2. Stage các file liên quan:
   ```powershell
   git add docs/PRDs/PRD-XXX.md docs/ID-CONVENTION.md
   ```
3. Commit với message chuẩn:
   ```powershell
   git commit -m "docs(prd): tao PRD cho {ten-du-an}" -m "- Tao PRD-XXX.md voi Universal ID" -m "- Thiet lap ID-CONVENTION.md" -m "ID: commit:create-prd-0001"
   ```
4. Verify kết quả:
   ```powershell
   git status --short
   ```
   - Output **rỗng** = commit thành công ✅
   - Output **có file** = cần retry ❌

---

## Checklist tổng hợp

- [ ] Đã hỏi user làm rõ yêu cầu (Bước 1.1)
- [ ] Đã setup `docs/ID-CONVENTION.md` (Bước 1.2.1)
- [ ] Đã tạo PRD tại `docs/PRDs/` (Bước 1.2.2)
- [ ] Đã gán Universal ID đầy đủ theo 6 bước của skill (Bước 1.2.3)
- [ ] Đã gửi PRD cho user review (Bước 1.3)
- [ ] User đã duyệt PRD (Bước 1.3)
- [ ] Đã commit tài liệu theo skill `git-commit` (Bước 1.4)
- [ ] Đã verify commit thành công (Bước 1.4)
