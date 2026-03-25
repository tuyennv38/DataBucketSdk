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
        // 7. SPECIFIC TRACKING MODULES TESTS
        // ============================================================
        Debug.Log("[Sample] 7. Testing Specific Tracking Modules...");
        TestUserProperties();
        TestLevelAnalytics();
        TestResourceAnalytics();
        TestIAP();
        TestIAA();
        TestNotifications();
        TestLiveOps();
        TestOtherMetrics();
        TestTechnicalPerformance();

        // ============================================================
        // 8. FORCE END SESSION — Kết thúc session thủ công
        // ============================================================
        Debug.Log("[Sample] 8. ForceEndSession...");

        // Ghi event trước khi kết thúc session (best practice)
        DataBucketWrapper.Record("user_logout", null);
        DataBucketWrapper.ForceEndSession();

        // ============================================================
        // 9. DISABLE EXCEPTION TRACKING — Tắt theo dõi exceptions
        // ============================================================
        Debug.Log("[Sample] 9. Disable Exception Tracking...");
        DataBucketWrapper.DisableExceptionTracking();

        Debug.Log("========== DataBucketWrapper Sample - END ==========");
    }

    private void TestUserProperties()
    {
        Debug.Log("[Sample] --- Test User Properties ---");
        DataBucketUserProperties.SetCurrentLevel(15);
        DataBucketUserProperties.SetUaAttribution(network: "Facebook", campaign: "ROAS_US");
        DataBucketUserProperties.SetResourceBalance("coin", 5000);
        DataBucketUserProperties.SetIsIapUser(true);
        DataBucketUserProperties.SetActiveDay(3);
    }

    private void TestLevelAnalytics()
    {
        Debug.Log("[Sample] --- Test Level Analytics ---");
        DataBucketLevel.LevelStart(
            level: 5,
            durationTotalStart: 60000,
            playType: "home",
            playIndex: 1,
            loseIndex: 0
        );

        DataBucketLevel.LevelEnd(
            level: 5,
            result: "win",
            durationPlay: 45000,
            loseBy: null,
            playType: "home",
            playIndex: 1,
            loseIndex: 0,
            durationTotalStart: 60000,
            durationTotalEnd: 60000,
            durationRemain: 15000,
            itemsTotal: 30,
            itemsCleared: 30
        );

        DataBucketLevel.LevelExit(
            level: 5,
            loopBy: 0,
            playType: "home",
            playIndex: 1,
            loseIndex: 0,
            exitIndex: 1,
            durationTotalStart: 60000,
            durationTotalEnd: 60000,
            durationRemain: 50000,
            durationPlay: 10000,
            itemsTotal: 30,
            itemsCleared: 5,
            actionSeq: "1,2,3",
            mode: "normal"
        );

        DataBucketLevel.LevelReopen(
            level: 5,
            loopBy: 0,
            playIndex: 1,
            loseIndex: 0,
            durationTotalStart: 60000,
            mode: "normal"
        );
    }

    private void TestResourceAnalytics()
    {
        Debug.Log("[Sample] --- Test Resource Analytics ---");
        DataBucketResource.Earn("currency", "gold", 50, "level_win", "reward", resourceBalance: 5050);
        DataBucketResource.Earn("currency,booster", "gold,hammer", "50,2", "daily_quest_reward", "reward");

        DataBucketResource.Spend("currency", "gold", 50, "use", "buy_hammer", resourceBalance: 5000);
    }

    private void TestIAP()
    {
        Debug.Log("[Sample] --- Test IAP ---");
        DataBucketIAP.Show("home_shop", "shop", "click", new[] { "null" });
        DataBucketIAP.Show("home_popup", "pack", "popup", new[] { "starterpack" });

        DataBucketIAP.Click("home_popup", "pack", "click", "starterpack");

        DataBucketIAP.PurchaseSuccess("home_shop", "pack", "click", "starterpack", 4.99, "USD");

        DataBucketIAP.PurchaseFailed("home_popup", "popup", "starterpack", 4.99, "USD", "user_cancelled");

        DataBucketIAP.Close("home_popup", "pack", "popup", "starterpack", 10000);
    }

    private void TestIAA()
    {
        Debug.Log("[Sample] --- Test IAA ---");
        DataBucketAd.Request("video_rewarded", "Admob", "Admob", "buy_booster", isLoad: 1, loadTime: 1500);

        DataBucketAd.Impression("video_rewarded", "Admob", "Admob", "buy_booster", value: 0.05, isShow: 1);

        DataBucketAd.Click("video_rewarded", "Admob", "Admob", "buy_booster");

        DataBucketAd.Complete("video_rewarded", "Admob", "Admob", "buy_booster", endType: "done", durationAd: 30000);
    }

    private void TestNotifications()
    {
        Debug.Log("[Sample] --- Test Notifications ---");
        DataBucketNotification.Send("remind", "Come back and play!");
        DataBucketNotification.Receive("remind", "Come back and play!");
        DataBucketNotification.Open("remind", "Come back and play!");
    }

    private void TestLiveOps()
    {
        Debug.Log("[Sample] --- Test LiveOps ---");
        DataBucketLiveOps.FeatureFirstShow("daily_reward", "home_icon");
        DataBucketLiveOps.FeatureOpen("daily_reward", "home_icon", "click", 1);
        DataBucketLiveOps.FeatureClose("daily_reward", "home_icon", 1, 15000);
    }

    private void TestOtherMetrics()
    {
        Debug.Log("[Sample] --- Test Other Metrics ---");
        DataBucketMetrics.TutorialAction("start", 0);
        DataBucketMetrics.TutorialAction("click_1", 1, actionCate: "click");
        DataBucketMetrics.TutorialAction("finish", 10);

        DataBucketMetrics.ButtonClick("Accept", "setting");

        DataBucketMetrics.ScreenShow("setting", "setting_btn", "home", 60000);

        DataBucketMetrics.ScreenExit("setting", 15000);
    }

    private void TestTechnicalPerformance()
    {
        Debug.Log("[Sample] --- Test Technical Performance ---");
        DataBucketTechnical.LoadingStart("api", "feed", "home", triggerSource: "screen_appear");
        DataBucketTechnical.LoadingFinish("api", "feed", "home", "success", 350L, triggerSource: "screen_appear");
    }
}
