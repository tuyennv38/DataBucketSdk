---
description: Quy trình phát triển phần mềm Agile/Scrum — Từ PRD đến sản phẩm hoàn chỉnh
---
// turbo-all
# 🚀 Agile/Scrum Development Workflow

> **Mục tiêu:** Chuyển đổi tài liệu PRD trong `docs/PRDs/` thành sản phẩm cuối cùng theo quy trình Agile/Scrum tiêu chuẩn, đảm bảo **truy vết hai chiều** (bidirectional traceability) xuyên suốt.

> **Phạm vi áp dụng:** Workflow này là **quy trình chung**, không phụ thuộc vào bất kỳ ngôn ngữ, framework hay nền tảng cụ thể nào. Có thể áp dụng cho mọi loại dự án: web, backend, mobile, game, desktop, v.v.

---

## Tổng quan quy trình

```
Thu thập yêu cầu → Tạo PRD → Backlog Refinement → System Design (Architect) → Sprint Planning → Implementation → User Testing → Commit Finish → Release
```

Quy trình gồm **4 giai đoạn chính**, được tách thành các file chi tiết trong thư mục `agile/`:

---

## 📂 Cấu trúc file workflow

```
.agents/workflows/
├── agile-development.md              ← 🎯 BẠN ĐANG Ở ĐÂY (file tổng — orchestrator)
└── agile/
    ├── 01-create-prd.md              # Giai đoạn 0: Thu thập yêu cầu, tạo PRD, gán ID, commit
    ├── 02-architect.md               # Giai đoạn 1a: Thiết kế kiến trúc (ADR + System Design)
    ├── 01-pre-sprint.md              # Giai đoạn 1b: Backlog, Setup thư mục, Commit docs
    ├── 02-sprint-planning.md         # Bước 2.1: Sprint Planning + Commit plan
    ├── 03-implementation.md          # Bước 2.2 + 2.3: Coding + User Testing + Commit Finish
    ├── 04-review-testing.md          # Bước 2.4: Code Review (self-review)
    ├── 06-release.md                 # Giai đoạn 3: Release, Deploy, Merge
    └── 07-reference.md              # Quy tắc, Vai trò, Traceability queries
```

---

## 🔄 Luồng thực hiện (Execution Flow)

> **⚠️ HƯỚNG DẪN CHO AGENT:** Khi USER yêu cầu chạy workflow này, hãy đọc từng file con theo thứ tự bên dưới. Đọc file ở mỗi bước bằng cách dùng `view_file` trên đường dẫn tương ứng. Chỉ đọc file cần thiết cho bước hiện tại, không cần đọc tất cả cùng lúc.

### Giai đoạn 0: Tạo PRD — Chạy 1 lần khi bắt đầu dự án

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 0.1 | Thu thập & làm rõ yêu cầu từ user | 📄 `agile/01-create-prd.md` |
| 0.2 | Tạo PRD & gán Universal ID (skill: assign-universal-id) | 📄 `agile/01-create-prd.md` |
| 0.3 | User review PRD (vòng lặp đến khi duyệt) | 📄 `agile/01-create-prd.md` |
| 0.4 | **Commit tài liệu PRD** ⚠️ BẮT BUỘC (skill: git-commit) | 📄 `agile/01-create-prd.md` |

> 📖 **Đọc:** `.agents/workflows/agile/01-create-prd.md` để thực hiện toàn bộ Giai đoạn 0.

---

### Giai đoạn 1: Chuẩn bị (Pre-Sprint) — Chạy 1 lần sau khi PRD được duyệt

#### 1a. Thiết kế kiến trúc (Architect)

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 1.1 | Phân tích yêu cầu kỹ thuật từ PRD | 📄 `agile/02-architect.md` |
| 1.2 | Architecture Decision Records (ADR) | 📄 `agile/02-architect.md` |
| 1.3 | System Architecture & Design | 📄 `agile/02-architect.md` |
| 1.4 | User review tài liệu kiến trúc | 📄 `agile/02-architect.md` |
| 1.5 | **Commit tài liệu kiến trúc** ⚠️ BẮT BUỘC | 📄 `agile/02-architect.md` |

> 📖 **Đọc:** `.agents/workflows/agile/02-architect.md` để thực hiện thiết kế kiến trúc.

#### 1b. Backlog & Setup

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 1.6 | Phân tích PRD & Tạo Product Backlog | 📄 `agile/01-pre-sprint.md` |
| 1.7 | Thiết lập cấu trúc thư mục dự án | 📄 `agile/01-pre-sprint.md` |
| 1.8 | **Commit tài liệu Pre-Sprint** ⚠️ BẮT BUỘC | 📄 `agile/01-pre-sprint.md` |

> 📖 **Đọc:** `.agents/workflows/agile/01-pre-sprint.md` để thực hiện backlog & setup.

---

### Giai đoạn 2: Sprint Execution — Lặp lại mỗi Sprint

> **Chu kỳ Sprint khuyến nghị:** 1-2 tuần (tùy quy mô dự án).
> Mỗi Sprint tập trung hoàn thành một nhóm PBI từ Product Backlog.

#### 🏁 Bắt đầu Sprint

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 2.1 | Sprint Planning + **Commit Sprint Plan** ⚠️ | 📄 `agile/02-sprint-planning.md` |

> 📖 **Đọc:** `.agents/workflows/agile/02-sprint-planning.md`

#### 🔁 Lặp lại cho MỖI task trong Sprint

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 2.2 | Implementation (Coding) | 📄 `agile/03-implementation.md` |
| 2.3 | **Báo User Testing** → Chờ duyệt → **Commit Finish** | 📄 `agile/03-implementation.md` |
| 2.4 | **Cập nhật Sprint Plan & Product Backlog** | 📄 `agile/03-implementation.md` |

> 📖 **Đọc:** `.agents/workflows/agile/03-implementation.md`
> ⚠️ **Quan trọng:** Code xong task → Báo user test → User duyệt → Commit finish → Cập nhật Sprint Plan & Backlog → Chuyển task tiếp theo.

---

### Giai đoạn 3: Release — Khi Sprint hoàn tất

| Bước | Mô tả | File chi tiết |
|------|-------|---------------|
| 3.1 | Pre-Release Checklist | 📄 `agile/06-release.md` |
| 3.2 | Deployment | 📄 `agile/06-release.md` |
| 3.3 | Post-Release (Tag, CHANGELOG) | 📄 `agile/06-release.md` |
| 3.4 | Sprint Closing & Merge | 📄 `agile/06-release.md` |

> 📖 **Đọc:** `.agents/workflows/agile/06-release.md`

---

## 📚 Tài liệu tham khảo

| Nội dung | File |
|----------|------|
| Quy tắc quan trọng (5 quy tắc vàng) | 📄 `agile/07-reference.md` |
| Vai trò trong quy trình (PO, Architect, Dev, QA, SM) | 📄 `agile/07-reference.md` |
| Truy vấn Traceability (git log, grep) | 📄 `agile/07-reference.md` |
| Traceability Map (mối liên kết tài liệu) | 📄 `agile/07-reference.md` |

> 📖 **Đọc:** `.agents/workflows/agile/07-reference.md` khi cần tra cứu.

---

## 🗂️ Danh mục Workflows liên quan

| Workflow | Mô tả | File tương ứng |
|----------|--------|---------------|
| `/agile-development` | **Workflow tổng** — chạy file này | `agile-development.md` |
| `/sprint-planning` | Tạo Sprint Plan từ Product Backlog | → xem `agile/02-sprint-planning.md` |
| `/system-design` | Thiết kế kiến trúc hệ thống (Architect) | → xem `agile/01-pre-sprint.md` (Bước 1.4) |
| `/implement` | Triển khai một task cụ thể | → xem `agile/03-implementation.md` |
| `/code-review` | Review code theo checklist | → xem `agile/04-review-testing.md` |
| `/release` | Thực hiện deployment | → xem `agile/06-release.md` |
