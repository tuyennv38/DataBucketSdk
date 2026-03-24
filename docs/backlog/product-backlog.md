# Product Backlog — Databuckets SDK Agent Skills

> **Nguồn:** [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001)
> **Architecture:** [`research:arch-design-0001`](../architecture/system-design.md#research-arch-design-0001)

---

<a id="feature-skill-init-0001"></a>

### PBI-1: Skill Init SDK

`feature:skill-init-0001`
> Implements: [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001)

- **User Story:** Với vai trò là AI agent, tôi muốn có skill hướng dẫn gọi `DatabucketsTracker.Init()` để khởi tạo SDK đúng cách.
- **Acceptance Criteria:**
  - [ ] SKILL.md theo chuẩn skill-creator (10 sections)
  - [ ] Code mẫu C# chạy được
  - [ ] Anti-patterns đủ rõ (Awake, gọi nhiều lần)
- **Priority:** Critical
- **Story Points:** 2
- **Status:** 📋 Backlog

---

<a id="feature-skill-record-0002"></a>

### PBI-2: Skill Record Event

`feature:skill-record-0002`
> Implements: [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001)

- **User Story:** Với vai trò là AI agent, tôi muốn có skill hướng dẫn gọi `DatabucketsTracker.Record()` để ghi nhận business events.
- **Acceptance Criteria:**
  - [ ] SKILL.md theo chuẩn skill-creator
  - [ ] Ví dụ với params Dictionary
  - [ ] Lưu ý auto-tracking events không trùng tên
- **Priority:** Critical
- **Story Points:** 2
- **Status:** 📋 Backlog

---

<a id="feature-skill-record-timing-0003"></a>

### PBI-3: Skill Record With Timing

`feature:skill-record-timing-0003`
> Implements: [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001)

- **User Story:** Với vai trò là AI agent, tôi muốn có skill hướng dẫn gọi `DatabucketsTracker.RecordWithTiming()` để đo thời gian giữa 2 events.
- **Acceptance Criteria:**
  - [ ] SKILL.md theo chuẩn skill-creator
  - [ ] Ví dụ cặp start/end event
  - [ ] Ghi rõ phải Record startEvent trước
- **Priority:** High
- **Story Points:** 3
- **Status:** 📋 Backlog

---

<a id="feature-skill-set-property-0004"></a>

### PBI-4: Skill Set Common Property

`feature:skill-set-property-0004`
> Implements: [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001)

- **User Story:** Với vai trò là AI agent, tôi muốn có skill hướng dẫn gọi `SetCommonProperty()` để set thuộc tính chung cho mọi event.
- **Acceptance Criteria:**
  - [ ] SKILL.md theo chuẩn skill-creator
  - [ ] Phân biệt rõ với SetCommonProperties
- **Priority:** Medium
- **Story Points:** 2
- **Status:** 📋 Backlog

---

<a id="feature-skill-set-properties-0005"></a>

### PBI-5: Skill Set Common Properties (Batch)

`feature:skill-set-properties-0005`
> Implements: [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001)

- **User Story:** Với vai trò là AI agent, tôi muốn có skill hướng dẫn gọi `SetCommonProperties()` để set nhiều thuộc tính cùng lúc.
- **Acceptance Criteria:**
  - [ ] SKILL.md theo chuẩn skill-creator
  - [ ] Phân biệt rõ với SetCommonProperty
- **Priority:** Medium
- **Story Points:** 2
- **Status:** 📋 Backlog

---

<a id="feature-skill-end-session-0006"></a>

### PBI-6: Skill End Session

`feature:skill-end-session-0006`
> Implements: [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001)

- **User Story:** Với vai trò là AI agent, tôi muốn có skill hướng dẫn gọi `ForceEndCurrentSession()` để kết thúc session thủ công.
- **Acceptance Criteria:**
  - [ ] SKILL.md theo chuẩn skill-creator
  - [ ] Ghi rõ chỉ dùng khi đặc biệt (logout, switch account)
- **Priority:** Low
- **Story Points:** 1
- **Status:** 📋 Backlog

---

<a id="feature-skill-exception-tracking-0007"></a>

### PBI-7: Skill Exception Tracking

`feature:skill-exception-tracking-0007`
> Implements: [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001)

- **User Story:** Với vai trò là AI agent, tôi muốn có skill hướng dẫn bật/tắt `EnableExceptionLogTracking()` để theo dõi Unity exceptions.
- **Acceptance Criteria:**
  - [ ] SKILL.md theo chuẩn skill-creator
  - [ ] Hướng dẫn Enable/Disable
- **Priority:** Low
- **Story Points:** 1
- **Status:** 📋 Backlog

---

## Tổng: 7 PBIs | 15 Story Points

---

## Phụ lục: Bảng tổng hợp ID & Truy vết

| ID | Loại | Implements | Mô tả ngắn |
|----|------|------------|-------------|
| [`feature:skill-init-0001`](#feature-skill-init-0001) | feature | [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001) | Skill Init SDK |
| [`feature:skill-record-0002`](#feature-skill-record-0002) | feature | [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001) | Skill Record Event |
| [`feature:skill-record-timing-0003`](#feature-skill-record-timing-0003) | feature | [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001) | Skill Record With Timing |
| [`feature:skill-set-property-0004`](#feature-skill-set-property-0004) | feature | [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001) | Skill Set Common Property |
| [`feature:skill-set-properties-0005`](#feature-skill-set-properties-0005) | feature | [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001) | Skill Set Common Properties |
| [`feature:skill-end-session-0006`](#feature-skill-end-session-0006) | feature | [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001) | Skill End Session |
| [`feature:skill-exception-tracking-0007`](#feature-skill-exception-tracking-0007) | feature | [`prd:databuckets-sdk-skills-0001`](../PRDs/PRD-001.md#prd-databuckets-sdk-skills-0001) | Skill Exception Tracking |
