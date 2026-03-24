---
description: Bước 2 - Thiết kế kiến trúc dự án (ADR + System Design) và commit tài liệu
---
// turbo-all
# 🏗️ Bước 2: Thiết kế Kiến trúc Dự án (Architect)

> **Mục tiêu:** Phân tích PRD → Ghi nhận quyết định kiến trúc (ADR) → Thiết kế hệ thống (System Design) → User duyệt → Commit.
>
> **Vai trò:** Agent đóng vai **Architect** — chịu trách nhiệm thiết kế kiến trúc trước khi dev code.
>
> **🔓 TỰ ĐỘNG HOÀN TOÀN:** Agent tự động thực hiện mọi thao tác (tạo thư mục, viết tài liệu, vẽ diagram...) mà KHÔNG cần hỏi user. Chỉ dừng lại ở Bước 2.4 (User review) trước khi commit.

---

## Tổng quan quy trình

```
Đọc PRD → Tạo ADR → Thiết kế System Design → User review → Commit
```

---

## Bước 2.1: Phân tích yêu cầu kỹ thuật từ PRD

> **⚠️ HƯỚNG DẪN CHO AGENT:** Đọc PRD để hiểu bối cảnh kỹ thuật trước khi thiết kế.

**Cách thực hiện:**
1. Đọc tất cả file PRD trong `docs/PRDs/`
2. Trích xuất các yêu cầu kỹ thuật:
   - Tech stack (`prd:tech-stack-*`)
   - Các ràng buộc phi chức năng (performance, security, compatibility)
   - Các tính năng (`feature:*`) cần thiết kế kiến trúc
3. Liệt kê các **quyết định kiến trúc** cần đưa ra

---

## Bước 2.2: Architecture Decision Records (ADR)

> **Output:** `docs/ADR/ADR-{NNN}_{tên-quyết-định}.md`

Ghi nhận các quyết định kiến trúc quan trọng trước khi bắt đầu code:

1. Đọc `prd:tech-stack-*` từ PRD để xác định tech stack
2. Tạo ADR cho mỗi quyết định kỹ thuật quan trọng
3. Lưu file theo format: `docs/ADR/ADR-{NNN}_{tên-quyết-định}.md`
4. Gán ID: `adr:{tên}-{XXXX}` → `Implements: prd:tech-stack-{XXXX}`

> ⚠️ **BẮT BUỘC:** Tất cả ID trong ADR phải tuân theo skill `assign-universal-id`:
> - Mỗi ID khai báo lần đầu phải có `<a id="..."></a>` anchor
> - Tham chiếu `Implements` phải dùng **cross-document link** đến PRD

**Template ADR:**
```markdown
# ADR-{NNN}: {Tiêu đề quyết định}
`adr:{tên}-{XXXX}`
> Implements: `prd:tech-stack-{XXXX}`

## Bối cảnh (Context)
{Tại sao phải đưa ra quyết định này?}

## Quyết định (Decision)
{Chọn giải pháp gì?}

## Hệ quả (Consequences)
- ✅ {Tác động tích cực}
- ⚠️ {Tác động tiêu cực / rủi ro}

## Trạng thái
- [x] Đề xuất (Proposed)
- [ ] Được chấp thuận (Accepted)
- [ ] Bị thay thế (Superseded by ADR-{NNN})
```

---

## Bước 2.3: System Architecture & Design

> **Input:** PRD (`docs/PRDs/*.md`), ADR (`docs/ADR/*.md`)
> **Output:** `docs/architecture/system-design.md`
>
> ⚠️ **Bước này rất quan trọng!** Architect thiết kế kiến trúc TRƯỚC KHI dev code. Bỏ qua bước này dẫn đến technical debt.

### 2.3.1: Thiết kế kiến trúc tổng thể (High-Level Architecture)

- Xác định kiến trúc phù hợp với loại dự án:
  - **Web:** Monolith, Microservices, Serverless, JAMstack, MVC...
  - **Mobile:** MVVM, Clean Architecture, BLoC...
  - **Game:** ECS (Entity-Component-System), Component-Based...
  - **Backend:** Layered, Hexagonal, CQRS, Event-Driven...
- Vẽ **System Context Diagram** — hệ thống tương tác với actor/hệ thống bên ngoài nào?
- Vẽ **Container Diagram** — các thành phần chính (Frontend, Backend, Database, 3rd-party)
- ID: `research:arch-design-{XXXX}` → `Implements: prd:tech-stack-{XXXX}`

### 2.3.2: Thiết kế Module/Component (Component Design)

- Xác định **cấu trúc folder/module** trong source code, tuỳ tech stack đã chọn trong ADR
  > **Lưu ý:** Cấu trúc folder phụ thuộc vào tech stack và convention:
  > - Web React/Next.js: `src/app/`, `src/components/`, `src/lib/`...
  > - C#/.NET: `Controllers/`, `Services/`, `Models/`, `Data/`...
  > - Unity: `Assets/Scripts/`, `Assets/Prefabs/`, `Assets/Scenes/`...
  > - Flutter: `lib/screens/`, `lib/widgets/`, `lib/models/`...
- Vẽ **Component/Module Diagram** — quan hệ giữa các module (dùng Mermaid)
- Xác định **interface/contract** — public API, method signatures, data contracts

### 2.3.3: Data Flow & State Management

- Thiết kế **luồng dữ liệu (Data Flow)** — dữ liệu đi từ đâu đến đâu?
- Xác định chiến lược **state management** phù hợp:
  - Web: React state, Context, Zustand, Redux...
  - Mobile: Provider, BLoC, GetX, ViewModel...
  - Game: ScriptableObject, Singleton Manager, Event Bus...
  - Backend: Repository pattern, Unit of Work, CQRS...
- Nếu có API: thiết kế **API contract** (endpoints, request/response schema)
- Nếu có database: thiết kế **data model / ERD**

### 2.3.4: Quy ước kỹ thuật (Technical Conventions)

- **Coding Standards:** naming conventions, file naming, import order
- **Error Handling:** chiến lược xử lý lỗi thống nhất
- **Quy ước đặc thù** theo loại dự án:
  - *Web:* Responsive Strategy, Accessibility (WCAG), Performance Budget
  - *Mobile:* Screen size support, Platform-specific guidelines
  - *Game:* Target FPS, Platform targets, Asset pipeline
  - *Backend:* API versioning, Logging strategy, Security standards

### 2.3.5: Đánh giá rủi ro kỹ thuật (Technical Risk Assessment)

- Liệt kê rủi ro kỹ thuật có thể gặp
- Đánh giá mức độ ảnh hưởng (Impact) và xác suất (Likelihood)
- Đề xuất biện pháp giảm thiểu (Mitigation)

**Template System Design Document:**
```markdown
# System Design Document
`research:arch-design-{XXXX}`
> Implements: `prd:tech-stack-{XXXX}`

## 1. Tổng quan kiến trúc
{Mô tả kiến trúc tổng thể — diagram bằng Mermaid}

## 2. Module/Component Design
### 2.1 Cấu trúc thư mục source code
{Folder structure — tuỳ tech stack}

### 2.2 Module/Component Diagram
{Mermaid diagram}

### 2.3 Interface/Contract
| Module/Component | Public API / Interface | Dependencies |
|------------------|----------------------|--------------|
| {Name} | {Methods / Props / Endpoints} | {Depends on} |

## 3. Data Flow
{Mô tả luồng dữ liệu — diagram bằng Mermaid}

## 4. Quy ước kỹ thuật
- **Coding Standards:** {Mô tả}
- **Error Handling:** {Mô tả}
- **Quy ước đặc thù:** {Tuỳ loại dự án}

## 5. Rủi ro kỹ thuật
| # | Rủi ro | Impact | Likelihood | Mitigation |
|---|--------|--------|------------|------------|
| 1 | {Rủi ro} | {H/M/L} | {H/M/L} | {Biện pháp} |

## 6. Danh mục Technology Stack
| Layer | Technology | Version | Lý do chọn |
|-------|------------|---------|------------|
| Language | {Tech} | {Ver} | {Lý do} |
| Framework | {Tech} | {Ver} | {Lý do} |
| Styling/UI | {Tech} | {Ver} | {Lý do} |
| Testing | {Tech} | {Ver} | {Lý do} |
| Build/Deploy | {Tech} | {Ver} | {Lý do} |
```

> **Lưu ý:** System Design Document là tài liệu **sống (living document)**. Cập nhật khi có thay đổi kiến trúc lớn.

---

## Bước 2.4: User review tài liệu kiến trúc

> **⚠️ HƯỚNG DẪN CHO AGENT:** Sau khi tạo xong ADR và System Design, PHẢI hỏi user duyệt.

**Cách thực hiện:**
```
1. Dùng notify_user để gửi tài liệu cho user review
   - PathsToReview: đường dẫn đến các file ADR và system-design.md
   - BlockedOnUser: true
   - Message: Tóm tắt kiến trúc và hỏi user cần điều chỉnh gì không
2. Chờ user phản hồi
3. Xử lý phản hồi:
   a. User yêu cầu điều chỉnh → Sửa tài liệu → Quay lại bước 2.4
   b. User duyệt → Chuyển sang Bước 2.5
```

---

## Bước 2.5: Commit tài liệu kiến trúc

> **⚠️ HƯỚNG DẪN CHO AGENT:** Sau khi user duyệt, commit theo skill `git-commit` (`.agents/skills/git-commit/SKILL.md`).
>
> **🔓 QUYỀN TỰ ĐỘNG:** User đã đồng ý mặc định với MỌI lệnh git. Agent PHẢI set `SafeToAutoRun = true`. KHÔNG cần hỏi user xác nhận.

**Cách thực hiện:**

1. Stage các file:
   ```powershell
   git add docs/ADR/ docs/architecture/
   ```
2. Commit:
   ```powershell
   git commit -m "docs(arch): thiet ke kien truc he thong" -m "- Tao ADR cho cac quyet dinh ky thuat" -m "- Tao System Design Document" -m "ID: commit:arch-design-{XXXX}" -m "Implements: prd:tech-stack-{XXXX}"
   ```
3. Verify:
   ```powershell
   git status --short
   ```

---

## Checklist tổng hợp

- [ ] Đã đọc và phân tích yêu cầu kỹ thuật từ PRD (Bước 2.1)
- [ ] Đã tạo ADR cho các quyết định kiến trúc (Bước 2.2)
- [ ] Đã thiết kế System Architecture (Bước 2.3)
  - [ ] High-Level Architecture + diagrams
  - [ ] Module/Component Design + folder structure
  - [ ] Data Flow & State Management
  - [ ] Technical Conventions
  - [ ] Technical Risk Assessment
- [ ] User đã duyệt tài liệu kiến trúc (Bước 2.4)
- [ ] Đã commit tài liệu theo skill `git-commit` (Bước 2.5)
- [ ] Đã verify commit thành công (Bước 2.5)
