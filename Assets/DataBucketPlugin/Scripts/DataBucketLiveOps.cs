// @implements task:create-tracking-classes-0006
// @implements feature:liveops-tracking-0016

using System.Collections.Generic;
using UnityEngine;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketLiveOps — Live Ops feature tracking events.
    /// Events: feature_first_show, feature_open, feature_close.
    /// KPI: Đánh giá hiệu quả của các tính năng live ops.
    /// </summary>
    /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#live-ops</remarks>
    public static class DataBucketLiveOps
    {
        private const string TAG = "[DataBucketLiveOps]";

        /// <summary>
        /// [feature_first_show] Feature được show lần đầu (icon/popup gợi ý).
        /// Trigger: Khi icon/popup gợi ý user vào feature được show lần đầu.
        /// Với features mặc định show ngay từ đầu thì không cần log.
        /// </summary>
        /// <param name="featureName">Tên feature. VD: "daily_reward", "lucky_wheel"</param>
        /// <param name="placement">Vị trí show. VD: "home_icon", "home_popup", "end_level_icon". Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#feature_first_show</remarks>
        public static void FeatureFirstShow(string featureName, string placement = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "feature_name", featureName }
            };

            if (placement != null) eventParams["placement"] = placement;

            DataBucketWrapper.Record("feature_first_show", eventParams);
        }

        /// <summary>
        /// [feature_open] User mở feature.
        /// Trigger: Khi user mở tính năng.
        /// </summary>
        /// <param name="featureName">Tên feature</param>
        /// <param name="placement">Vị trí mở. Nullable.</param>
        /// <param name="openType">Cách mở: "popup" (tự động) | "click" (user chủ động). Nullable.</param>
        /// <param name="openIndex">Lần thứ mấy user mở loại feature này (>= 1). Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#feature_open</remarks>
        public static void FeatureOpen(
            string featureName,
            string placement = null,
            string openType = null,
            int? openIndex = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "feature_name", featureName }
            };

            if (placement != null) eventParams["placement"] = placement;
            if (openType != null) eventParams["open_type"] = openType;
            if (openIndex.HasValue) eventParams["open_index"] = openIndex.Value;

            DataBucketWrapper.Record("feature_open", eventParams);
        }

        /// <summary>
        /// [feature_close] User thoát/đóng tính năng sau khi dùng xong.
        /// Trigger: Khi user click X hoặc back để thoát/đóng feature.
        /// </summary>
        /// <param name="featureName">Tên feature</param>
        /// <param name="placement">Vị trí mở feature. Nullable.</param>
        /// <param name="openIndex">Lần thứ mấy user mở (>= 1). Nullable.</param>
        /// <param name="durationFeature">Thời gian ở trong feature, msec (> 0). Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#feature_close</remarks>
        public static void FeatureClose(
            string featureName,
            string placement = null,
            int? openIndex = null,
            long? durationFeature = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "feature_name", featureName }
            };

            if (placement != null) eventParams["placement"] = placement;
            if (openIndex.HasValue) eventParams["open_index"] = openIndex.Value;
            if (durationFeature.HasValue) eventParams["duration_feature"] = durationFeature.Value;

            DataBucketWrapper.Record("feature_close", eventParams);
        }
    }
}
