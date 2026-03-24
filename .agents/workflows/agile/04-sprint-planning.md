---
description: Bước 4 - Sprint Planning - Lập kế hoạch Sprint từ Product Backlog
---
// turbo-all
# 🏁 Bước 4: Sprint Planning

> **Mục tiêu:** Chọn PBI từ Product Backlog → Phân tách thành task → User duyệt → Commit Sprint Plan.
>
> **🔓 TỰ ĐỘNG HOÀN TOÀN:** Agent tự động thực hiện mọi thao tác mà KHÔNG cần hỏi user. Chỉ dừng lại ở Bước 4.3 (User review) trước khi commit.

---

## Tổng quan quy trình

```
Đọc Backlog + Architecture → Chọn PBI → Phân tách task (SMART) → User review → Commit
```

---

## Bước 4.1: Đọc dữ liệu đầu vào

> **Input cần đọc:**
> - `docs/backlog/product-backlog.md` — Product Backlog (danh sách PBI theo priority)
> - `docs/architecture/system-design.md` — Kiến trúc hệ thống (để phân tách task theo module/component)
> - `docs/ADR/*.md` — Quyết định kiến trúc (để task phù hợp với tech stack đã chọn)
> - `docs/PRDs/*.md` — PRD gốc (để hiểu chi tiết yêu cầu khi cần)
>
> **Output:** `docs/sprints/sprint-{N}/sprint-plan.md`

1. Đọc Product Backlog → xác định các PBI theo thứ tự ưu tiên
2. Đọc System Design → hiểu cấu trúc module/component để phân tách task hợp lý
3. Đọc ADR → nắm tech stack và constraints

---

## Bước 4.2: Chọn PBI và phân tách thành Task

1. Chọn các PBI từ Product Backlog theo thứ tự ưu tiên (Critical → High → Medium → Low)
2. Phân tách mỗi PBI thành các **task** cụ thể:
   - Giữ nguyên task ID từ PRD nếu đã có, hoặc tạo mới theo quy ước `assign-universal-id`
   - Phân tách theo module/component từ System Design
3. Mỗi task phải đáp ứng tiêu chí **SMART**:
   - **S**pecific: Rõ ràng, cụ thể
   - **M**easurable: Có thể đo lường (Definition of Done)
   - **A**chievable: Có thể hoàn thành trong Sprint
   - **R**elevant: Liên quan đến Sprint Goal
   - **T**ime-bound: Ước lượng thời gian
4. Tạo file Sprint Plan theo template bên dưới

> ⚠️ **BẮT BUỘC:** Tất cả ID trong Sprint Plan phải tuân theo skill `assign-universal-id`:
> - Mỗi ID khai báo lần đầu phải có `<a id="..."></a>` anchor
> - Tham chiếu `Implements` phải dùng **cross-document link** đến Backlog
> - Cuối file phải có **Phụ lục: Bảng tổng hợp ID & Truy vết**
> - Mỗi `doc-type` có bộ đếm XXXX riêng biệt

**Template Sprint Plan:**
```markdown
# Sprint {N} Plan
<a id="plan-sprint-{N}-{XXXX}"></a>
`plan:sprint-{N}-{XXXX}`

## Sprint Goal
{Mục tiêu Sprint — 1-2 câu mô tả giá trị delivery}

## Sprint Duration
- **Start:** {YYYY-MM-DD}
- **End:** {YYYY-MM-DD}

## Sprint Backlog

<a id="task-{name}-0001"></a>
<a id="task-{name}-0002"></a>

| # | Task ID | Implements | Mô tả | Ước lượng | Status |
|---|---------|------------|--------|-----------|--------|
| 1 | `task:{name}-0001` | [`feature:{name}-{XXXX}`](../../backlog/{backlog-file}.md#feature-{name}-{XXXX}) | {Mô tả} | {giờ} | ⬜ To Do |
| 2 | `task:{name}-0002` | [`feature:{name}-{XXXX}`](../../backlog/{backlog-file}.md#feature-{name}-{XXXX}) | {Mô tả} | {giờ} | ⬜ To Do |

## Definition of Done (DoD)
- [ ] Code đã được viết và build thành công
- [ ] Unit tests pass
- [ ] Code review hoàn tất và không còn comment mở
- [ ] Đã được QA Verify (Pass Test và đóng tất cả bug liên quan)
- [ ] Tài liệu đã cập nhật (nếu cần)
- [ ] Demo thành công trên môi trường staging

## Phụ lục: Bảng tổng hợp ID & Truy vết

| ID | Loại | Implements | Mô tả ngắn |
|----|------|------------|-------------|
| [`plan:sprint-{N}-{XXXX}`](#plan-sprint-{N}-{XXXX}) | plan | — (gốc) | Sprint {N} Plan |
| [`task:{name}-0001`](#task-{name}-0001) | task | [`feature:{name}-{XXXX}`](../../backlog/{backlog-file}.md#feature-{name}-{XXXX}) | {Mô tả} |
```

---

## Bước 4.3: User review Sprint Plan

> **⚠️ HƯỚNG DẪN CHO AGENT:** Sau khi tạo xong Sprint Plan, PHẢI hỏi user duyệt.

**Cách thực hiện:**
```
1. Dùng notify_user để gửi Sprint Plan cho user review
   - PathsToReview: đường dẫn đến sprint-plan.md
   - BlockedOnUser: true
   - Message: Tóm tắt Sprint Goal, số lượng task, tổng ước lượng
2. Chờ user phản hồi
3. Xử lý phản hồi:
   a. User yêu cầu điều chỉnh → Sửa Sprint Plan → Quay lại bước 4.3
   b. User duyệt → Chuyển sang Bước 4.4
```

---

## Bước 4.4: Commit Sprint Plan

> **⚠️ HƯỚNG DẪN CHO AGENT:** Sau khi user duyệt, commit theo skill `git-commit` (`.agents/skills/git-commit/SKILL.md`).
>
> **🔓 QUYỀN TỰ ĐỘNG:** User đã đồng ý mặc định với MỌI lệnh git. Agent PHẢI set `SafeToAutoRun = true`. KHÔNG cần hỏi user xác nhận.

**Cách thực hiện:**

1. Stage:
   ```powershell
   git add docs/sprints/sprint-{N}/
   ```
2. Commit:
   ```powershell
   git commit -m "docs(sprint-{N}): tao Sprint {N} Plan" -m "- Sprint Goal: {mo ta ngan}" -m "- {so luong} tasks planned" -m "ID: commit:sprint-{N}-plan-{XXXX}" -m "Refs: plan:sprint-{N}-{XXXX}"
   ```
3. Verify:
   ```powershell
   git status --short
   ```

---

## Checklist tổng hợp

- [ ] Đã đọc Product Backlog, Architecture docs, ADR (Bước 4.1)
- [ ] Đã chọn PBI và phân tách thành task SMART (Bước 4.2)
- [ ] Đã tạo Sprint Plan theo template (Bước 4.2)
- [ ] User đã duyệt Sprint Plan (Bước 4.3)
- [ ] Đã commit theo skill `git-commit` (Bước 4.4)
- [ ] Đã verify commit thành công (Bước 4.4)
