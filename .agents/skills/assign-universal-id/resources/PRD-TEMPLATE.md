# Product Requirements Document (PRD): {Tên dự án / Module}

> **Quy ước ID:** Xem chi tiết tại [docs/ID-CONVENTION.md](../../docs/ID-CONVENTION.md)
>
> **Hướng dẫn sử dụng template:**
> - Thay thế các placeholder `{...}` bằng nội dung thực tế.
> - Xóa các dòng comment `<!-- ... -->` sau khi điền xong.
> - Đảm bảo mọi feature/task đều có dòng `Implements:`.
> - Thêm hoặc bớt section tùy theo quy mô dự án.

---

## 1. Tổng quan
`prd:{tên-dự-án}-0001`

<!-- Mô tả ngắn gọn dự án/module cần xây dựng. Trả lời câu hỏi: "Chúng ta đang xây cái gì và tại sao?" -->

{Mô tả tổng quan dự án ở đây...}

---

## 2. Giao diện người dùng (UI)
`feature:{tên-feature}-0001`
> Implements: `prd:{tên-dự-án}-0001`

<!-- Liệt kê các thành phần UI chính. GOM các thành phần liên quan thành 1 feature duy nhất. KHÔNG tách mỗi ô input/button thành feature riêng. -->

- {Thành phần 1}: {Mô tả}
- {Thành phần 2}: {Mô tả}
- {Thành phần 3}: {Mô tả}

---

## 3. Yêu cầu kỹ thuật
`prd:tech-stack-{XXXX}`

<!-- Liệt kê stack công nghệ, framework, styling, ngôn ngữ -->

- **Framework**: {Framework}
- **Styling**: {Công cụ CSS}
- **Loại ứng dụng**: {Static / SPA / SSR / ...}
- **Ngôn ngữ hiển thị**: {Tiếng Việt / Tiếng Anh / ...}

---

## 4. Validation / Logic nghiệp vụ
`feature:{tên-validation}-{XXXX}`
> Implements: `prd:{tên-dự-án}-0001`

<!-- Liệt kê các quy tắc kiểm tra. GOM tất cả rules vào 1 feature, KHÔNG tách mỗi rule = 1 feature. -->

- **Trường {A}**:
  - {Quy tắc 1} (báo lỗi: "{Thông báo lỗi}")
  - {Quy tắc 2} (báo lỗi: "{Thông báo lỗi}")
- **Trường {B}**:
  - {Quy tắc 1} (báo lỗi: "{Thông báo lỗi}")

---

## 5. Các bước triển khai

<!-- Mỗi task PHẢI có cột Implements trỏ về feature hoặc prd tương ứng -->

| # | ID | Implements | Bước | Trạng thái |
|---|----|------------|------|------------|
| 1 | `task:{tên}-0001` | `prd:tech-stack-{XXXX}` | {Mô tả bước} | ⬜ Chưa làm |
| 2 | `task:{tên}-0002` | `feature:{tên}-{XXXX}` | {Mô tả bước} | ⬜ Chưa làm |
| 3 | `task:{tên}-0003` | `feature:{tên}-{XXXX}` | {Mô tả bước} | ⬜ Chưa làm |

---

## Phụ lục: Bảng tổng hợp ID & Truy vết

<!-- Bảng này PHẢI chứa TẤT CẢ ID trong tài liệu. Cập nhật mỗi khi thêm/sửa ID. -->

| ID | Loại | Implements | Mô tả ngắn |
|----|------|------------|-------------|
| `prd:{...}-0001` | prd | — (gốc) | {Mô tả} |
| `prd:tech-stack-{XXXX}` | prd | — (gốc) | {Mô tả} |
| `feature:{...}-0001` | feature | `prd:{...}-0001` | {Mô tả} |
| `task:{...}-0001` | task | `prd:tech-stack-{XXXX}` | {Mô tả} |
| `task:{...}-0002` | task | `feature:{...}-0001` | {Mô tả} |
