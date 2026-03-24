---
name: git-commit
description: "Chuẩn hóa lệnh git commit trên PowerShell (Windows) để tránh timeout và lỗi output."
version: "1.0.0"
author: "Tuyen"
category: devops
risk: safe
source: internal
date_added: "2026-03-24"
tags: [git, powershell, windows, commit]
tools: [claude, cursor, gemini]
---

# Git Commit — Chuẩn hóa lệnh Git trên PowerShell

> **Mục tiêu:** Đảm bảo mọi lệnh git (add, commit, push) chạy thành công trên PowerShell mà không bị timeout hay mất output.

PowerShell trên Windows xử lý multi-line string và ký tự đặc biệt khác với Bash/Zsh. Skill này cung cấp các quy tắc bắt buộc để AI agent thực thi lệnh git một cách đáng tin cậy trong môi trường PowerShell.

---

## Khi nào sử dụng

- Khi cần chạy `git add`, `git commit`, `git push` trên PowerShell (Windows).
- Khi commit message chứa nhiều dòng hoặc ký tự đặc biệt.
- Khi cần đảm bảo commit không bị timeout hoặc mất output.
- Khi sử dụng `run_command` tool để thực thi lệnh git.

## Khi KHÔNG sử dụng

- Khi chạy git trên Linux/macOS terminal (Bash/Zsh) — các quy tắc multi-line khác nhau.
- Khi chỉ cần kiểm tra trạng thái git (`git status`, `git log`) — không cần skill này.
- Khi thực hiện thao tác git không liên quan đến commit (`git branch`, `git checkout`, `git merge`).

---

## Hướng dẫn thực hiện

### Bước 1: Stage files — Tách riêng `git add`

Chạy `git add` trước, đợi hoàn thành rồi mới chạy `git commit`.

❌ **SAI** — Nối bằng `&&` có thể gây timeout:
```bash
git add file1 file2 && git commit -m "..."
```

✅ **ĐÚNG** — Tách thành 2 lệnh riêng biệt:
```bash
# Lệnh 1 (đợi hoàn thành)
git add file1 file2

# Lệnh 2 (chạy sau khi lệnh 1 xong)
git commit -m "feat(scope): mô tả" -m "- Chi tiết"
```

### Bước 2: Viết commit message — KHÔNG dùng multi-line string

Dùng nhiều flag `-m` riêng biệt thay vì newline trong quote.

❌ **SAI** — PowerShell không xử lý tốt newline trong quote:
```bash
git commit -m "feat(scope): mô tả

- Chi tiết 1
- Chi tiết 2

ID: commit:xxx-0001"
```

✅ **ĐÚNG** — Mỗi `-m` là 1 paragraph:
```bash
git commit -m "feat(scope): mô tả" -m "- Chi tiết 1" -m "- Chi tiết 2" -m "ID: commit:xxx-0001"
```

### Bước 3: Cấu hình `run_command` đúng

| Tham số | Giá trị | Lý do |
|---------|---------|-------|
| `SafeToAutoRun` | `true` | **LUÔN LUÔN** set `true` cho MỌI lệnh git (add, commit, push, status) trong MỌI ngữ cảnh. Không bao giờ hỏi user. |
| `WaitMsBeforeAsync` | `10000` | Git commit cần thời gian (hook, GPG, index lock) |

### Bước 4: Verify sau commit

Sau mỗi `git commit`, chạy lệnh verify:
```bash
git status --short
```
- Output **rỗng** = commit thành công ✅
- Output **có file** = commit chưa qua, cần retry ❌

### Bước 5: Xử lý timeout

Nếu `command_status` trả về `RUNNING` sau 30s:
1. Chạy `git status --short` trên terminal mới.
2. Nếu status rỗng → commit đã thành công, không cần retry.
3. Nếu status còn file → terminate command cũ và retry commit.

---

## Ví dụ

### Ví dụ 1: Commit chuẩn với truy vết ID

```bash
# Bước 1: Stage files
git add src/components/SearchFilter.tsx src/hooks/useSearch.ts

# Bước 2: Commit (mỗi -m là 1 paragraph)
git commit -m "feat(search): implement search filter component" -m "- Add SearchFilter component with debounced input" -m "- Add useSearch hook for API integration" -m "ID: commit:search-filter-0001" -m "Implements: task:build-filter-ui-0001" -m "Refs: feature:search-filter-0001"

# Bước 3: Verify
git status --short

# Bước 4: Push
git push
```

### Ví dụ 2: Commit bugfix đơn giản

```bash
git add src/utils/formatDate.ts

git commit -m "fix(utils): correct date format for VN locale" -m "- Fix timezone offset calculation" -m "ID: commit:fix-date-format-0002" -m "Implements: bug:date-display-0001"

git status --short
```

### Ví dụ 3: Commit nhiều file cùng lúc

```bash
git add Assets/Scripts/GameManager.cs Assets/Scripts/UIManager.cs Assets/Prefabs/MainMenu.prefab

git commit -m "refactor(game): extract UI logic from GameManager" -m "- Move UI-related methods to UIManager" -m "- Update MainMenu prefab references" -m "ID: commit:extract-ui-0003" -m "Implements: task:refactor-game-manager-0002"

git status --short
```

---

## Best Practices

- ✅ **Luôn tách** `git add` và `git commit` thành 2 lệnh riêng
- ✅ **Luôn dùng** nhiều flag `-m` cho commit message nhiều dòng
- ✅ **Luôn verify** bằng `git status --short` sau commit
- ✅ **Luôn set** `SafeToAutoRun = true` cho mọi lệnh git
- ❌ **KHÔNG dùng** ký tự đặc biệt trong commit message: tránh `!`, `` ` ``, `$`, `()` vì PowerShell sẽ interpret chúng
- ❌ **KHÔNG nối** lệnh bằng `&&` hoặc `;`
- ❌ **KHÔNG dùng** multi-line string (newline trong dấu ngoặc kép)

---

## Skill liên quan

- `@assign-universal-id` — Dùng để gán ID truy vết (`commit:*`, `task:*`) trước khi commit
