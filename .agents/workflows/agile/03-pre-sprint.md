---
description: Bước 3 - Tạo Product Backlog, thiết lập cấu trúc thư mục, và commit tài liệu
---
// turbo-all
# 📋 Bước 3: Pre-Sprint (Backlog & Setup)

> **Mục tiêu:** Phân tích PRD → Tạo Product Backlog → Thiết lập cấu trúc thư mục → User duyệt → Commit.
>
> **🔓 TỰ ĐỘNG HOÀN TOÀN:** Agent tự động thực hiện mọi thao tác mà KHÔNG cần hỏi user. Chỉ dừng lại ở Bước 3.3 (User review) trước khi commit.

---

## Tổng quan quy trình

```
Đọc PRD → Tạo Product Backlog → Thiết lập thư mục → User review → Commit
```

---

## Bước 3.1: Phân tích PRD & Architecture → Tạo Product Backlog

> **Input:** `docs/PRDs/*.md` + `docs/architecture/system-design.md` + `docs/ADR/*.md`
> **Output:** `docs/backlog/product-backlog.md`

1. Đọc tất cả file PRD trong `docs/PRDs/`
2. Đọc tài liệu kiến trúc trong `docs/architecture/` và `docs/ADR/` (đã tạo ở bước `02-architect.md`)
3. Trích xuất tất cả **feature** (các ID có prefix `feature:*`) từ PRD
4. Kết hợp thông tin từ System Design (module, component, data flow) để phân rã feature thành các PBI phù hợp
3. Với mỗi feature, tạo một **Product Backlog Item (PBI)** bao gồm:
   - ID (giữ nguyên Universal ID từ PRD)
   - Mô tả (User Story format: "Với vai trò là... tôi muốn... để...")
   - Tiêu chí chấp nhận (Acceptance Criteria)
   - Mức độ ưu tiên (Priority: Critical / High / Medium / Low)
   - Ước lượng Story Points (Fibonacci: 1, 2, 3, 5, 8, 13)
   - Tham chiếu ngược: `Implements: prd:*`
4. Lưu vào `docs/backlog/product-backlog.md`

**Template Product Backlog Item:**

> ⚠️ **BẮT BUỘC:** Tất cả ID trong Product Backlog phải tuân theo skill `assign-universal-id`:
> - Mỗi ID khai báo lần đầu phải có `<a id="..."></a>` anchor ngay phía trên
> - Tham chiếu `Implements` phải dùng **cross-document link** đến PRD: `[prd:...](../PRDs/{file}.md#anchor)`
> - Cuối file phải có **Phụ lục: Bảng tổng hợp ID & Truy vết** với clickable anchor links
> - Mỗi `doc-type` có bộ đếm XXXX riêng biệt

```markdown
<a id="feature-{name}-{XXXX}"></a>

### PBI: {Tên feature}
`feature:{name}-{XXXX}`
> Implements: [`prd:{name}-{XXXX}`](../PRDs/{prd-file}.md#prd-{name}-{XXXX})

- **User Story:** Với vai trò là [persona], tôi muốn [hành động] để [lợi ích].
- **Acceptance Criteria:**
  - [ ] AC1: {Tiêu chí 1}
  - [ ] AC2: {Tiêu chí 2}
- **Priority:** {Critical | High | Medium | Low}
- **Story Points:** {1 | 2 | 3 | 5 | 8 | 13}
- **Status:** 📋 Backlog

---

## Phụ lục: Bảng tổng hợp ID & Truy vết

| ID | Loại | Implements | Mô tả ngắn |
|----|------|------------|-------------|
| [`feature:{name}-{XXXX}`](#feature-{name}-{XXXX}) | feature | [`prd:{name}-{XXXX}`](../PRDs/{prd-file}.md#prd-{name}-{XXXX}) | {Mô tả} |
```

---

## Bước 3.2: Thiết lập cấu trúc thư mục dự án

> **Output:** Cấu trúc thư mục chuẩn

> ⚠️ **CRITICAL: KHÔNG ĐƯỢC XOÁ các file/thư mục đã có sẵn trong `docs/` và `.agents/`.** Chỉ **tạo thêm** các thư mục con còn thiếu, **TUYỆT ĐỐI không xoá, ghi đè, hay di chuyển** các file hiện có.

Tạo cấu trúc thư mục tài liệu dự án. Phần `docs/` là cố định, phần source code tuỳ theo tech stack:
```
{project-root}/
├── docs/                  # Tài liệu dự án (cố định cho mọi project)
│   ├── PRDs/              # Tài liệu yêu cầu sản phẩm
│   ├── ID-CONVENTION.md   # Quy ước Universal ID
│   ├── backlog/           # Product Backlog
│   ├── sprints/           # Sprint records
│   ├── architecture/      # System Design & Architecture documents
│   ├── ADR/               # Architecture Decision Records
│   └── test-plans/        # Test plans
├── {source-code}/         # Source code (tùy tech stack: src/, Assets/, app/, lib/...)
├── {test-code}/           # Test files (tùy tech stack: tests/, __tests__/, Tests/...)
└── .agents/               # Agent skills & workflows
```
> **Lưu ý:** Cấu trúc source code bên trong (`{source-code}/`) phụ thuộc vào tech stack và kiến trúc đã chọn trong `02-architect.md`.

---

## Bước 3.3: User review tài liệu

> **⚠️ HƯỚNG DẪN CHO AGENT:** Sau khi tạo xong Backlog và cấu trúc thư mục, PHẢI hỏi user duyệt.

**Cách thực hiện:**
```
1. Dùng notify_user để gửi tài liệu cho user review
   - PathsToReview: đường dẫn đến product-backlog.md
   - BlockedOnUser: true
   - Message: Tóm tắt backlog và hỏi user cần điều chỉnh gì không
2. Chờ user phản hồi
3. Xử lý phản hồi:
   a. User yêu cầu điều chỉnh → Sửa tài liệu → Quay lại bước 3.3
   b. User duyệt → Chuyển sang Bước 3.4
```

---

## Bước 3.4: Commit tài liệu Pre-Sprint

> **⚠️ HƯỚNG DẪN CHO AGENT:** Sau khi user duyệt, commit theo skill `git-commit` (`.agents/skills/git-commit/SKILL.md`).
>
> **🔓 QUYỀN TỰ ĐỘNG:** User đã đồng ý mặc định với MỌI lệnh git. Agent PHẢI set `SafeToAutoRun = true`. KHÔNG cần hỏi user xác nhận.

**Cách thực hiện:**

1. Stage các file:
   ```powershell
   git add docs/backlog/
   ```
2. Commit:
   ```powershell
   git commit -m "docs(backlog): tao Product Backlog tu PRD" -m "- Trich xuat PBI tu PRD" -m "- Thiet lap cau truc thu muc" -m "ID: commit:pre-sprint-{XXXX}"
   ```
3. Verify:
   ```powershell
   git status --short
   ```

---

## Checklist tổng hợp

- [ ] Đã phân tích PRD và tạo Product Backlog (Bước 3.1)
- [ ] Đã thiết lập cấu trúc thư mục dự án (Bước 3.2)
- [ ] User đã duyệt tài liệu (Bước 3.3)
- [ ] Đã commit tài liệu theo skill `git-commit` (Bước 3.4)
- [ ] Đã verify commit thành công (Bước 3.4)
