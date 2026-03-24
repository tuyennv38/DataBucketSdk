---
name: skill-creator
description: "Hướng dẫn quy trình tạo skill mới có cấu trúc tốt, đảm bảo chất lượng và tính nhất quán theo chuẩn dự án."
---

# Skill Creator — Quy trình tạo Skill mới

> **Mục tiêu:** Mỗi khi cần tạo một skill mới, sử dụng skill này làm kim chỉ nam để đảm bảo skill được viết đúng cấu trúc, rõ ràng, và dễ bảo trì.

---

## 1. Khi nào sử dụng

- Khi nhận yêu cầu tạo một skill mới.
- Khi cần chuẩn hóa lại một skill hiện có (refactor).
- Khi muốn review/kiểm tra chất lượng của một skill đã viết.

## 2. Khi KHÔNG sử dụng

- Khi chỉ cần sửa nhỏ nội dung trong skill (typo, cập nhật link).
- Khi viết tài liệu không phải skill (README, PRD, workflow).

---

## 3. Quy trình tạo Skill (5 bước)

### Bước 1: Thu thập yêu cầu

Trước khi viết, trả lời các câu hỏi sau (tự suy luận hoặc hỏi user):

1. **Skill này giải quyết vấn đề gì?** — Mô tả ngắn gọn trong 1–2 câu.
2. **Khi nào AI agent nên kích hoạt skill này?** — Liệt kê các scenario cụ thể.
3. **Khi nào KHÔNG nên dùng?** — Tránh nhầm lẫn với skill/workflow khác.
4. **Có cần script/template/examples đi kèm không?** — Xác định các file bổ sung.
5. **Skill thuộc mức nào?** — Minimum (100–200 từ), Standard (300–800 từ), hay Comprehensive (800–2000 từ)?

### Bước 2: Tạo cấu trúc thư mục

```
.agents/skills/
└── <tên-skill>/              ← kebab-case, khớp với field `name` trong frontmatter
    ├── SKILL.md              ← BẮT BUỘC: File hướng dẫn chính
    ├── examples/             ← Tùy chọn: Ví dụ minh họa thực tế
    ├── scripts/              ← Tùy chọn: Script hỗ trợ (setup, validate, generate)
    ├── templates/            ← Tùy chọn: Template code tái sử dụng
    ├── references/           ← Tùy chọn: Tài liệu API, best practices
    └── README.md             ← Tùy chọn: Tài liệu bổ sung
```

> **Quy tắc:** Chỉ `SKILL.md` là bắt buộc. Thư mục con chỉ tạo khi thực sự cần.

### Bước 3: Viết SKILL.md

SKILL.md gồm **2 phần chính**: Frontmatter + Content.

---

#### 3A. Frontmatter (Metadata)

Frontmatter nằm ở đầu file, bọc trong `---`:

**Các field BẮT BUỘC:**

| Field | Quy tắc | Ví dụ |
|-------|---------|-------|
| `name` | kebab-case, **phải khớp** tên thư mục | `stripe-integration` |
| `description` | 1 câu, trong dấu ngoặc kép, **dưới 150 ký tự** | `"Stripe payment integration patterns"` |

**Các field TÙY CHỌN (khuyến khích):**

| Field | Mô tả | Ví dụ |
|-------|-------|-------|
| `version` | Phiên bản skill | `"1.0.0"` |
| `author` | Tác giả | `"Tuyen"` |
| `tags` | Nhãn phân loại | `[react, typescript, testing]` |
| `category` | Danh mục skill | `development` |
| `risk` | Mức rủi ro | `safe`, `moderate`, `high` |
| `source` | Nguồn gốc | `community`, `internal` |
| `date_added` | Ngày tạo | `"2026-03-24"` |
| `tools` | Tool tương thích | `[claude, cursor, gemini]` |

**Ví dụ frontmatter đầy đủ:**
```yaml
---
name: my-awesome-skill
description: "Hướng dẫn triển khai feature X với best practices"
version: "1.0.0"
author: "Tuyen"
category: development
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk]
tools: [claude, cursor, gemini]
---
```

---

#### 3B. Content (Nội dung)

Sau frontmatter là nội dung skill. **Tuân thủ thứ tự sections** sau:

##### Section 1: Title (H1) — BẮT BUỘC

```markdown
# Tên Skill — Tiêu đề mô tả
```
- Dùng tiêu đề rõ ràng, mở rộng từ `name`.

##### Section 2: Overview — BẮT BUỘC

```markdown
> **Mục tiêu:** 1 câu tóm tắt giá trị cốt lõi.

Giải thích ngắn 2–4 câu: skill này làm gì, tại sao cần nó.
```
- Trả lời câu hỏi **"WHY"** — tại sao skill này tồn tại.

##### Section 3: When to Use — BẮT BUỘC

```markdown
## Khi nào sử dụng
- Khi [scenario 1]
- Khi [scenario 2]

## Khi KHÔNG sử dụng
- Khi [anti-scenario 1]
```
- **Tại sao quan trọng:** Giúp AI biết khi nào kích hoạt skill. Nếu không có section này, AI có thể dùng sai ngữ cảnh.

##### Section 4: Prerequisites (nếu có)

```markdown
## Điều kiện tiên quyết
1. Đọc file `resources/xxx.md` đi kèm skill.
2. Cài đặt tool Y phiên bản Z.
```

##### Section 5: Core Instructions — BẮT BUỘC (Trái tim của skill)

```markdown
## Hướng dẫn thực hiện

### Bước 1: [Hành động]
Mô tả chi tiết, dùng động từ mệnh lệnh...

### Bước 2: [Hành động]
Tiếp tục...
```

- Đây là phần **quan trọng nhất**. Viết từng bước rõ ràng, có thể thực thi được.

##### Section 6: Examples — BẮT BUỘC (trừ Minimum skill)

```markdown
## Ví dụ

### Ví dụ 1: [Use Case]
```javascript
// Code mẫu thực tế
```

### Ví dụ 2: [Use Case khác]
```javascript
// Code mẫu khác
```
```

- **Tại sao ví dụ quan trọng:** Ví dụ cho AI thấy chính xác output mong muốn.
- Tối thiểu **2–3 ví dụ** thực tế, cover happy path + edge case.

##### Section 7: Best Practices — KHUYẾN KHÍCH

```markdown
## Best Practices
- ✅ Nên làm điều này
- ✅ Nên làm điều kia
- ❌ KHÔNG làm điều này
- ❌ Tránh điều kia
```

##### Section 8: Common Pitfalls / Anti-Patterns — KHUYẾN KHÍCH

```markdown
## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: [Tên]
```markdown
# SAI
<ví dụ sai>
```
**Tại sao sai:** <giải thích>
**Cách đúng:**
```markdown
# ĐÚNG
<ví dụ đúng>
```
```

##### Section 9: Related Skills — TÙY CHỌN

```markdown
## Skill liên quan
- `@other-skill` — Khi nào dùng thay thế
- `@complementary-skill` — Dùng phối hợp cùng
```

##### Section 10: Resources — TÙY CHỌN

```markdown
## Resources (Đóng gói trong skill)
| File | Mô tả | Cách sử dụng |
|------|-------|--------------|
| `resources/<file>` | <mô tả> | <hướng dẫn> |
| `templates/<file>` | <mô tả> | <hướng dẫn> |
```

---

### Bước 4: Áp dụng nguyên tắc viết hiệu quả

#### 4.1. Dùng ngôn ngữ trực tiếp, ra lệnh

❌ **SAI:**
```markdown
Bạn có thể cân nhắc việc kiểm tra xem user đã xác thực chưa.
```

✅ **ĐÚNG:**
```markdown
Kiểm tra user đã xác thực trước khi tiếp tục.
```

#### 4.2. Dùng động từ hành động

❌ **SAI:** `File nên được tạo...`
✅ **ĐÚNG:** `Tạo file...`

#### 4.3. Cụ thể, không mơ hồ

❌ **SAI:** `Thiết lập database cho đúng.`
✅ **ĐÚNG:**
```
1. Tạo PostgreSQL database
2. Chạy migration: `npm run migrate`
3. Seed dữ liệu: `npm run seed`
```

#### 4.4. Markdown formatting

| Yếu tố | Quy tắc |
|---------|---------|
| Code blocks | **Luôn** chỉ định language: ` ```javascript `, ` ```bash ` |
| Lists | Dùng `-` nhất quán, indent 2 space cho sub-items |
| **Bold** | Cho thuật ngữ quan trọng |
| *Italic* | Cho nhấn mạnh |
| `Code` | Cho commands, tên file, tên biến |
| Links | `[text](url)` |

#### 4.5. Ví dụ phải thực tế

- Tối thiểu **2–3 ví dụ** có code chạy được.
- Cover cả happy path và edge case.
- Không dùng placeholder vô nghĩa (`foo`, `bar`), dùng tên thực tế.

---

### Bước 5: Tự kiểm tra — QA Checklist

> ⚠️ **BẮT BUỘC** chạy checklist trước khi hoàn tất. Không skip.

#### Cấu trúc (Structure)
- [ ] Frontmatter hợp lệ YAML (giữa hai dòng `---`)?
- [ ] Field `name` khớp chính xác tên thư mục (kebab-case)?
- [ ] `description` dưới 150 ký tự, nêu rõ trọng tâm?
- [ ] Heading hierarchy đúng (H1 → H2 → H3), chỉ 1 H1?
- [ ] Sections theo đúng thứ tự khuyến nghị?

#### Nội dung (Content Quality)
- [ ] Có section "Khi nào sử dụng" rõ ràng?
- [ ] Có section "Khi KHÔNG sử dụng" (tránh nhầm lẫn)?
- [ ] Hướng dẫn sử dụng động từ mệnh lệnh, trực tiếp?
- [ ] Không có nội dung mơ hồ, chung chung?
- [ ] Không có lỗi chính tả, ngữ pháp?
- [ ] Thông tin kỹ thuật chính xác, up-to-date?

#### Hoàn chỉnh (Completeness)
- [ ] Overview giải thích **"WHY"**?
- [ ] Instructions giải thích **"HOW"**?
- [ ] Examples minh họa **"WHAT"**?
- [ ] Tối thiểu 2 ví dụ thực tế (trừ Minimum skill)?
- [ ] Edge cases được đề cập?

#### Tính dùng được (Usability)
- [ ] Người mới có thể hiểu và làm theo?
- [ ] Người có kinh nghiệm vẫn thấy hữu ích?
- [ ] AI agent có thể parse và thực thi chính xác?
- [ ] Skill giải quyết một vấn đề thực tế?

---

## 4. Hướng dẫn chọn mức độ chi tiết (Skill Size)

| Mức | Số từ | Sections bắt buộc | Extras | Khi nào dùng |
|-----|-------|--------------------|--------|--------------|
| **Minimum** | 100–200 | Overview + Instructions | Không | Skill đơn giản, 1–2 bước |
| **Standard** | 300–800 | Overview + When to Use + Instructions + Examples | Không | Phần lớn các skill |
| **Comprehensive** | 800–2000 | Tất cả sections | Scripts, Templates, References | Skill phức tạp, nhiều edge case |

> **Nguyên tắc:** Bắt đầu nhỏ, mở rộng dựa trên feedback thực tế.

---

## 5. Advanced Patterns

### 5.1. Conditional Logic — Xử lý theo ngữ cảnh

```markdown
Nếu user dùng React:
- Sử dụng functional components
- Ưu tiên hooks

Nếu user dùng Vue:
- Sử dụng Composition API
- Theo patterns Vue 3
```

### 5.2. Progressive Disclosure — Từ đơn giản đến nâng cao

```markdown
## Sử dụng cơ bản
[Hướng dẫn cho trường hợp phổ biến]

## Sử dụng nâng cao
[Patterns phức tạp cho power users]
```

### 5.3. Cross-References — Liên kết với skill/workflow khác

```markdown
## Skill liên quan
1. Dùng `@brainstorming` để thiết kế ý tưởng
2. Dùng `@skill-creator` để tạo cấu trúc skill
3. Dùng `@git-commit` để commit theo chuẩn
```

---

## 6. Đo lường chất lượng Skill (Effectiveness Metrics)

### Clarity Test
- Người không biết về chủ đề có thể làm theo không?
- Có chỗ nào nhập nhằng không?

### Completeness Test
- Cover được happy path?
- Xử lý edge cases?
- Có hướng dẫn khi gặp lỗi?

### Usefulness Test
- Giải quyết vấn đề thực tế?
- Bản thân bạn có muốn dùng không?
- Tiết kiệm thời gian hoặc nâng cao chất lượng?

---

## 7. Các lỗi phổ biến khi viết Skill

### ❌ Lỗi 1: Quá mơ hồ
```markdown
## Hướng dẫn
Viết code cho tốt.
```
✅ **Cách sửa:** Chia thành bước cụ thể: `1. Extract logic → 2. Add error handling → 3. Write tests`.

### ❌ Lỗi 2: Quá dài và phức tạp
```markdown
## Hướng dẫn
[5000 từ thuật ngữ kỹ thuật dày đặc]
```
✅ **Cách sửa:** Tách thành nhiều skill hoặc dùng Progressive Disclosure.

### ❌ Lỗi 3: Không có ví dụ
✅ **Cách sửa:** Thêm tối thiểu 2–3 ví dụ thực tế.

### ❌ Lỗi 4: `name` frontmatter không khớp tên thư mục
```yaml
# Thư mục: skills/my-awesome-skill/
name: myAwesomeSkill    # ← SAI! Phải là: my-awesome-skill
```

### ❌ Lỗi 5: Thông tin lỗi thời
```markdown
Dùng React class components...  # ← Lỗi thời, nên dùng functional + hooks
```
✅ **Cách sửa:** Luôn cập nhật theo best practices hiện tại.

---

## 8. Tài liệu tham khảo

| Tài liệu | Đường dẫn | Mô tả |
|-----------|-----------|-------|
| Skill Anatomy (chi tiết) | `docs/contributors/skill-anatomy.md` | Phân tích sâu từng phần của skill |
| Skill Template (bộ khung) | `docs/contributors/skill-template.md` | Template sẵn để copy & fill |
| Assign Universal ID | `.agents/skills/assign-universal-id/SKILL.md` | Ví dụ skill Comprehensive |
| Git Commit | `.agents/skills/git-commit/SKILL.md` | Ví dụ skill Standard |

---

## 💡 Pro Tips

1. **Bắt đầu từ "When to Use"** — Làm rõ mục đích trước khi viết chi tiết.
2. **Viết ví dụ trước** — Ví dụ giúp bạn hiểu rõ skill đang dạy gì.
3. **Test với AI agent** — Kiểm tra xem skill có thực sự hoạt động không.
4. **Lấy feedback** — Nhờ người khác review.
5. **Iterate** — Skill cải thiện theo thời gian dựa trên thực tế sử dụng.
