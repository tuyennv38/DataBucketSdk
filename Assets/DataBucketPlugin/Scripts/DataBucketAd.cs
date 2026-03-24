// @implements task:create-tracking-classes-0006
// @implements feature:iaa-tracking-0014

using System.Collections.Generic;
using UnityEngine;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketAd — In-App Advertising (IAA) tracking events.
    /// Events: ad_request, ad_impression, ad_click, ad_complete.
    /// KPI: Đánh giá chất lượng kịch bản và hiệu quả quảng cáo.
    /// </summary>
    /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#iaa</remarks>
    public static class DataBucketAd
    {
        private const string TAG = "[DataBucketAd]";

        // ============================================================
        // AD REQUEST
        // ============================================================

        /// <summary>
        /// [ad_request] Request/load quảng cáo.
        /// Trigger: Khi bắt đầu request quảng cáo, bắn lên khi kết thúc request.
        /// KPI: Đánh giá chất lượng kịch bản load/show quảng cáo.
        /// </summary>
        /// <param name="adFormat">Format QC: "native" | "banner" | "interstitial" | "video_rewarded"</param>
        /// <param name="adPlatform">Mediation platform: "Admob" | "Max" | "IronSource"</param>
        /// <param name="adNetwork">Network cung cấp QC: "Admob" | "Applovin" | "Mintegral" | "Unity"</param>
        /// <param name="placement">Vị trí load QC. VD: "game_open", "level_end", "buy_booster"</param>
        /// <param name="isLoad">Load thành công? 1 = Success, 0 = Fail</param>
        /// <param name="loadTime">Thời gian load, msec (>= 0)</param>
        /// <param name="adUnitId">Ad unit ID. Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#ad_request</remarks>
        public static void Request(
            string adFormat,
            string adPlatform,
            string adNetwork,
            string placement,
            int isLoad,
            long loadTime,
            string adUnitId = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "ad_format", adFormat },
                { "ad_platform", adPlatform },
                { "ad_network", adNetwork },
                { "placement", placement },
                { "is_load", isLoad },
                { "load_time", loadTime }
            };

            if (adUnitId != null) eventParams["ad_unit_id"] = adUnitId;

            DataBucketWrapper.Record("ad_request", eventParams);
        }

        // ============================================================
        // AD IMPRESSION
        // ============================================================

        /// <summary>
        /// [ad_impression] Hiển thị quảng cáo.
        /// Trigger: Khi quảng cáo được hiển thị thành công.
        /// KPI: Đánh giá hiệu quả quảng cáo.
        /// </summary>
        /// <param name="adFormat">Format QC</param>
        /// <param name="adPlatform">Mediation platform</param>
        /// <param name="adNetwork">Network cung cấp QC</param>
        /// <param name="placement">Vị trí show QC</param>
        /// <param name="value">Giá trị tiền của QC (> 0). Dữ liệu từ mediation/Firebase.</param>
        /// <param name="adUnitId">Ad unit ID. Nullable.</param>
        /// <param name="isShow">Show thành công? 1 = Success, 0 = Fail. Nullable (Level 3).</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#ad_impression</remarks>
        public static void Impression(
            string adFormat,
            string adPlatform,
            string adNetwork,
            string placement,
            double value,
            string adUnitId = null,
            int? isShow = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "ad_format", adFormat },
                { "ad_platform", adPlatform },
                { "ad_network", adNetwork },
                { "placement", placement },
                { "value", value }
            };

            if (adUnitId != null) eventParams["ad_unit_id"] = adUnitId;
            if (isShow.HasValue) eventParams["is_show"] = isShow.Value;

            DataBucketWrapper.Record("ad_impression", eventParams);
        }

        // ============================================================
        // AD CLICK
        // ============================================================

        /// <summary>
        /// [ad_click] User click vào quảng cáo.
        /// Trigger: Khi user click vào quảng cáo.
        /// </summary>
        /// <param name="adFormat">Format QC</param>
        /// <param name="adPlatform">Mediation platform</param>
        /// <param name="adNetwork">Network cung cấp QC</param>
        /// <param name="placement">Vị trí show QC</param>
        /// <param name="adUnitId">Ad unit ID. Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#ad_click</remarks>
        public static void Click(
            string adFormat,
            string adPlatform,
            string adNetwork,
            string placement,
            string adUnitId = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "ad_format", adFormat },
                { "ad_platform", adPlatform },
                { "ad_network", adNetwork },
                { "placement", placement }
            };

            if (adUnitId != null) eventParams["ad_unit_id"] = adUnitId;

            DataBucketWrapper.Record("ad_click", eventParams);
        }

        // ============================================================
        // AD COMPLETE
        // ============================================================

        /// <summary>
        /// [ad_complete] Quảng cáo kết thúc.
        /// Trigger: Khi user xem xong hoặc tắt quảng cáo full màn hình.
        /// Áp dụng cho: open ad, interstitial, rewarded, fullscreen native.
        /// </summary>
        /// <param name="adFormat">Format QC: "interstitial" | "video_rewarded"</param>
        /// <param name="adPlatform">Mediation platform</param>
        /// <param name="adNetwork">Network cung cấp QC</param>
        /// <param name="placement">Vị trí show QC</param>
        /// <param name="adUnitId">Ad unit ID. Nullable.</param>
        /// <param name="endType">Kết thúc: "quit" (tắt giữa chừng) | "done" (xem hết + nhận thưởng). Chỉ áp dụng rewarded. Nullable.</param>
        /// <param name="durationAd">Thời gian xem QC, msec (> 0). Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#ad_complete</remarks>
        public static void Complete(
            string adFormat,
            string adPlatform,
            string adNetwork,
            string placement,
            string adUnitId = null,
            string endType = null,
            long? durationAd = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "ad_format", adFormat },
                { "ad_platform", adPlatform },
                { "ad_network", adNetwork },
                { "placement", placement }
            };

            if (adUnitId != null) eventParams["ad_unit_id"] = adUnitId;
            if (endType != null) eventParams["end_type"] = endType;
            if (durationAd.HasValue) eventParams["duration_ad"] = durationAd.Value;

            DataBucketWrapper.Record("ad_complete", eventParams);
        }
    }
}
