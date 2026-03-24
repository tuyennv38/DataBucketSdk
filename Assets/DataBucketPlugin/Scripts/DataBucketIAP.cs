// @implements task:create-tracking-classes-0006
// @implements feature:iap-tracking-0013

using System.Collections.Generic;
using UnityEngine;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketIAP — In-App Purchase tracking events.
    /// Events: iap_show, iap_click, iap_purchase_success, iap_purchase_failed, iap_close.
    /// KPI: Tỉ lệ purchase/show, click/show của từng gói IAP tại từng vị trí.
    /// </summary>
    /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#iap</remarks>
    public static class DataBucketIAP
    {
        private const string TAG = "[DataBucketIAP]";

        // ============================================================
        // IAP SHOW
        // ============================================================

        /// <summary>
        /// [iap_show] Màn/banner/popup IAP được hiển thị cho user.
        /// Trigger: Khi màn/banner/popup IAP show ra trên giao diện user.
        /// KPI: Tỉ lệ purchase/show, click/show.
        /// </summary>
        /// <param name="placement">Vị trí hiển thị. VD: "home_icon", "home_shop", "home_popup", "ingame_booster"</param>
        /// <param name="showType">Kiểu show: "shop" (cả shop) | "pack" (riêng gói)</param>
        /// <param name="triggerType">Cách show: "popup" (tự show) | "click" (user bấm)</param>
        /// <param name="packName">Tên gói IAP (Array). ["null"] nếu show cả shop. ["removeads","starterpack"] nếu show nhiều gói.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#iap_show</remarks>
        public static void Show(
            string placement,
            string showType,
            string triggerType,
            string[] packName)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "placement", placement },
                { "show_type", showType },
                { "trigger_type", triggerType },
                { "pack_name", packName }
            };

            DataBucketWrapper.Record("iap_show", eventParams);
        }

        // ============================================================
        // IAP CLICK
        // ============================================================

        /// <summary>
        /// [iap_click] User click mua gói IAP (chưa mua thành công).
        /// Trigger: Khi user click vào button mua 1 gói IAP.
        /// </summary>
        /// <param name="placement">Vị trí hiển thị</param>
        /// <param name="showType">Kiểu show: "shop" | "pack"</param>
        /// <param name="triggerType">Cách show: "popup" | "click"</param>
        /// <param name="packName">Tên gói IAP mà user click</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#iap_click</remarks>
        public static void Click(
            string placement,
            string showType,
            string triggerType,
            string packName)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "placement", placement },
                { "show_type", showType },
                { "trigger_type", triggerType },
                { "pack_name", packName }
            };

            DataBucketWrapper.Record("iap_click", eventParams);
        }

        // ============================================================
        // IAP PURCHASE SUCCESS
        // ============================================================

        /// <summary>
        /// [iap_purchase_success] User mua IAP thành công (client-side).
        /// Trigger: Khi user mua IAP thành công.
        /// </summary>
        /// <param name="placement">Vị trí hiển thị</param>
        /// <param name="showType">Kiểu show: "shop" | "pack"</param>
        /// <param name="triggerType">Cách show: "popup" | "click"</param>
        /// <param name="packName">Tên gói IAP mà user mua</param>
        /// <param name="price">Giá trị tiền của gói IAP (> 0). VD: 1.99</param>
        /// <param name="currency">Đơn vị tiền tệ. VD: "USD", "JPY", "KRW"</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#iap_purchase_success</remarks>
        public static void PurchaseSuccess(
            string placement,
            string showType,
            string triggerType,
            string packName,
            double price,
            string currency)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "placement", placement },
                { "show_type", showType },
                { "trigger_type", triggerType },
                { "pack_name", packName },
                { "price", price },
                { "currency", currency }
            };

            DataBucketWrapper.Record("iap_purchase_success", eventParams);
        }

        // ============================================================
        // IAP PURCHASE FAILED
        // ============================================================

        /// <summary>
        /// [iap_purchase_failed] User mua IAP thất bại.
        /// Trigger: Khi user mua IAP thất bại.
        /// </summary>
        /// <param name="placement">Vị trí hiển thị</param>
        /// <param name="triggerType">Cách show: "popup" | "click"</param>
        /// <param name="packName">Tên gói IAP</param>
        /// <param name="price">Giá trị tiền của gói IAP</param>
        /// <param name="currency">Đơn vị tiền tệ</param>
        /// <param name="failReason">Lý do thất bại. VD: "user_cancelled", "payment_declined", "network_error"</param>
        /// <param name="errorCode">Mã lỗi từ store. "null" nếu không có. Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#iap_purchase_failed</remarks>
        public static void PurchaseFailed(
            string placement,
            string triggerType,
            string packName,
            double price,
            string currency,
            string failReason,
            string errorCode = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "placement", placement },
                { "trigger_type", triggerType },
                { "pack_name", packName },
                { "price", price },
                { "currency", currency },
                { "fail_reason", failReason }
            };

            if (errorCode != null) eventParams["error_code"] = errorCode;

            DataBucketWrapper.Record("iap_purchase_failed", eventParams);
        }

        // ============================================================
        // IAP CLOSE
        // ============================================================

        /// <summary>
        /// [iap_close] Màn/banner/popup IAP được đóng lại.
        /// Trigger: Khi user đóng màn/banner/popup IAP.
        /// KPI: Tính thời gian user ở màn IAP cho bài toán IAP prediction.
        /// </summary>
        /// <param name="placement">Vị trí hiển thị</param>
        /// <param name="showType">Kiểu show: "shop" | "pack"</param>
        /// <param name="triggerType">Cách show: "popup" | "click"</param>
        /// <param name="packName">Tên gói IAP. "null" nếu show cả shop.</param>
        /// <param name="durationIap">Thời gian user ở màn IAP, msec (> 0)</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#iap_close</remarks>
        public static void Close(
            string placement,
            string showType,
            string triggerType,
            string packName,
            long durationIap)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "placement", placement },
                { "show_type", showType },
                { "trigger_type", triggerType },
                { "pack_name", packName },
                { "duration_iap", durationIap }
            };

            DataBucketWrapper.Record("iap_close", eventParams);
        }
    }
}
