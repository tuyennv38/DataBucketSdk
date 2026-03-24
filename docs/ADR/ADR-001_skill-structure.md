<a id="adr-skill-structure-0001"></a>

# ADR-001: Cấu trúc Skill theo chuẩn skill-creator

`adr:skill-structure-0001`
> Implements: [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001)

## Bối cảnh (Context)

Cần tạo 7 agent skills cho Databuckets Unity SDK v1.0.6. Mỗi skill phải đảm bảo AI agent hiểu và sử dụng đúng API. Cần một chuẩn thống nhất cho cấu trúc và nội dung skill.

## Quyết định (Decision)

Áp dụng chuẩn `skill-creator` (`.agents/skills/skill-creator/SKILL.md`) cho tất cả 7 skills:

- **Mức độ:** Standard (300–800 từ)
- **Cấu trúc thư mục:** 1 skill tổng `databuckets-skills/` chứa 7 skill con (init/, record/, record-timing/, set-property/, set-properties/, end-session/, exception-tracking/)
- **Skill tổng** (`SKILL.md` ở root): Tổng quan SDK, hướng dẫn chọn skill con, auto-injected fields, PlayerPrefs keys
- **Skill con** (thư mục con): Mỗi skill chỉ có 1 file `SKILL.md`
- **Naming:** Thư mục con dùng kebab-case (không prefix `databuckets-`)
- **Sections bắt buộc:** Frontmatter → Title → Overview → When to Use → Prerequisites → Core Instructions → Examples (1 ví dụ) → Best Practices → Anti-Patterns → Related Skills

## Hệ quả (Consequences)

- ✅ Thống nhất cấu trúc giữa 7 skills → dễ bảo trì
- ✅ AI agent dễ parse và thực thi chính xác
- ✅ Có QA Checklist sẵn để kiểm tra chất lượng
- ⚠️ Mỗi skill chỉ có 1 ví dụ (user đã duyệt) → có thể cần bổ sung sau

## Trạng thái

- [x] Đề xuất (Proposed)
- [x] Được chấp thuận (Accepted)
