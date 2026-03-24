// @implements task:create-sample-0005
// @implements feature:sample-script-0009

using UnityEngine;
using System.Collections.Generic;
using DataBucketPlugin;

/// <summary>
/// DataBucketWrapperSample — Script mẫu test tất cả API của DataBucketWrapper.
/// Gắn script này vào bất kỳ GameObject nào trong Scene để chạy test.
/// </summary>
public class DataBucketWrapperSample : MonoBehaviour
{
    [Header("Databuckets Config")]
    [SerializeField] private string apiEndpoint = "api-endpoint-here";
    [SerializeField] private string apiKey = "your-api-key-here";

    void Start()
    {
        Debug.Log("========== DataBucketWrapper Sample - START ==========");

        // ============================================================
        // 1. INIT — Khởi tạo SDK (BẮT BUỘC gọi đầu tiên)
        // ============================================================
        Debug.Log("[Sample] 1. Init SDK...");
        DataBucketWrapper.Init(apiEndpoint, apiKey);

        // Test: Gọi Init lần 2 → phải log warning
        Debug.Log("[Sample] 1b. Init lan 2 (kiem tra warning)...");
        DataBucketWrapper.Init(apiEndpoint, apiKey);

        // Kiểm tra IsInitialized
        Debug.Log($"[Sample] IsInitialized = {DataBucketWrapper.IsInitialized}");

        // ============================================================
        // 2. ENABLE EXCEPTION TRACKING — Bật theo dõi exceptions
        // ============================================================
        Debug.Log("[Sample] 2. Enable Exception Tracking...");
        DataBucketWrapper.EnableExceptionTracking();

        // ============================================================
        // 3. SET COMMON PROPERTY — Set 1 thuộc tính chung
        // ============================================================
        Debug.Log("[Sample] 3. SetCommonProperty...");
        DataBucketWrapper.SetCommonProperty("user_level", 1);
        DataBucketWrapper.SetCommonProperty("game_mode", "story");

        // ============================================================
        // 4. SET COMMON PROPERTIES — Set nhiều thuộc tính cùng lúc
        // ============================================================
        Debug.Log("[Sample] 4. SetCommonProperties (batch)...");
        DataBucketWrapper.SetCommonProperties(new Dictionary<string, object>
        {
            ["user_level"] = 5,
            ["game_mode"] = "pvp",
            ["user_type"] = "premium",
            ["country"] = "VN"
        });

        // ============================================================
        // 5. RECORD — Ghi nhận event đơn giản
        // ============================================================
        Debug.Log("[Sample] 5. Record event...");

        // Event không có params
        DataBucketWrapper.Record("app_started", null);

        // Event có params
        var levelParams = new Dictionary<string, object>
        {
            ["level"] = 1,
            ["difficulty"] = "normal",
            ["lives_remaining"] = 3
        };
        DataBucketWrapper.Record("level_started", levelParams);

        // ============================================================
        // 6. RECORD WITH TIMING — Đo thời gian giữa 2 events
        // ============================================================
        Debug.Log("[Sample] 6. RecordWithTiming...");

        // Bước 6a: Ghi event bắt đầu (đã ghi "level_started" ở trên)
        // Bước 6b: Ghi event kết thúc kèm timing
        var completionParams = new Dictionary<string, object>
        {
            ["level"] = 1,
            ["score"] = 5000,
            ["success"] = true
        };
        DataBucketWrapper.RecordWithTiming(
            "level_completed",      // event kết thúc
            completionParams,       // params
            "level_duration",       // property chứa timing
            "level_started"         // event bắt đầu
        );

        // ============================================================
        // 7. FORCE END SESSION — Kết thúc session thủ công
        // ============================================================
        Debug.Log("[Sample] 7. ForceEndSession...");

        // Ghi event trước khi kết thúc session (best practice)
        DataBucketWrapper.Record("user_logout", null);
        DataBucketWrapper.ForceEndSession();

        // ============================================================
        // 8. DISABLE EXCEPTION TRACKING — Tắt theo dõi exceptions
        // ============================================================
        Debug.Log("[Sample] 8. Disable Exception Tracking...");
        DataBucketWrapper.DisableExceptionTracking();

        Debug.Log("========== DataBucketWrapper Sample - END ==========");
    }
}
