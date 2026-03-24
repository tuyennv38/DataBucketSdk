// @implements task:create-tracking-classes-0006
// @implements feature:level-analytics-0011

using System.Collections.Generic;
using UnityEngine;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketLevel — Level analytics events.
    /// Events: level_start, level_end, level_exit, level_reopen.
    /// </summary>
    /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#level-analytics</remarks>
    public static class DataBucketLevel
    {
        private const string TAG = "[DataBucketLevel]";

        // ============================================================
        // LEVEL START
        // ============================================================

        /// <summary>
        /// [level_start] User bắt đầu chơi 1 level.
        /// Trigger: Khi user bắt đầu chơi 1 level.
        /// </summary>
        /// <param name="level">Level mà user chơi (>= 1)</param>
        /// <param name="durationTotalStart">Thời gian cho phép chơi ban đầu của level, tính bằng msec (> 0)</param>
        /// <param name="loopBy">Level lấy content từ level nào (0 = không lặp). Nullable.</param>
        /// <param name="playType">Hoàn cảnh chơi: "home" | "next" | "restart". Nullable.</param>
        /// <param name="playIndex">Lần thứ mấy user chơi level này (>= 1). Nullable.</param>
        /// <param name="loseIndex">Số lần thua trước đó (>= 0, lần đầu = 0). Nullable.</param>
        /// <param name="mode">Mode chơi: "normal" | "challenge" | "endless". Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#level_start</remarks>
        public static void LevelStart(
            int level,
            long durationTotalStart,
            int? loopBy = null,
            string playType = null,
            int? playIndex = null,
            int? loseIndex = null,
            string mode = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "level", level },
                { "duration_total_start", durationTotalStart }
            };

            if (loopBy.HasValue) eventParams["loop_by"] = loopBy.Value;
            if (playType != null) eventParams["play_type"] = playType;
            if (playIndex.HasValue) eventParams["play_index"] = playIndex.Value;
            if (loseIndex.HasValue) eventParams["lose_index"] = loseIndex.Value;
            if (mode != null) eventParams["mode"] = mode;

            DataBucketWrapper.Record("level_start", eventParams);
        }

        // ============================================================
        // LEVEL END
        // ============================================================

        /// <summary>
        /// [level_end] User kết thúc quá trình chơi 1 level.
        /// Trigger: Khi user kết thúc màn chơi (win/lose/quit/exit).
        /// </summary>
        /// <param name="level">Level mà user chơi (>= 1)</param>
        /// <param name="result">Kết quả: "win" | "lose" | "quit" | "exit"</param>
        /// <param name="durationPlay">Thời gian chơi, msec (>= 0, không tính thời gian xem ad)</param>
        /// <param name="loseBy">Lý do thua. "null" nếu win/quit/exit. VD: "out_of_move" | "out_of_time". Nullable.</param>
        /// <param name="loopBy">Level lấy content từ level nào (0 = không lặp). Nullable.</param>
        /// <param name="playType">Hoàn cảnh chơi: "home" | "next" | "restart". Nullable.</param>
        /// <param name="playIndex">Lần thứ mấy user chơi level này. Nullable.</param>
        /// <param name="loseIndex">Số lần thua trước đó. Nullable.</param>
        /// <param name="durationTotalStart">Thời gian cho phép chơi ban đầu, msec. Nullable.</param>
        /// <param name="durationTotalEnd">Tổng thời gian cho phép chơi (có thể > start nếu dùng booster), msec. Nullable.</param>
        /// <param name="durationRemain">Thời gian còn lại khi end, msec. Nullable.</param>
        /// <param name="itemsTotal">Tổng số item cần xử lý trong level. Nullable.</param>
        /// <param name="itemsCleared">Số item đã xử lý được. Nullable.</param>
        /// <param name="actionSeq">Chuỗi hành động: "3,6,1,2,4" (không dấu cách). "0" nếu không action. Nullable.</param>
        /// <param name="mode">Mode chơi. Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#level_end</remarks>
        public static void LevelEnd(
            int level,
            string result,
            long durationPlay,
            string loseBy = null,
            int? loopBy = null,
            string playType = null,
            int? playIndex = null,
            int? loseIndex = null,
            long? durationTotalStart = null,
            long? durationTotalEnd = null,
            long? durationRemain = null,
            int? itemsTotal = null,
            int? itemsCleared = null,
            string actionSeq = null,
            string mode = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "level", level },
                { "result", result },
                { "duration_play", durationPlay }
            };

            if (loseBy != null) eventParams["lose_by"] = loseBy;
            if (loopBy.HasValue) eventParams["loop_by"] = loopBy.Value;
            if (playType != null) eventParams["play_type"] = playType;
            if (playIndex.HasValue) eventParams["play_index"] = playIndex.Value;
            if (loseIndex.HasValue) eventParams["lose_index"] = loseIndex.Value;
            if (durationTotalStart.HasValue) eventParams["duration_total_start"] = durationTotalStart.Value;
            if (durationTotalEnd.HasValue) eventParams["duration_total_end"] = durationTotalEnd.Value;
            if (durationRemain.HasValue) eventParams["duration_remain"] = durationRemain.Value;
            if (itemsTotal.HasValue) eventParams["items_total"] = itemsTotal.Value;
            if (itemsCleared.HasValue) eventParams["items_cleared"] = itemsCleared.Value;
            if (actionSeq != null) eventParams["action_seq"] = actionSeq;
            if (mode != null) eventParams["mode"] = mode;

            DataBucketWrapper.Record("level_end", eventParams);
        }

        // ============================================================
        // LEVEL EXIT
        // ============================================================

        /// <summary>
        /// [level_exit] User thoát game khi đang chơi dở level.
        /// Trigger: Khi user rời khỏi game giữa màn chơi (kill app, background...).
        /// Chỉ cần log nếu KHÔNG log level_end với result="exit".
        /// </summary>
        /// <param name="level">Level mà user chơi (>= 1)</param>
        /// <param name="loopBy">Level lấy content từ level nào. Nullable.</param>
        /// <param name="playType">Hoàn cảnh chơi. Nullable.</param>
        /// <param name="playIndex">Lần thứ mấy user chơi. Nullable.</param>
        /// <param name="loseIndex">Số lần thua trước đó. Nullable.</param>
        /// <param name="exitIndex">Lần exit thứ mấy trong lần chơi này (>= 1, reset mỗi level_start). Nullable.</param>
        /// <param name="durationTotalStart">Thời gian cho phép ban đầu, msec. Nullable.</param>
        /// <param name="durationTotalEnd">Tổng thời gian cho phép, msec. Nullable.</param>
        /// <param name="durationRemain">Thời gian còn lại, msec. Nullable.</param>
        /// <param name="durationPlay">Thời gian đã chơi, msec. Nullable.</param>
        /// <param name="itemsTotal">Tổng item cần xử lý. Nullable.</param>
        /// <param name="itemsCleared">Số item đã xử lý. Nullable.</param>
        /// <param name="actionSeq">Chuỗi hành động. Nullable.</param>
        /// <param name="mode">Mode chơi. Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#level_exit</remarks>
        public static void LevelExit(
            int level,
            int? loopBy = null,
            string playType = null,
            int? playIndex = null,
            int? loseIndex = null,
            int? exitIndex = null,
            long? durationTotalStart = null,
            long? durationTotalEnd = null,
            long? durationRemain = null,
            long? durationPlay = null,
            int? itemsTotal = null,
            int? itemsCleared = null,
            string actionSeq = null,
            string mode = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "level", level }
            };

            if (loopBy.HasValue) eventParams["loop_by"] = loopBy.Value;
            if (playType != null) eventParams["play_type"] = playType;
            if (playIndex.HasValue) eventParams["play_index"] = playIndex.Value;
            if (loseIndex.HasValue) eventParams["lose_index"] = loseIndex.Value;
            if (exitIndex.HasValue) eventParams["exit_index"] = exitIndex.Value;
            if (durationTotalStart.HasValue) eventParams["duration_total_start"] = durationTotalStart.Value;
            if (durationTotalEnd.HasValue) eventParams["duration_total_end"] = durationTotalEnd.Value;
            if (durationRemain.HasValue) eventParams["duration_remain"] = durationRemain.Value;
            if (durationPlay.HasValue) eventParams["duration_play"] = durationPlay.Value;
            if (itemsTotal.HasValue) eventParams["items_total"] = itemsTotal.Value;
            if (itemsCleared.HasValue) eventParams["items_cleared"] = itemsCleared.Value;
            if (actionSeq != null) eventParams["action_seq"] = actionSeq;
            if (mode != null) eventParams["mode"] = mode;

            DataBucketWrapper.Record("level_exit", eventParams);
        }

        // ============================================================
        // LEVEL REOPEN
        // ============================================================

        /// <summary>
        /// [level_reopen] User quay lại game và chơi tiếp level đang chơi dở.
        /// Trigger: Khi user quay lại game sau level_exit và chơi tiếp.
        /// Nếu user phải chơi lại từ đầu → dùng LevelStart() thay vì LevelReopen().
        /// Chỉ cần log nếu KHÔNG log level_end với result="exit".
        /// </summary>
        /// <param name="level">Level mà user chơi (>= 1)</param>
        /// <param name="loopBy">Level lấy content từ level nào. Nullable.</param>
        /// <param name="playIndex">Lần thứ mấy user chơi. Nullable.</param>
        /// <param name="loseIndex">Số lần thua trước đó. Nullable.</param>
        /// <param name="durationTotalStart">Thời gian cho phép ban đầu, msec. Nullable.</param>
        /// <param name="mode">Mode chơi. Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#level_reopen</remarks>
        public static void LevelReopen(
            int level,
            int? loopBy = null,
            int? playIndex = null,
            int? loseIndex = null,
            long? durationTotalStart = null,
            string mode = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "level", level }
            };

            if (loopBy.HasValue) eventParams["loop_by"] = loopBy.Value;
            if (playIndex.HasValue) eventParams["play_index"] = playIndex.Value;
            if (loseIndex.HasValue) eventParams["lose_index"] = loseIndex.Value;
            if (durationTotalStart.HasValue) eventParams["duration_total_start"] = durationTotalStart.Value;
            if (mode != null) eventParams["mode"] = mode;

            DataBucketWrapper.Record("level_reopen", eventParams);
        }
    }
}
