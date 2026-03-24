// @implements task:create-wrapper-0002
// @implements feature:wrapper-init-0001
// @implements feature:wrapper-record-0002
// @implements feature:wrapper-record-timing-0003
// @implements feature:wrapper-set-property-0004
// @implements feature:wrapper-set-properties-0005
// @implements feature:wrapper-end-session-0006
// @implements feature:wrapper-exception-tracking-0007

using System.Collections.Generic;
using UnityEngine;
using Databuckets;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketWrapper — Static wrapper class cho Databuckets Unity SDK v1.0.6.
    /// Cung cấp kiểm tra Init state tự động và log error khi gọi sai thứ tự.
    /// Version: 1.0.0
    /// </summary>
    public static class DataBucketWrapper
    {
        private static bool _isInitialized = false;

        /// <summary>
        /// Kiểm tra SDK đã được Init hay chưa.
        /// </summary>
        public static bool IsInitialized => _isInitialized;

        // ============================================================
        // INIT
        // ============================================================

        /// <summary>
        /// Khởi tạo Databuckets SDK. BẮT BUỘC gọi 1 lần duy nhất trước mọi API khác.
        /// Gọi trong Start() hoặc sau yield return null. KHÔNG gọi trong Awake().
        /// </summary>
        /// <param name="apiEndpoint">URL endpoint của Databuckets API</param>
        /// <param name="apiKey">API key xác thực</param>
        public static void Init(string apiEndpoint, string apiKey)
        {
            if (_isInitialized)
            {
                Debug.LogWarning("[DataBucketWrapper] Init: SDK da duoc khoi tao roi. Khong goi lai.");
                return;
            }

            DatabucketsTracker.Init(apiEndpoint, apiKey);
            _isInitialized = true;
            Debug.Log("[DataBucketWrapper] Init: SDK khoi tao thanh cong.");
        }

        // ============================================================
        // RECORD
        // ============================================================

        /// <summary>
        /// Ghi nhận một business event.
        /// Events tự động gửi mỗi 10 giây và backup khi mất mạng.
        /// </summary>
        /// <param name="eventName">Tên event (snake_case). KHÔNG trùng auto-tracking events.</param>
        /// <param name="eventParams">Thuộc tính kèm theo (Dictionary) hoặc null.</param>
        public static void Record(string eventName, Dictionary<string, object> eventParams)
        {
            if (!CheckInitialized("Record")) return;

            DatabucketsTracker.Record(eventName, eventParams);
        }

        // ============================================================
        // RECORD WITH TIMING
        // ============================================================

        /// <summary>
        /// Ghi nhận event kèm đo thời gian từ startEvent đến event hiện tại.
        /// PHẢI gọi Record() cho startEvent trước khi gọi method này.
        /// </summary>
        /// <param name="eventName">Tên event kết thúc</param>
        /// <param name="eventParams">Thuộc tính kèm theo</param>
        /// <param name="timingProperty">Tên property chứa giá trị timing</param>
        /// <param name="startEvent">Tên event bắt đầu (đã Record trước đó)</param>
        public static void RecordWithTiming(string eventName, Dictionary<string, object> eventParams, string timingProperty, string startEvent)
        {
            if (!CheckInitialized("RecordWithTiming")) return;

            DatabucketsTracker.RecordWithTiming(eventName, eventParams, timingProperty, startEvent);
        }

        // ============================================================
        // SET COMMON PROPERTY
        // ============================================================

        /// <summary>
        /// Đặt 1 common property sẽ gắn tự động vào mọi event sau đó.
        /// Dùng khi user level up, đổi game mode, mua premium...
        /// </summary>
        /// <param name="key">Tên property. KHÔNG trùng auto-injected fields.</param>
        /// <param name="value">Giá trị property (primitive types).</param>
        public static void SetCommonProperty(string key, object value)
        {
            if (!CheckInitialized("SetCommonProperty")) return;

            DatabucketsTracker.SetCommonProperty(key, value);
        }

        // ============================================================
        // SET COMMON PROPERTIES (BATCH)
        // ============================================================

        /// <summary>
        /// Đặt nhiều common properties cùng lúc.
        /// Hiệu quả hơn gọi SetCommonProperty() nhiều lần.
        /// </summary>
        /// <param name="properties">Dictionary chứa các cặp key-value.</param>
        public static void SetCommonProperties(Dictionary<string, object> properties)
        {
            if (!CheckInitialized("SetCommonProperties")) return;

            DatabucketsTracker.SetCommonProperties(properties);
        }

        // ============================================================
        // FORCE END SESSION
        // ============================================================

        /// <summary>
        /// Kết thúc session hiện tại thủ công.
        /// Chỉ dùng khi: user logout, chuyển đổi tài khoản.
        /// Session mới tự động tạo khi có event tiếp theo.
        /// </summary>
        public static void ForceEndSession()
        {
            if (!CheckInitialized("ForceEndSession")) return;

            DatabucketsTracker.ForceEndCurrentSession();
        }

        // ============================================================
        // EXCEPTION TRACKING
        // ============================================================

        /// <summary>
        /// Bật tự động theo dõi Unity exceptions.
        /// Khi bật, mỗi LogType.Exception sẽ gửi event "app_exception_log".
        /// Hữu ích cho production builds.
        /// </summary>
        public static void EnableExceptionTracking()
        {
            if (!CheckInitialized("EnableExceptionTracking")) return;

            DatabucketsTracker.EnableExceptionLogTracking();
        }

        /// <summary>
        /// Tắt theo dõi Unity exceptions.
        /// </summary>
        public static void DisableExceptionTracking()
        {
            if (!CheckInitialized("DisableExceptionTracking")) return;

            DatabucketsTracker.DisableExceptionLogTracking();
        }

        // ============================================================
        // PRIVATE HELPER
        // ============================================================

        /// <summary>
        /// Kiểm tra SDK đã Init chưa. Log error nếu chưa.
        /// </summary>
        /// <param name="methodName">Tên method đang gọi (để log rõ ràng).</param>
        /// <returns>true nếu đã Init, false nếu chưa.</returns>
        private static bool CheckInitialized(string methodName)
        {
            if (!_isInitialized)
            {
                Debug.LogError($"[DataBucketWrapper] {methodName}: SDK chua duoc khoi tao. Hay goi DataBucketWrapper.Init() truoc.");
                return false;
            }
            return true;
        }
    }
}
