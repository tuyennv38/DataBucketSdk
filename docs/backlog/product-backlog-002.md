# Product Backlog — DataBucketPlugin

> Trích xuất từ [PRD-002](../PRDs/PRD-002.md)

---

<a id="feature-wrapper-init-0001"></a>

### PBI-1: Init Wrapper
`feature:wrapper-init-0001`
> Implements: [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001)

- **User Story:** Với vai trò là Unity developer, tôi muốn gọi `DataBucketWrapper.Init()` để khởi tạo SDK với kiểm tra trùng lặp.
- **Acceptance Criteria:**
  - [ ] AC1: Gọi Init thành công, set `_isInitialized = true`
  - [ ] AC2: Gọi Init lần 2 → log warning, không gọi lại SDK
- **Priority:** Critical
- **Story Points:** 2
- **Status:** ✅ Done

---

<a id="feature-wrapper-record-0002"></a>

### PBI-2: Record Wrapper
`feature:wrapper-record-0002`
> Implements: [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001)

- **User Story:** Với vai trò là Unity developer, tôi muốn gọi `DataBucketWrapper.Record()` để ghi event với kiểm tra Init.
- **Acceptance Criteria:**
  - [ ] AC1: Gọi Record sau Init → forward sang SDK
  - [ ] AC2: Gọi Record trước Init → log error, không forward
- **Priority:** Critical
- **Story Points:** 1
- **Status:** ✅ Done

---

<a id="feature-wrapper-record-timing-0003"></a>

### PBI-3: RecordWithTiming Wrapper
`feature:wrapper-record-timing-0003`
> Implements: [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001)

- **User Story:** Với vai trò là Unity developer, tôi muốn gọi `DataBucketWrapper.RecordWithTiming()` để đo thời gian giữa 2 events.
- **Acceptance Criteria:**
  - [ ] AC1: Gọi RecordWithTiming sau Init → forward sang SDK
  - [ ] AC2: Gọi trước Init → log error
- **Priority:** High
- **Story Points:** 1
- **Status:** ✅ Done

---

<a id="feature-wrapper-set-property-0004"></a>

### PBI-4: SetCommonProperty Wrapper
`feature:wrapper-set-property-0004`
> Implements: [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001)

- **User Story:** Với vai trò là Unity developer, tôi muốn gọi `DataBucketWrapper.SetCommonProperty()` để set 1 property chung.
- **Acceptance Criteria:**
  - [ ] AC1: Gọi sau Init → forward sang SDK
  - [ ] AC2: Gọi trước Init → log error
- **Priority:** High
- **Story Points:** 1
- **Status:** ✅ Done

---

<a id="feature-wrapper-set-properties-0005"></a>

### PBI-5: SetCommonProperties Wrapper
`feature:wrapper-set-properties-0005`
> Implements: [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001)

- **User Story:** Với vai trò là Unity developer, tôi muốn gọi `DataBucketWrapper.SetCommonProperties()` để set nhiều properties cùng lúc.
- **Acceptance Criteria:**
  - [ ] AC1: Gọi sau Init → forward sang SDK
  - [ ] AC2: Gọi trước Init → log error
- **Priority:** High
- **Story Points:** 1
- **Status:** ✅ Done

---

<a id="feature-wrapper-end-session-0006"></a>

### PBI-6: ForceEndSession Wrapper
`feature:wrapper-end-session-0006`
> Implements: [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001)

- **User Story:** Với vai trò là Unity developer, tôi muốn gọi `DataBucketWrapper.ForceEndSession()` để kết thúc session thủ công.
- **Acceptance Criteria:**
  - [ ] AC1: Gọi sau Init → forward sang SDK
  - [ ] AC2: Gọi trước Init → log error
- **Priority:** Medium
- **Story Points:** 1
- **Status:** ✅ Done

---

<a id="feature-wrapper-exception-tracking-0007"></a>

### PBI-7: Exception Tracking Wrapper
`feature:wrapper-exception-tracking-0007`
> Implements: [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001)

- **User Story:** Với vai trò là Unity developer, tôi muốn bật/tắt exception tracking qua wrapper.
- **Acceptance Criteria:**
  - [ ] AC1: Enable/Disable sau Init → forward sang SDK
  - [ ] AC2: Gọi trước Init → log error
- **Priority:** Medium
- **Story Points:** 1
- **Status:** ✅ Done

---

<a id="feature-documentation-0008"></a>

### PBI-8: Documentation (README + CHANGE_LOG)
`feature:documentation-0008`
> Implements: [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001)

- **User Story:** Với vai trò là Unity developer, tôi muốn có tài liệu README và CHANGE_LOG rõ ràng để dễ tích hợp.
- **Acceptance Criteria:**
  - [ ] AC1: README.md hướng dẫn từng API, có code examples
  - [ ] AC2: CHANGE_LOG.md theo mẫu, có version 1.0.0
- **Priority:** High
- **Story Points:** 3
- **Status:** 📋 Backlog

---

<a id="feature-sample-script-0009"></a>

### PBI-9: Sample Script
`feature:sample-script-0009`
> Implements: [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001)

- **User Story:** Với vai trò là Unity developer, tôi muốn có script mẫu test tất cả API để tham khảo.
- **Acceptance Criteria:**
  - [ ] AC1: MonoBehaviour gắn vào GameObject
  - [ ] AC2: Chạy tất cả 7 API trong Start()
- **Priority:** Medium
- **Story Points:** 2
- **Status:** 📋 Backlog

---

## Phụ lục: Bảng tổng hợp ID & Truy vết

| ID | Loại | Implements | Mô tả ngắn |
|----|------|------------|-------------|
| [`feature:wrapper-init-0001`](#feature-wrapper-init-0001) | feature | [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001) | Init wrapper |
| [`feature:wrapper-record-0002`](#feature-wrapper-record-0002) | feature | [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001) | Record wrapper |
| [`feature:wrapper-record-timing-0003`](#feature-wrapper-record-timing-0003) | feature | [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001) | RecordWithTiming wrapper |
| [`feature:wrapper-set-property-0004`](#feature-wrapper-set-property-0004) | feature | [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001) | SetCommonProperty wrapper |
| [`feature:wrapper-set-properties-0005`](#feature-wrapper-set-properties-0005) | feature | [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001) | SetCommonProperties wrapper |
| [`feature:wrapper-end-session-0006`](#feature-wrapper-end-session-0006) | feature | [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001) | ForceEndSession wrapper |
| [`feature:wrapper-exception-tracking-0007`](#feature-wrapper-exception-tracking-0007) | feature | [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001) | Exception tracking wrapper |
| [`feature:documentation-0008`](#feature-documentation-0008) | feature | [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001) | README + CHANGE_LOG |
| [`feature:sample-script-0009`](#feature-sample-script-0009) | feature | [`prd:databucket-plugin-0001`](../PRDs/PRD-002.md#prd-databucket-plugin-0001) | Sample script |
