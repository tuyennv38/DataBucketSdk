---
name: unity-plugin-skills
description: "Hướng dẫn cấu trúc chuẩn cho Unity Plugin, đảm bảo thống nhất thư mục, tài liệu, và version xuyên suốt dự án."
version: "1.0.0"
author: "Tuyen"
category: development
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, plugin, structure, csharp]
tools: [claude, cursor, gemini]
---

# Unity Plugin Skills — Chuẩn hóa cấu trúc Plugin cho Unity

> **Mục tiêu:** Đảm bảo mọi Unity Plugin trong dự án được tổ chức theo cấu trúc thống nhất, có đầy đủ tài liệu, và quản lý version rõ ràng.

Mỗi Plugin Unity cần tuân thủ một bộ khung thư mục chuẩn để dễ bảo trì, tái sử dụng, và tích hợp. Skill này định nghĩa cấu trúc bắt buộc và khuyến nghị cho mọi plugin.

---

## Khi nào sử dụng

- Khi tạo plugin Unity mới từ đầu.
- Khi chuẩn hóa lại (refactor) plugin hiện có cho đúng cấu trúc.
- Khi review cấu trúc plugin trước khi release hoặc chia sẻ.
- Khi cần kiểm tra plugin có đầy đủ thành phần bắt buộc chưa.

## Khi KHÔNG sử dụng

- Khi viết script nhỏ lẻ không đủ lớn để thành plugin (1–2 file tiện ích → để trong `Assets/Scripts/`).
- Khi tạo Unity Package Manager (UPM) package — UPM có convention riêng (`package.json`, `Runtime/`, `Editor/`).
- Khi làm việc với third-party SDK đã có sẵn cấu trúc riêng — không cần restructure.

---

## Hướng dẫn thực hiện

### Bước 1: Tạo cấu trúc thư mục

Mỗi plugin nằm trong một thư mục gốc dưới `Assets/`, với cấu trúc bắt buộc như sau:

```
Assets/
└── <PluginName>/                 ← PascalCase, mô tả chức năng plugin
    ├── Documents/                ← BẮT BUỘC: Tài liệu plugin
    │   ├── README.md             ← BẮT BUỘC: Hướng dẫn sử dụng
    │   ├── CHANGE_LOG.md         ← BẮT BUỘC: Lịch sử thay đổi theo version
    │   └── skills/               ← Tùy chọn: Skill hướng dẫn cho AI agent
    ├── Scripts/                  ← BẮT BUỘC: Source code C#
    │   ├── <PluginName>.cs       ← Entry point / Main class
    │   └── ...                   ← Các class phụ trợ
    ├── Editor/                   ← BẮT BUỘC: Editor scripts (Custom Inspector, Window, Menu)
    │   └── ...                   ← Chỉ chạy trong Unity Editor, không build vào game
    ├── Resources/                ← BẮT BUỘC: Assets load bằng Resources.Load()
    │   └── ...                   ← Prefabs, ScriptableObjects, Config files
    └── Samples/                  ← Tùy chọn: Code mẫu minh họa cách sử dụng
        └── <PluginName>Sample.cs
```

> **Quy tắc đặt tên thư mục gốc:** PascalCase, mô tả chức năng (ví dụ: `DataBucketPlugin`, `AdsManager`, `LocalizationKit`).

#### Bảng mô tả thư mục

| Thư mục | Bắt buộc | Mục đích | Nội dung |
|---------|:--------:|----------|----------|
| `Documents/` | ✅ | Tài liệu plugin | README.md, CHANGE_LOG.md, guides, skills |
| `Scripts/` | ✅ | Source code C# | Entry point, helper classes, utilities |
| `Editor/` | ✅ | Editor-only scripts | Custom Inspector, EditorWindow, MenuItem |
| `Resources/` | ✅ | Runtime assets | Prefabs, ScriptableObjects, configs |
| `Samples/` | ❌ | Code mẫu minh họa | Sample MonoBehaviour, test scripts |

### Bước 2: Tạo README.md

Đặt tại `<PluginName>/Documents/README.md`. File này là **tài liệu chính** của plugin.

**Format bắt buộc:**

```markdown
# <PluginName> — Mô tả ngắn

> **Version:** X.Y.Z  
> **Namespace:** `<PluginNamespace>`

Mô tả 2–3 câu: plugin làm gì, giải quyết vấn đề gì.

---

## Cài đặt

1. Bước cài đặt 1
2. Bước cài đặt 2

---

## Quick Start

{code ví dụ sử dụng cơ bản}

---

## API Reference

### Method 1 — Mô tả

{signature + bảng tham số + ví dụ}

---

## Bảng tổng hợp API

| Method | Mô tả |
|--------|-------|
| ... | ... |
```

**Các section BẮT BUỘC trong README.md:**

| Section | Mô tả |
|---------|-------|
| Header (H1 + Version) | Tên plugin, version, namespace |
| Cài đặt | Các bước cài plugin vào project |
| Quick Start | Code mẫu tối thiểu để bắt đầu sử dụng |
| API Reference | Chi tiết từng public method/class |
| Bảng tổng hợp API | Tóm tắt nhanh tất cả API |

### Bước 3: Tạo CHANGE_LOG.md

Đặt tại `<PluginName>/Documents/CHANGE_LOG.md`. Ghi lại **mọi thay đổi** theo version.

**Format bắt buộc:**

```markdown
# <PluginName> – Change Log

---

## <PluginName> - vX.Y.Z (DDth MM YYYY)

### Change

- **[NEW] ClassName:** Mô tả tính năng mới.
- **[FIX] ClassName:** Mô tả bug đã sửa.
- **[CHANGE] ClassName:** Mô tả thay đổi.
- **[REMOVE] ClassName:** Mô tả thành phần đã xóa.

---

## <PluginName> - vX.Y.Z-1 (DDth MM YYYY)

### Change

- ...
```

**Quy tắc tag thay đổi:**

| Tag | Ý nghĩa |
|-----|---------|
| `[NEW]` | Tính năng hoặc class mới |
| `[FIX]` | Sửa lỗi |
| `[CHANGE]` | Thay đổi behavior hoặc API |
| `[REMOVE]` | Xóa tính năng hoặc class |

**Quy tắc version**: Dùng **Semantic Versioning** (`X.Y.Z`):
- `X` (Major): Breaking changes, API thay đổi không tương thích ngược.
- `Y` (Minor): Thêm tính năng mới, tương thích ngược.
- `Z` (Patch): Sửa lỗi, tương thích ngược.

### Bước 4: Quản lý Version

Version **PHẢI** được khai báo ở **2 nơi** và luôn đồng bộ:

| Vị trí | Format | Ví dụ |
|--------|--------|-------|
| `Documents/README.md` header | `> **Version:** X.Y.Z` | `> **Version:** 1.0.1` |
| `Documents/CHANGE_LOG.md` entry | `## PluginName - vX.Y.Z (date)` | `## DataBucketPlugin - v1.0.1 (24th 03 2026)` |

**Quy trình cập nhật version:**
1. Xác định loại thay đổi → chọn bump Major/Minor/Patch.
2. Cập nhật version trong `README.md` header.
3. Thêm entry mới **ở đầu** `CHANGE_LOG.md` (mới nhất ở trên).
4. Liệt kê tất cả thay đổi với tag `[NEW]`/`[FIX]`/`[CHANGE]`/`[REMOVE]`.

### Bước 5: Thiết lập Editor scripts

Thư mục `Editor/` chứa các script **chỉ chạy trong Unity Editor**, không build vào game.

**Các loại script Editor phổ biến:**
- **Custom Inspector**: Tùy biến giao diện Inspector cho MonoBehaviour/ScriptableObject.
- **EditorWindow**: Cửa sổ tool riêng trong Editor.
- **MenuItem**: Thêm menu item vào Unity menu bar.
- **PropertyDrawer**: Tùy biến cách hiển thị field trong Inspector.

> **Lưu ý:** Unity tự động exclude thư mục `Editor/` khỏi build. Không cần cấu hình thêm.

### Bước 6: Thiết lập Resources

Thư mục `Resources/` chứa assets có thể load runtime bằng `Resources.Load<T>()`.

**Các loại assets phổ biến:**
- **ScriptableObject configs**: Cấu hình plugin (API key, endpoint, settings).
- **Prefabs**: UI prefab, popup, notification template.
- **TextAsset**: JSON/XML config, localization data.

> **⚠️ Cảnh báo:** Mọi file trong `Resources/` đều được đóng gói vào build, kể cả khi không sử dụng. Chỉ đặt file thực sự cần load runtime.

---

## Ví dụ

### Ví dụ 1: Plugin DataBucketPlugin (thực tế)

```
Assets/
└── DataBucketPlugin/
    ├── Documents/
    │   ├── README.md                    ← API guide, Quick Start, version
    │   ├── CHANGE_LOG.md                ← v1.0.0, v1.0.1 entries
    │   ├── DATA_TRACKING_GUIDE.md       ← Guide bổ sung
    │   └── skills/                      ← Skills cho AI agent
    │       └── databuckets-plugin-skills/
    ├── Scripts/
    │   ├── DataBucketWrapper.cs          ← Entry point (Init, Record, etc.)
    │   ├── DataBucketLevel.cs            ← Tracking class
    │   ├── DataBucketResource.cs         ← Tracking class
    │   ├── DataBucketIAP.cs              ← Tracking class
    │   └── ...                           ← 6 tracking classes khác
    └── Samples/
        └── DataBucketWrapperSample.cs    ← MonoBehaviour test script
```

README.md header:
```markdown
# DataBucketPlugin — Hướng dẫn sử dụng

> **Version:** 1.0.1  
> **SDK gốc:** Databuckets Unity SDK v1.0.6  
> **Namespace:** `DataBucketPlugin`
```

### Ví dụ 2: Plugin AdsManager (giả định)

```
Assets/
└── AdsManager/
    ├── Documents/
    │   ├── README.md
    │   └── CHANGE_LOG.md
    ├── Scripts/
    │   ├── AdsManager.cs                 ← Entry point
    │   ├── BannerAdController.cs
    │   ├── InterstitialAdController.cs
    │   └── RewardedAdController.cs
    ├── Editor/
    │   └── AdsManagerInspector.cs        ← Custom Inspector
    ├── Resources/
    │   └── AdsConfig.asset              ← ScriptableObject config
    └── Samples/
        └── AdsManagerSample.cs
```

README.md header:
```markdown
# AdsManager — Quản lý quảng cáo thống nhất

> **Version:** 1.0.0  
> **Namespace:** `AdsManager`
```

### Ví dụ 3: CHANGE_LOG entry đúng format

```markdown
# AdsManager – Change Log

---

## AdsManager - v1.1.0 (24th 03 2026)

### Change

- **[NEW] RewardedAdController:** Thêm class quản lý rewarded ads với callback.
- **[CHANGE] AdsManager:** Thêm method `ShowRewardedAd()` vào entry point.
- **[FIX] BannerAdController:** Sửa lỗi banner không hide khi chuyển scene.
- **README.md:** Cập nhật API Reference cho rewarded ads.

---

## AdsManager - v1.0.0 (20th 03 2026)

### Change

- **[NEW] AdsManager:** Tạo entry point với Init, ShowBanner, ShowInterstitial.
- **[NEW] BannerAdController:** Quản lý banner ads lifecycle.
- **[NEW] InterstitialAdController:** Quản lý interstitial ads lifecycle.
- **[NEW] AdsManagerInspector:** Custom Inspector hiển thị ad status.
- **[NEW] AdsConfig:** ScriptableObject lưu ad unit IDs.
```

---

## Best Practices

- ✅ **Luôn có README.md và CHANGE_LOG.md** trong `Documents/` — không bao giờ bỏ qua
- ✅ **Cập nhật tài liệu khi thay đổi code** — mọi thay đổi trong `Scripts/` hoặc `Editor/` **PHẢI** đi kèm cập nhật `README.md` (nếu API thay đổi) và/hoặc `CHANGE_LOG.md` (ghi nhận thay đổi). Không bao giờ commit code mà không cập nhật tài liệu tương ứng
- ✅ **Đồng bộ version** giữa README.md header và CHANGE_LOG.md entry mới nhất
- ✅ **Dùng PascalCase** cho tên thư mục plugin gốc
- ✅ **Dùng Semantic Versioning** (`X.Y.Z`) cho mọi version
- ✅ **CHANGE_LOG entry mới nhất ở trên** — reverse chronological order
- ✅ **Mỗi class có 1 file** — `ClassName.cs`, đặt đúng thư mục (`Scripts/` hoặc `Editor/`)
- ❌ **KHÔNG đặt Editor script trong `Scripts/`** — sẽ gây lỗi build
- ❌ **KHÔNG đặt file không cần thiết trong `Resources/`** — tăng build size
- ❌ **KHÔNG bỏ qua `.meta` file** — Unity cần `.meta` để track assets
- ❌ **KHÔNG commit code mà không cập nhật tài liệu** — xem Anti-Pattern 4

---

## Anti-Patterns (KHÔNG làm)

### ❌ Anti-Pattern 1: Plugin không có Documents

```
Assets/
└── MyPlugin/
    └── Scripts/
        ├── MyPlugin.cs
        └── Helper.cs
```

**Tại sao sai:** Không ai biết plugin làm gì, cách cài đặt, hay version nào đang dùng. Khi nhiều người cùng làm việc sẽ gây confusion.

**Cách đúng:**
```
Assets/
└── MyPlugin/
    ├── Documents/
    │   ├── README.md          ← Hướng dẫn sử dụng
    │   └── CHANGE_LOG.md      ← Lịch sử thay đổi
    ├── Scripts/
    │   ├── MyPlugin.cs
    │   └── Helper.cs
    ├── Editor/
    └── Resources/
```

### ❌ Anti-Pattern 2: Version không đồng bộ

```markdown
<!-- README.md -->
> **Version:** 1.2.0

<!-- CHANGE_LOG.md — entry mới nhất -->
## MyPlugin - v1.1.0 (...)
```

**Tại sao sai:** README ghi v1.2.0 nhưng CHANGE_LOG chưa có entry v1.2.0 → không biết v1.2.0 thay đổi gì.

**Cách đúng:** Cập nhật cả 2 file cùng lúc khi bump version.

### ❌ Anti-Pattern 3: Editor script nằm trong Scripts/

```
Assets/
└── MyPlugin/
    └── Scripts/
        ├── MyPlugin.cs
        └── MyPluginInspector.cs    ← Editor script nằm sai chỗ!
```

**Tại sao sai:** `MyPluginInspector.cs` dùng `UnityEditor` namespace → build lỗi trên device.

**Cách đúng:**
```
Assets/
└── MyPlugin/
    ├── Scripts/
    │   └── MyPlugin.cs
    └── Editor/
        └── MyPluginInspector.cs    ← Unity tự exclude khỏi build
```

### ❌ Anti-Pattern 4: Cập nhật code mà không cập nhật tài liệu

```csharp
// Thêm method mới vào Scripts/MyPlugin.cs
public static void NewFeature(string param) { ... }

// Nhưng KHÔNG cập nhật Documents/README.md
// Và KHÔNG thêm entry vào Documents/CHANGE_LOG.md
```

**Tại sao sai:** Người dùng plugin không biết có method mới. CHANGE_LOG không ghi nhận → không thể track lịch sử thay đổi. Gây mất đồng bộ giữa code và tài liệu.

**Cách đúng:** Mỗi khi thay đổi code, **BẮT BUỘC** cập nhật tài liệu tương ứng:

| Loại thay đổi code | Cập nhật README.md | Cập nhật CHANGE_LOG.md |
|---------------------|:------------------:|:----------------------:|
| Thêm public method/class mới | ✅ Thêm vào API Reference | ✅ Entry `[NEW]` |
| Sửa lỗi (bug fix) | ❌ Không cần (trừ khi behavior thay đổi) | ✅ Entry `[FIX]` |
| Thay đổi API signature | ✅ Cập nhật API Reference | ✅ Entry `[CHANGE]` |
| Xóa method/class | ✅ Xóa khỏi API Reference | ✅ Entry `[REMOVE]` |
| Refactor nội bộ (không đổi API) | ❌ Không cần | ✅ Entry `[CHANGE]` |

---

## Skill liên quan

- `@skill-creator` — Dùng khi cần tạo skill mới cho plugin
- `@assign-universal-id` — Dùng khi cần gán ID truy vết cho tài liệu plugin
- `@git-commit` — Dùng khi commit code plugin
