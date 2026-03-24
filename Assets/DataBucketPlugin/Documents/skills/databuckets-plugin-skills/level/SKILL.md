---
name: level
description: "Hướng dẫn sử dụng DataBucketLevel để track level_start, level_end, level_exit, level_reopen."
version: "1.0.1"
author: "Tuyen"
category: sdk
risk: safe
source: internal
date_added: "2026-03-24"
tags: [unity, csharp, sdk, analytics, level]
---

# DataBucketLevel — Level Analytics

> **Mục tiêu:** Hướng dẫn sử dụng `DataBucketLevel` để track vòng đời level: start → end/exit → reopen.

---

## Khi nào sử dụng

- Khi user bắt đầu, kết thúc, thoát giữa chừng, hoặc quay lại level.
- Khi cần phân tích win/lose rate, play duration, drop-off level.

## Khi KHÔNG sử dụng

- Khi track resource earn/spend trong level → dùng `databuckets-plugin-skills/resource`.
- Khi track IAP/Ad trong level → dùng skill tương ứng.

---

## Hướng dẫn thực hiện

### Method 1: LevelStart

Gọi khi user bắt đầu chơi 1 level.

```csharp
DataBucketLevel.LevelStart(
    int level,                    // Level number (>=1)
    long durationTotalStart,      // Thời gian cho phép, msec (>0)
    string playType = null,       // "home" | "next" | "restart"
    int? playIndex = null,        // Lần chơi thứ mấy (>=1)
    int? loseIndex = null,        // Số lần thua trước (>=0, lần đầu=0)
    int? loopBy = null,           // Level lấy content (0=không lặp)
    string mode = null            // "normal" | "challenge" | "endless"
);
```

### Method 2: LevelEnd

Gọi khi user kết thúc màn chơi (win/lose/quit/exit).

```csharp
DataBucketLevel.LevelEnd(
    int level,                    // Level number
    string result,                // "win" | "lose" | "quit" | "exit"
    long durationPlay,            // Thời gian chơi, msec (>=0)
    string playType = null,
    int? playIndex = null,
    int? loseIndex = null,
    string loseBy = null,         // "out_of_move" | "out_of_time" | null
    long? durationTotalStart = null,
    long? durationTotalEnd = null, // >= durationTotalStart nếu dùng booster
    long? durationRemain = null,
    int? itemsTotal = null,
    int? itemsCleared = null,
    string actionSeq = null,      // "3,6,1,2,4" (không dấu cách)
    int? loopBy = null,
    string mode = null
);
```

### Method 3: LevelExit

Gọi khi user thoát game giữa level (kill app, background).

```csharp
DataBucketLevel.LevelExit(
    int level, long durationPlay,
    // ... params tương tự LevelEnd
);
```

### Method 4: LevelReopen

Gọi khi user quay lại game và chơi tiếp level dở.

```csharp
DataBucketLevel.LevelReopen(
    int level, long durationTotalStart,
    int? playIndex = null, int? loseIndex = null,
    int? loopBy = null, string mode = null
);
```

---

## Ví dụ

### Ví dụ 1: Flow cơ bản — user chơi và thắng

```csharp
// User bấm Play từ home
DataBucketLevel.LevelStart(level: 5, durationTotalStart: 60000, playType: "home", playIndex: 1, loseIndex: 0);

// User win sau 45 giây
DataBucketLevel.LevelEnd(
    level: 5, result: "win", durationPlay: 45000,
    playType: "home", playIndex: 1, loseIndex: 0,
    durationTotalStart: 60000, durationTotalEnd: 60000,
    durationRemain: 15000, itemsTotal: 30, itemsCleared: 30
);
```

### Ví dụ 2: User thua rồi chơi lại

```csharp
// Lần 1: thua
DataBucketLevel.LevelStart(level: 10, durationTotalStart: 90000, playType: "home", playIndex: 1, loseIndex: 0);
DataBucketLevel.LevelEnd(level: 10, result: "lose", durationPlay: 90000, loseBy: "out_of_move", playIndex: 1, loseIndex: 0);

// Lần 2: restart
DataBucketLevel.LevelStart(level: 10, durationTotalStart: 90000, playType: "restart", playIndex: 2, loseIndex: 1);
```

### Ví dụ 3: User thoát game giữa chừng

```csharp
DataBucketLevel.LevelStart(level: 3, durationTotalStart: 60000, playType: "home");

// User kill app sau 20 giây
DataBucketLevel.LevelExit(level: 3, durationPlay: 20000, durationTotalStart: 60000, durationRemain: 40000);

// User quay lại, chơi tiếp
DataBucketLevel.LevelReopen(level: 3, durationTotalStart: 60000);
```

---

## Best Practices

- ✅ `result="exit"` trong LevelEnd **hoặc** dùng LevelExit — chọn 1, không dùng cả 2
- ✅ `durationPlay` không tính thời gian xem quảng cáo
- ✅ `durationRemain = durationTotalEnd - durationPlay`
- ✅ Sau LevelEnd với result="win", gọi `DataBucketUserProperties.SetCurrentLevel(level + 1)`
- ❌ KHÔNG gọi LevelEnd 2 lần cho cùng 1 level session

---

## Skill liên quan

- `databuckets-plugin-skills/user-properties` — Cập nhật current_level sau khi win
- `databuckets-plugin-skills/resource` — Track resource earn/spend trong level
- `databuckets-plugin-skills/ad` — Track ad xem trong level
