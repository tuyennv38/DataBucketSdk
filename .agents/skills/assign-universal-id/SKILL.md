---
name: assign-universal-id
description: "Gán Universal ID cho các thành phần trong tài liệu PRD, đảm bảo truy vết hai chiều (bidirectional traceability) xuyên suốt vòng đời dự án."
version: "1.0.0"
author: "Tuyen"
category: documentation
risk: safe
source: internal
date_added: "2026-03-24"
tags: [traceability, prd, universal-id, agile]
tools: [claude, cursor, gemini]
---

# Assign Universal ID — Gán mã định danh truy vết cho tài liệu dự án

> **Mục tiêu:** Đảm bảo mọi yêu cầu, tính năng, task trong tài liệu PRD đều có mã định danh duy nhất, cho phép truy vết hai chiều (bidirectional traceability).

Skill này hướng dẫn cách gán **Universal ID** cho các thành phần trong tài liệu PRD (và các tài liệu downstream khác) của **bất kỳ dự án nào**:
- **Truy ngược (Backward Trace):** code/commit → task → feature → PRD gốc.
- **Truy xuôi (Forward Trace):** PRD → feature → task → code/commit/test.

---

## Khi nào sử dụng

- Khi nhận được yêu cầu gán ID cho tài liệu PRD hoặc bất kỳ tài liệu nào trong dự án.
- Khi tạo mới hoặc cập nhật PRD, plan, feature spec, task list.
- Khi cần thiết lập hệ thống truy vết (traceability) cho dự án mới.
- Khi cần liên kết chéo (cross-reference) giữa các tầng: PRD → feature → task → code.

## Khi KHÔNG sử dụng

- **Tài liệu tạm thời / scratch notes**: Ghi chú nháp, brainstorm không cần gán ID.
- **README.md hoặc hướng dẫn sử dụng chung**: Tài liệu không mô tả yêu cầu sản phẩm.
- **Tài liệu ngoài scope dự án**: Meeting notes, email, tài liệu onboarding.
- **Khi dự án chưa đạt đủ mức trưởng thành**: Nếu PRD còn đang thay đổi liên tục ở giai đoạn brainstorm ban đầu, chờ nội dung ổn định rồi hãy gán ID.

---

## Điều kiện tiên quyết

1. Đọc file `resources/ID-CONVENTION.md` (đi kèm trong skill này) để nắm quy ước ID.
2. Copy file `resources/ID-CONVENTION.md` vào `docs/ID-CONVENTION.md` của dự án nếu dự án chưa có.
3. Nếu dự án đã có sẵn `docs/ID-CONVENTION.md`, **ưu tiên dùng file của dự án** (có thể đã được tùy biến cho phù hợp với ngữ cảnh riêng).

---

## Hướng dẫn thực hiện

### Định dạng ID

```
<doc-type>:<component-name-XXXX>
```

| Thành phần | Quy tắc |
|------------|---------|
| `doc-type` | Loại tài liệu. Xem bảng bên dưới. |
| `component-name` | Tên ngắn gọn, **kebab-case**, mô tả thành phần. |
| `XXXX` | Số thứ tự 4 chữ số, bắt đầu từ `0001`, tăng dần **trong cùng một doc-type**. |

#### Bảng doc-type

| doc-type   | Mô tả                                        | Ví dụ                          |
|------------|-----------------------------------------------|--------------------------------|
| `prd`      | Yêu cầu sản phẩm gốc trong PRD               | `prd:user-dashboard-0001`      |
| `feature`  | Tính năng (khối chức năng trọn vẹn)           | `feature:search-filter-0001`   |
| `plan`     | Kế hoạch triển khai                           | `plan:sprint-1-setup-0001`     |
| `task`     | Đơn vị công việc nhỏ nhất có thể giao         | `task:setup-database-0001`     |
| `code`     | Module/Component trong source code            | `code:auth-middleware-0001`    |
| `commit`   | Git commit liên kết                           | `commit:init-project-0001`     |
| `bug`      | Lỗi được ghi nhận                             | `bug:null-pointer-0001`        |
| `test`     | Test case                                     | `test:login-flow-0001`         |
| `adr`      | Architecture Decision Record                  | `adr:db-choice-0001`           |
| `research` | Nghiên cứu, spike, proof-of-concept           | `research:cache-options-0001`  |

> **Lưu ý:** Dự án có thể bổ sung thêm doc-type khác nếu cần (ví dụ: `epic`, `story`, `spike`). Cập nhật vào `docs/ID-CONVENTION.md` của dự án.

### Bước 1: Đọc và phân tích tài liệu nguồn

- Đọc toàn bộ tài liệu PRD cần gán ID.
- Xác định các **khối chức năng có ý nghĩa trọn vẹn** (meaningful functional blocks).

### Bước 2: Phân loại thành phần — Chọn đúng mức độ chi tiết (Granularity)

> **⚠️ QUY TẮC VÀNG: Gom nhóm trước, tách nhỏ sau (nếu có nhu cầu).**

Phân loại theo **3 tầng chính**:

| Tầng | doc-type | Mức độ chi tiết | Nguyên tắc |
|------|----------|-----------------|------------|
| **Tầng 1: Yêu cầu gốc** | `prd` | Mỗi section lớn trong PRD = 1 ID | Mô tả "CÁI GÌ" cần làm |
| **Tầng 2: Tính năng** | `feature` | Mỗi khối chức năng trọn vẹn = 1 ID | Mô tả "CHỨC NĂNG GÌ" được cung cấp |
| **Tầng 3: Công việc** | `task` | Mỗi đơn vị công việc có thể giao = 1 ID | Mô tả "LÀM GÌ" cụ thể |

**Cách xác định "khối chức năng trọn vẹn":**
- Tự hỏi: *"Thành phần này có thể hoạt động/có ý nghĩa độc lập không?"*
- Nếu **CÓ** → ID riêng (ví dụ: "Form đăng nhập", "Bộ lọc tìm kiếm", "Dashboard thống kê").
- Nếu **KHÔNG** → Gộp vào khối cha (ví dụ: "ô nhập email" chỉ tồn tại trong form → gộp vào feature form).

### Bước 3: Gán ID theo format chuẩn

Áp dụng format `<doc-type>:<component-name-XXXX>` cho từng thành phần đã xác định ở Bước 2.

**Lưu ý quan trọng:** Mỗi `doc-type` có **bộ đếm XXXX riêng biệt**, không đánh số xuyên suốt.

### Bước 3b: Tạo Anchor cho ID tại nơi khai báo (Inline ID Anchor) — BẮT BUỘC

> **⚠️ CRITICAL: Mỗi ID khi được khai báo lần đầu trong tài liệu PHẢI được bọc trong HTML anchor tag `<a id="...">` để các bảng phụ lục và tài liệu khác có thể link trực tiếp đến.**

**Mục đích:**
- Cho phép bảng Phụ lục link chính xác đến nơi ID được mô tả chi tiết
- Cho phép **liên kết chéo giữa các file** (cross-document linking): file backlog link đến chi tiết trong PRD, file sprint plan link đến backlog, v.v.
- Cho phép IDE/editor nhảy trực tiếp đến mô tả của ID

**Format khai báo ID inline (nơi ID được định nghĩa lần đầu):**
```markdown
<a id="prd-restructure-overview-0001"></a>
`prd:restructure-overview-0001`
```

**Quy tắc chuyển đổi ID → anchor id:**
- Thay dấu `:` bằng `-` (vì HTML id không nên chứa `:`)
- Giữ nguyên phần còn lại
- Ví dụ: `feature:split-router-0001` → `id="feature-split-router-0001"`

**Format link đến ID (trong cùng file):**
```markdown
[`prd:restructure-overview-0001`](#prd-restructure-overview-0001)
```

**Format link đến ID (từ file khác - cross-document):**
```markdown
[`prd:restructure-overview-0001`](file:///path/to/PRD-001.md#prd-restructure-overview-0001)
```

### Bước 4: Gán tham chiếu ngược (Implements) — BẮT BUỘC

> **⚠️ CRITICAL: Đây là bước hay bị bỏ sót nhất. KHÔNG BAO GIỜ được bỏ qua.**

Mọi thành phần downstream **PHẢI** tham chiếu ngược về thành phần upstream:

```
feature  → Implements: prd:*
task     → Implements: feature:* hoặc prd:*
code     → Implements: task:* hoặc feature:*
test     → Implements: feature:* hoặc task:*
commit   → Implements: task:*
```

**Format trong tài liệu:**
```markdown
`feature:search-filter-0001`
> Implements: `prd:product-listing-0001`
```

**Format trong bảng:**
```markdown
| ID | Implements | Mô tả |
|----|------------|-------|
| `task:build-filter-ui-0001` | `feature:search-filter-0001` | Dựng UI bộ lọc |
```

### Bước 5: Tạo bảng tổng hợp (Phụ lục) dạng CLICKABLE

Luôn thêm **Phụ lục: Bảng tổng hợp ID & Truy vết** ở cuối tài liệu. Bảng này chứa **TẤT CẢ** ID trong tài liệu.
> 💡 **QUAN TRỌNG:** Ở bảng này, các ID trong cột "ID" và các tham chiếu ở cột "Implements" (nếu có ở cùng file) **PHẢI** được định dạng thành **Markdown Anchor Link**. 
> Điều này giúp người đọc bấm vào ID là tự động cuộn (jump) về đúng đề mục mô tả chi tiết của ID đó.
> Cú pháp Markdown: `[`<ID>`]({{anchor-link}})` — Ví dụ: `[`prd:login-0001`](#1-tổng-quan)`

```markdown
## Phụ lục: Bảng tổng hợp ID & Truy vết

| ID | Loại | Implements | Mô tả ngắn |
|----|------|------------|-------------|
| [`prd:...-0001`](#1-tổng-quan) | prd | — (gốc) | ... |
| [`feature:...-0001`](#2-tính-năng) | feature | [`prd:...-0001`](#1-tổng-quan) | ... |
| [`task:...-0001`](#5-tasks) | task | [`feature:...-0001`](#2-tính-năng) | ... |
```

### Bước 6: Tự kiểm tra (Self-QA Checklist)

Trước khi hoàn tất, **bắt buộc kiểm tra** tất cả các điểm sau:

- [ ] Mỗi ID khai báo lần đầu có `<a id="..."></a>` anchor ngay phía trên không? (Bước 3b)
- [ ] Mỗi `feature:*` có dòng `> Implements: prd:*` với link anchor không?
- [ ] Mỗi `task:*` có cột/dòng `Implements` trỏ về `feature:*` hoặc `prd:*` không?
- [ ] Không có ID nào bị trùng lặp trên toàn tài liệu?
- [ ] Không có thành phần nào bị tách quá nhỏ (xem Anti-Pattern 1)?
- [ ] Bảng phụ lục cuối file có đầy đủ tất cả ID?
- [ ] Tất cả ID trong bảng phụ lục dùng anchor link (`[...](#id-anchor)`) để click được?
- [ ] Mọi ID upstream (`prd:*`) có ít nhất 1 thành phần downstream tham chiếu đến?
- [ ] Mỗi doc-type có bộ đếm XXXX riêng biệt?
- [ ] Các tham chiếu cross-document dùng đúng `[id](relative-path.md#anchor)` format?

---

## Ví dụ

> Chú ý: Mỗi ID được khai báo với `<a id="...">` anchor ngay phía trên. Bảng phụ lục dùng `[id](#anchor)` để click nhảy về đúng vị trí.

### Ví dụ 1: PRD trang danh sách sản phẩm

```markdown
## 1. Tổng quan
<a id="prd-product-listing-0001"></a>
`prd:product-listing-0001`
Xây dựng trang danh sách sản phẩm...

## 2. Giao diện
<a id="feature-product-grid-0001"></a>
`feature:product-grid-0001`
> Implements: [`prd:product-listing-0001`](#prd-product-listing-0001)
- Lưới sản phẩm, card sản phẩm, phân trang.

## 3. Tech Stack
<a id="prd-tech-stack-0002"></a>
`prd:tech-stack-0002`
- Framework, styling, database...

## 4. Tìm kiếm & Lọc
<a id="feature-search-filter-0002"></a>
`feature:search-filter-0002`
> Implements: [`prd:product-listing-0001`](#prd-product-listing-0001)
- Tìm theo tên, lọc theo danh mục, lọc theo giá.

## 5. Tasks
<a id="task-setup-project-0001"></a>
<a id="task-build-grid-ui-0002"></a>
<a id="task-add-search-0003"></a>

| ID | Implements | Bước |
|----|------------|------|
| `task:setup-project-0001` | [`prd:tech-stack-0002`](#prd-tech-stack-0002) | Khởi tạo dự án |
| `task:build-grid-ui-0002` | [`feature:product-grid-0001`](#feature-product-grid-0001) | Dựng lưới sản phẩm |
| `task:add-search-0003` | [`feature:search-filter-0002`](#feature-search-filter-0002) | Thêm bộ lọc |

## Phụ lục: Bảng tổng hợp ID & Truy vết
| ID | Loại | Implements | Mô tả |
|----|------|------------|-------|
| [`prd:product-listing-0001`](#prd-product-listing-0001) | prd | — (gốc) | Trang danh sách sản phẩm |
| [`prd:tech-stack-0002`](#prd-tech-stack-0002) | prd | — (gốc) | Tech stack |
| [`feature:product-grid-0001`](#feature-product-grid-0001) | feature | [`prd:product-listing-0001`](#prd-product-listing-0001) | Lưới sản phẩm |
| [`feature:search-filter-0002`](#feature-search-filter-0002) | feature | [`prd:product-listing-0001`](#prd-product-listing-0001) | Tìm kiếm & lọc |
| [`task:setup-project-0001`](#task-setup-project-0001) | task | [`prd:tech-stack-0002`](#prd-tech-stack-0002) | Khởi tạo dự án |
| [`task:build-grid-ui-0002`](#task-build-grid-ui-0002) | task | [`feature:product-grid-0001`](#feature-product-grid-0001) | Dựng UI |
| [`task:add-search-0003`](#task-add-search-0003) | task | [`feature:search-filter-0002`](#feature-search-filter-0002) | Thêm bộ lọc |
```

### Ví dụ 2: Link từ file backlog đến PRD (cross-document)

```markdown
## Backlog Item
- **Feature:** `feature:search-filter-0001`
- **Implements:** [`prd:product-listing-0001`](./PRDs/PRD-001.md#prd-product-listing-0001)
- **Priority:** High
```

### Ví dụ 3: ID anchor khai báo đúng format

```markdown
<!-- Khai báo ID lần đầu — LUÔN có anchor -->
<a id="prd-product-listing-0001"></a>
`prd:product-listing-0001`
Xây dựng trang danh sách sản phẩm...

<!-- Tham chiếu đến ID (trong cùng file) — dùng link -->
> Implements: [`prd:product-listing-0001`](#prd-product-listing-0001)

<!-- Tham chiếu đến ID (từ file khác) — dùng relative path -->
> Implements: [`prd:product-listing-0001`](./PRDs/PRD-001.md#prd-product-listing-0001)
```

---

## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: Tách quá nhỏ (Over-Granular IDs)

```markdown
# SAI — Mỗi thành phần nhỏ (ô input, button, label) = 1 feature riêng
feature:name-input-0001       ← 1 ô input
feature:email-input-0002      ← 1 ô input khác
feature:submit-button-0003    ← 1 nút bấm
feature:validate-name-required-0004   ← 1 rule validate
feature:validate-email-format-0005    ← 1 rule validate khác
```

**Tại sao sai:** Các thành phần con (ô input, button) không có ý nghĩa độc lập – chúng là bộ phận của một form. Tách quá nhỏ tạo quá nhiều ID vụn vặt, gây khó quản lý mà không tăng giá trị truy vết.

**Cách đúng:**
```markdown
feature:registration-form-0001     ← gộp tất cả thành phần UI của form
feature:form-validation-0002       ← gộp tất cả validation rules
```

### ❌ Anti-Pattern 2: Thiếu tham chiếu ngược (Missing Implements)

```markdown
# SAI — feature và task không ghi Implements
feature:search-filter-0001
Bộ lọc tìm kiếm sản phẩm...

| task:build-filter-ui-0001 | Dựng giao diện bộ lọc |
```

**Tại sao sai:** Không thể truy vết ngược task → feature → PRD. Vi phạm nguyên tắc cốt lõi của hệ thống ID.

**Cách đúng:**
```markdown
feature:search-filter-0001
> Implements: prd:product-listing-0001

| task:build-filter-ui-0001 | feature:search-filter-0001 | Dựng giao diện bộ lọc |
```

### ❌ Anti-Pattern 3: Đánh số xuyên doc-type

```markdown
# SAI — số thứ tự chạy liên tục qua nhiều doc-type
prd:dashboard-0001
feature:chart-widget-0002     ← lẽ ra phải là 0001 của feature
task:setup-project-0003       ← lẽ ra phải là 0001 của task
```

**Cách đúng:** Mỗi `doc-type` có bộ đếm riêng:
```markdown
prd:dashboard-0001            ← prd counter: 1
prd:tech-stack-0002           ← prd counter: 2
feature:chart-widget-0001     ← feature counter: 1 (riêng biệt)
task:setup-project-0001       ← task counter: 1 (riêng biệt)
```

### ❌ Anti-Pattern 4: Khai báo ID không có Anchor (Missing Inline Anchor)

```markdown
# SAI — ID khai báo mà không có <a id> anchor
`prd:product-listing-0001`
Xây dựng trang danh sách sản phẩm...

`feature:search-filter-0001`
> Implements: `prd:product-listing-0001`
```

**Tại sao sai:** Bảng Phụ lục không thể link click về đúng vị trí vì không có anchor. Các file khác cũng không thể deep-link được.

**Cách đúng:**
```markdown
<a id="prd-product-listing-0001"></a>
`prd:product-listing-0001`
Xây dựng trang danh sách sản phẩm...

<a id="feature-search-filter-0001"></a>
`feature:search-filter-0001`
> Implements: [`prd:product-listing-0001`](#prd-product-listing-0001)
```

---

## Skill liên quan

- `@skill-creator` — Dùng để tạo hoặc chuẩn hóa skill mới
- `@git-commit` — Dùng khi commit code với ID truy vết (`commit:*`, `Implements: task:*`)

---

## Resources (Đóng gói trong skill)

| File | Mô tả | Cách sử dụng |
|------|-------|--------------|
| `resources/ID-CONVENTION.md` | Quy ước chính thức format, doc-type, quy tắc ID | Copy vào `docs/ID-CONVENTION.md` của dự án khi bắt đầu |
| `resources/PRD-TEMPLATE.md` | Template PRD mẫu với Universal ID | Dùng làm điểm khởi đầu khi viết PRD mới |
