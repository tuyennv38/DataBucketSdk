// @implements task:create-tracking-classes-0006
// @implements feature:technical-tracking-0018

using System.Collections.Generic;
using UnityEngine;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketTechnical — Technical performance tracking events.
    /// Events: loading_start, loading_finish.
    /// KPI: Time to data, Success rate.
    /// </summary>
    /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#technical-performance</remarks>
    public static class DataBucketTechnical
    {
        private const string TAG = "[DataBucketTechnical]";

        // ============================================================
        // LOADING START
        // ============================================================

        /// <summary>
        /// [loading_start] Bắt đầu tiến trình tải dữ liệu (API/CDN/SDK).
        /// Trigger: Khi app phát động gọi dữ liệu.
        /// KPI: Time to data, Success rate.
        /// </summary>
        /// <param name="dataSource">Nguồn dữ liệu: "api" | "cdn" | "sdk" | "store" | "remote_config"</param>
        /// <param name="resourceType">Loại dữ liệu: "feed" | "content_detail" | "search" | "user_profile" | "paywall"</param>
        /// <param name="loadContext">Màn hình/luồng nơi gọi: "app_open" | "home" | "content_detail"</param>
        /// <param name="triggerSource">Nguyên nhân: "screen_appear" | "user_action" | "background" | "prefetch". Nullable.</param>
        /// <param name="loadId">UUID duy nhất của request (cần thiết nếu nhiều request song song). Nullable.</param>
        /// <param name="priority">Độ ưu tiên: "critical" | "high" | "normal" | "low" | "prefetch". Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#loading_start</remarks>
        public static void LoadingStart(
            string dataSource,
            string resourceType,
            string loadContext,
            string triggerSource = null,
            string loadId = null,
            string priority = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "data_source", dataSource },
                { "resource_type", resourceType },
                { "load_context", loadContext }
            };

            if (triggerSource != null) eventParams["trigger_source"] = triggerSource;
            if (loadId != null) eventParams["load_id"] = loadId;
            if (priority != null) eventParams["priority"] = priority;

            DataBucketWrapper.Record("loading_start", eventParams);
        }

        // ============================================================
        // LOADING FINISH
        // ============================================================

        /// <summary>
        /// [loading_finish] Kết thúc tiến trình tải dữ liệu (thành công hoặc thất bại).
        /// Trigger: Khi nhận được kết quả của request.
        /// KPI: Time to data, Success rate.
        /// </summary>
        /// <param name="dataSource">Nguồn dữ liệu</param>
        /// <param name="resourceType">Loại dữ liệu</param>
        /// <param name="loadContext">Màn hình/luồng nơi gọi</param>
        /// <param name="result">Kết quả: "success" | "fail" | "canceled" | "timeout" | "offline"</param>
        /// <param name="loadTime">Thời gian từ start đến finish, msec (>= 0)</param>
        /// <param name="triggerSource">Nguyên nhân khởi phát. Nullable.</param>
        /// <param name="loadId">UUID duy nhất. Nullable.</param>
        /// <param name="priority">Độ ưu tiên. Nullable.</param>
        /// <param name="errorCode">Mã lỗi: "network_timeout" | "server_error" | "http_404_not_found". Nullable.</param>
        /// <param name="responseBytes">Kích thước response, bytes. Nullable.</param>
        /// <param name="retryCount">Số lần thử lại (0 = thành công lần đầu). Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#loading_finish</remarks>
        public static void LoadingFinish(
            string dataSource,
            string resourceType,
            string loadContext,
            string result,
            long loadTime,
            string triggerSource = null,
            string loadId = null,
            string priority = null,
            string errorCode = null,
            long? responseBytes = null,
            int? retryCount = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "data_source", dataSource },
                { "resource_type", resourceType },
                { "load_context", loadContext },
                { "result", result },
                { "load_time", loadTime }
            };

            if (triggerSource != null) eventParams["trigger_source"] = triggerSource;
            if (loadId != null) eventParams["load_id"] = loadId;
            if (priority != null) eventParams["priority"] = priority;
            if (errorCode != null) eventParams["error_code"] = errorCode;
            if (responseBytes.HasValue) eventParams["response_bytes"] = responseBytes.Value;
            if (retryCount.HasValue) eventParams["retry_count"] = retryCount.Value;

            DataBucketWrapper.Record("loading_finish", eventParams);
        }
    }
}
