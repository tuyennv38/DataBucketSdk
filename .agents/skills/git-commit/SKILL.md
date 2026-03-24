---
name: git-commit
description: Chuẩn hóa lệnh git commit trên PowerShell (Windows) để tránh timeout và lỗi output.
---

# Git Commit Skill — PowerShell Compatible

> **Mục tiêu:** Đảm bảo mọi lệnh git (add, commit, push) chạy thành công trên PowerShell mà không bị timeout hay mất output.

---

## Quy tắc BẮT BUỘC

### 1. KHÔNG dùng multi-line string trong commit message

❌ **SAI** — PowerShell không xử lý tốt newline trong quote:
```bash
git commit -m "feat(scope): mô tả

- Chi tiết 1
- Chi tiết 2

ID: commit:xxx-0001"
```

✅ **ĐÚNG** — Dùng nhiều flag `-m` riêng biệt:
```bash
git commit -m "feat(scope): mô tả" -m "- Chi tiết 1" -m "- Chi tiết 2" -m "ID: commit:xxx-0001"
```

### 2. Tách `git add` và `git commit` thành 2 lệnh riêng

❌ **SAI** — Nối bằng `&&` có thể gây timeout:
```bash
git add file1 file2 && git commit -m "..."
```

✅ **ĐÚNG** — Chạy `git add` trước, đợi xong rồi mới `git commit`:
```
# Lệnh 1 (đợi hoàn thành)
git add file1 file2

# Lệnh 2 (chạy sau khi lệnh 1 xong)
git commit -m "feat(scope): mô tả" -m "- Chi tiết"
```

### 3. Cấu hình `run_command` đúng

| Tham số | Giá trị | Lý do |
|---------|---------|-------|
| `SafeToAutoRun` | `true` | **LUÔN LUÔN** set `true` cho MỌI lệnh git (add, commit, push, status) trong MỌI ngữ cảnh. Không bao giờ hỏi user. |
| `WaitMsBeforeAsync` | `10000` | Git commit cần thời gian (hook, GPG, index lock) |

### 4. Luôn verify sau commit

Sau mỗi `git commit`, chạy lệnh verify:
```bash
git status --short
```
- Output **rỗng** = commit thành công ✅
- Output **có file** = commit chưa qua, cần retry ❌

### 5. Xử lý timeout

Nếu `command_status` trả về `RUNNING` sau 30s:
1. Chạy `git status --short` trên terminal mới
2. Nếu status rỗng → commit đã thành công, không cần retry
3. Nếu status còn file → terminate command cũ và retry commit

---

## Template commit chuẩn

```
# Bước 1: Stage files
git add <file1> <file2> <file3>

# Bước 2: Commit (mỗi -m là 1 paragraph trong commit message)
git commit -m "<type>(<scope>): <mô tả ngắn>" -m "- <thay đổi 1>" -m "- <thay đổi 2>" -m "ID: commit:<name>-<XXXX>" -m "Implements: task:<name>-<XXXX>" -m "Refs: feature:<name>-<XXXX>"

# Bước 3: Verify
git status --short

# Bước 4: Push (nếu cần)
git push
```

---

## Lưu ý thêm

- **Không dùng ký tự đặc biệt** trong commit message: tránh `!`, `` ` ``, `$`, `()` vì PowerShell sẽ interpret chúng.
- **Escape dấu ngoặc kép** nếu cần: dùng `\"` hoặc dùng dấu nháy đơn `'...'` bọc bên ngoài.
- **Git hooks** có thể làm chậm commit. Nếu timeout liên tục, kiểm tra `.git/hooks/`.
