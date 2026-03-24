// @implements task:create-tracking-classes-0006
// @implements feature:user-properties-0010

using UnityEngine;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketUserProperties — Setter methods cho User Properties.
    /// Không phải event, gọi DataBucketWrapper.SetCommonProperty() bên dưới.
    /// Properties sẽ tự động đính kèm vào mọi event sau khi được set.
    /// </summary>
    /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#user-properties</remarks>
    public static class DataBucketUserProperties
    {
        private const string TAG = "[DataBucketUserProperties]";

        // ============================================================
        // LEVEL 2 — Quan trọng nhất
        // ============================================================

        /// <summary>
        /// Set level hiện tại của user. Cập nhật khi user bắt đầu chơi level mới.
        /// Property key: "current_level" (Number, Databuckets auto convert).
        /// </summary>
        /// <param name="level">Level hiện tại (>= 0). Cập nhật vào thời điểm user bắt đầu chơi level sau.</param>
        /// <remarks>Value Requirement: >= 0</remarks>
        public static void SetCurrentLevel(int level)
        {
            DataBucketWrapper.SetCommonProperty("current_level", level);
        }

        // ============================================================
        // LEVEL 3 — UA Attribution
        // ============================================================

        /// <summary>
        /// Set thông tin User Acquisition attribution từ MMP (Adjust/AppsFlyer/...).
        /// Gọi sau khi nhận callback data từ MMP.
        /// </summary>
        /// <param name="network">Ad network nguồn user (nullable)</param>
        /// <param name="campaign">Campaign nguồn user (nullable)</param>
        /// <param name="adgroup">Adgroup/Adset nguồn user (nullable)</param>
        /// <param name="creative">Creative nguồn user (nullable)</param>
        /// <param name="trackerName">Tracker name tổng hợp — chỉ có nếu dùng Adjust (nullable)</param>
        public static void SetUaAttribution(
            string network = null,
            string campaign = null,
            string adgroup = null,
            string creative = null,
            string trackerName = null)
        {
            if (network != null) DataBucketWrapper.SetCommonProperty("ua_network", network);
            if (campaign != null) DataBucketWrapper.SetCommonProperty("ua_campaign", campaign);
            if (adgroup != null) DataBucketWrapper.SetCommonProperty("ua_adgroup", adgroup);
            if (creative != null) DataBucketWrapper.SetCommonProperty("ua_creative", creative);
            if (trackerName != null) DataBucketWrapper.SetCommonProperty("ua_tracker_name", trackerName);
        }

        /// <summary>
        /// Set Firebase A/B test experiments mà user đang tham gia.
        /// Chỉ cần log khi bắn data trực tiếp lên Databuckets (không qua Firebase).
        /// </summary>
        /// <param name="experiments">Mảng experiment strings. VD: ["firebase_exp_1:0", "firebase_exp_2:1"]. Null nếu không tham gia.</param>
        public static void SetFirebaseExp(string[] experiments)
        {
            DataBucketWrapper.SetCommonProperty("firebase_exp", experiments);
        }

        /// <summary>
        /// Set số lượng tài nguyên hiện tại của user.
        /// Gọi mỗi khi resource balance thay đổi.
        /// </summary>
        /// <param name="resourceName">Tên resource (VD: "coin", "ticket", "freeze")</param>
        /// <param name="balance">Số lượng hiện tại (>= 0)</param>
        /// <remarks>
        /// Property key: "balance_{resourceName}_n" (hậu tố _n để Databuckets convert Number khi bắn qua Firebase).
        /// Value Requirement: >= 0
        /// </remarks>
        public static void SetResourceBalance(string resourceName, int balance)
        {
            DataBucketWrapper.SetCommonProperty($"balance_{resourceName}_n", balance);
        }

        // ============================================================
        // LEVEL 4 — Bổ sung
        // ============================================================

        /// <summary>
        /// Set user ID do client tạo, dùng mapping data với third-party/backend.
        /// Chỉ cần log khi cần mapping data.
        /// </summary>
        /// <param name="userId">UUID của user</param>
        public static void SetUserId(string userId)
        {
            DataBucketWrapper.SetCommonProperty("user_id", userId);
        }

        /// <summary>
        /// Set mode chơi hiện tại của user.
        /// </summary>
        /// <param name="mode">VD: "normal", "challenge", "endless"</param>
        /// <remarks>Value Requirement: normal | challenge | endless (tùy game)</remarks>
        public static void SetCurrentMode(string mode)
        {
            DataBucketWrapper.SetCommonProperty("current_mode", mode);
        }

        /// <summary>
        /// Set event (live ops) mà user đang tham gia.
        /// </summary>
        /// <param name="eventName">VD: "race", "double_reward"</param>
        public static void SetCurrentEvent(string eventName)
        {
            DataBucketWrapper.SetCommonProperty("current_event", eventName);
        }

        /// <summary>
        /// Set user có phải là IAP user hay không.
        /// Cập nhật từ false → true sau lần mua IAP đầu tiên.
        /// </summary>
        /// <param name="isIapUser">true nếu đã mua IAP ít nhất 1 lần</param>
        /// <remarks>Value Requirement: 0 | 1</remarks>
        public static void SetIsIapUser(bool isIapUser)
        {
            DataBucketWrapper.SetCommonProperty("is_iap_user_n", isIapUser ? 1 : 0);
        }

        /// <summary>
        /// Set số lần user đã mua IAP.
        /// </summary>
        /// <param name="count">Số lần mua IAP (>= 0)</param>
        /// <remarks>Value Requirement: >= 0</remarks>
        public static void SetIapCount(int count)
        {
            DataBucketWrapper.SetCommonProperty("iap_count_n", count);
        }

        /// <summary>
        /// Set số ngày active của user kể từ ngày cài game (D0).
        /// Chỉ tính ngày user thực sự mở game.
        /// VD: D0 cài → active_day=0, D1 không mở, D2 mở → active_day=1.
        /// </summary>
        /// <param name="day">Số ngày active (>= 0)</param>
        /// <remarks>Value Requirement: >= 0</remarks>
        public static void SetActiveDay(int day)
        {
            DataBucketWrapper.SetCommonProperty("active_day_n", day);
        }

        /// <summary>
        /// Set loại kết nối internet hiện tại.
        /// </summary>
        /// <param name="type">VD: "offline", "online", "wifi", "mobile_data", "unknown"</param>
        public static void SetConnectionType(string type)
        {
            DataBucketWrapper.SetCommonProperty("connection_type", type);
        }

        /// <summary>
        /// Set chuỗi thắng liên tiếp hiện tại.
        /// </summary>
        /// <param name="count">Số level thắng liên tiếp (>= 0)</param>
        /// <remarks>Value Requirement: >= 0</remarks>
        public static void SetWinStreak(int count)
        {
            DataBucketWrapper.SetCommonProperty("win_streak_n", count);
        }

        /// <summary>
        /// Set chuỗi thua liên tiếp hiện tại.
        /// </summary>
        /// <param name="count">Số level thua liên tiếp (>= 0)</param>
        /// <remarks>Value Requirement: >= 0</remarks>
        public static void SetLoseStreak(int count)
        {
            DataBucketWrapper.SetCommonProperty("lose_streak_n", count);
        }
    }
}
