---
description: Quy trình phát triển phần mềm Agile/Scrum — Từ PRD đến sản phẩm hoàn chỉnh
---
// turbo-all
# 🚀 Agile/Scrum Development Workflow

> **Mục tiêu:** Chuyển đổi yêu cầu thành sản phẩm cuối cùng theo quy trình Agile, đảm bảo **truy vết hai chiều** (bidirectional traceability) xuyên suốt.

> **Phạm vi áp dụng:** Workflow này là **quy trình chung**, không phụ thuộc vào bất kỳ ngôn ngữ, framework hay nền tảng cụ thể nào.

> **🔓 QUY TẮC CHUNG VỀ COMMIT:** Khi user đã duyệt (review) ở bất kỳ bước nào, Agent tự động commit mà KHÔNG cần hỏi bất kỳ quyền gì liên quan đến git (add, commit, push, status...). Agent PHẢI set `SafeToAutoRun = true` cho tất cả lệnh git. Quy tắc này áp dụng cho MỌI bước trong toàn bộ workflow.

---

## Tổng quan quy trình

```
Thu thập yêu cầu → Tạo PRD → Thiết kế kiến trúc → Tạo Backlog → Sprint Planning → Implementation (lặp)
```

Quy trình gồm **5 bước chính**, được tách thành các file chi tiết trong thư mục `agile/`:

---

## 📂 Cấu trúc file workflow

```
.agents/workflows/
├── agile-development.md              ← 🎯 BẠN ĐANG Ở ĐÂY (file tổng — orchestrator)
└── agile/
    ├── 01-create-prd.md              # Bước 1: Thu thập yêu cầu, tạo PRD, gán ID, commit
    ├── 02-architect.md               # Bước 2: Thiết kế kiến trúc (ADR + System Design)
    ├── 03-pre-sprint.md              # Bước 3: Tạo Product Backlog, setup thư mục
    ├── 04-sprint-planning.md         # Bước 4: Sprint Planning
    └── 05-implementation.md          # Bước 5: Coding + User Test + Commit (lặp lại mỗi task)
```

---

## 🔄 Luồng thực hiện (Execution Flow)

> **⚠️ HƯỚNG DẪN CHO AGENT:** Đọc từng file con theo thứ tự bên dưới. Chỉ đọc file cần thiết cho bước hiện tại.

### Bước 1: Tạo PRD — Chạy 1 lần khi bắt đầu dự án

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 1.1 | Thu thập & làm rõ yêu cầu từ user | 📄 `agile/01-create-prd.md` |
| 1.2 | Tạo PRD & gán Universal ID | 📄 `agile/01-create-prd.md` |
| 1.3 | User review PRD | 📄 `agile/01-create-prd.md` |
| 1.4 | **Commit tài liệu PRD** ⚠️ | 📄 `agile/01-create-prd.md` |

> 📖 **Đọc:** `.agents/workflows/agile/01-create-prd.md`

---

### Bước 2: Thiết kế Kiến trúc (Architect)

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 2.1 | Phân tích yêu cầu kỹ thuật từ PRD | 📄 `agile/02-architect.md` |
| 2.2 | Architecture Decision Records (ADR) | 📄 `agile/02-architect.md` |
| 2.3 | System Architecture & Design | 📄 `agile/02-architect.md` |
| 2.4 | User review tài liệu kiến trúc | 📄 `agile/02-architect.md` |
| 2.5 | **Commit tài liệu kiến trúc** ⚠️ | 📄 `agile/02-architect.md` |

> 📖 **Đọc:** `.agents/workflows/agile/02-architect.md`

---

### Bước 3: Pre-Sprint (Backlog & Setup)

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 3.1 | Phân tích PRD & Architecture → Tạo Product Backlog | 📄 `agile/03-pre-sprint.md` |
| 3.2 | Thiết lập cấu trúc thư mục dự án | 📄 `agile/03-pre-sprint.md` |
| 3.3 | User review tài liệu | 📄 `agile/03-pre-sprint.md` |
| 3.4 | **Commit tài liệu Pre-Sprint** ⚠️ | 📄 `agile/03-pre-sprint.md` |

> 📖 **Đọc:** `.agents/workflows/agile/03-pre-sprint.md`

---

### Bước 4: Sprint Planning — Lặp lại mỗi Sprint

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 4.1 | Đọc Backlog + Architecture | 📄 `agile/04-sprint-planning.md` |
| 4.2 | Chọn PBI & phân tách task (SMART) | 📄 `agile/04-sprint-planning.md` |
| 4.3 | User review Sprint Plan | 📄 `agile/04-sprint-planning.md` |
| 4.4 | **Commit Sprint Plan** ⚠️ | 📄 `agile/04-sprint-planning.md` |

> 📖 **Đọc:** `.agents/workflows/agile/04-sprint-planning.md`

---

### Bước 5: Implementation — Lặp lại cho MỖI task trong Sprint

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 5.1 | Coding (viết code, self-review) | 📄 `agile/05-implementation.md` |
| 5.2 | Báo User Test → Chờ duyệt | 📄 `agile/05-implementation.md` |
| 5.3 | Commit Finish + Cập nhật Sprint Plan & Backlog | 📄 `agile/05-implementation.md` |

> 📖 **Đọc:** `.agents/workflows/agile/05-implementation.md`
> ⚠️ **Lặp lại:** Bước 5.1 → 5.3 cho mỗi task. Khi hết task trong Sprint → Quay lại Bước 4 cho Sprint tiếp theo.
