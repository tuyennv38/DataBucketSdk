// @implements task:create-tracking-classes-0006
// @implements feature:other-metrics-0017

using System.Collections.Generic;
using UnityEngine;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketMetrics — Other metrics tracking events.
    /// Events: tutorial_action, button_click, screen_show, screen_exit.
    /// </summary>
    /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#other-metrics</remarks>
    public static class DataBucketMetrics
    {
        private const string TAG = "[DataBucketMetrics]";

        // ============================================================
        // TUTORIAL ACTION
        // ============================================================

        /// <summary>
        /// [tutorial_action] User thực hiện action theo tutorial.
        /// Trigger: Khi user thực hiện action bất kỳ theo tutorial.
        /// action_name="start" khi bắt đầu, "finish" khi hoàn thành.
        /// </summary>
        /// <param name="actionName">Tên action. "start" | "finish" | "click_1" | "use_booster_A"...</param>
        /// <param name="actionIndex">Thứ tự action trong chuỗi tutorial (>= 0). 0 = start.</param>
        /// <param name="actionCate">Nhóm action. VD: "click", "use_booster". Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#tutorial_action</remarks>
        public static void TutorialAction(
            string actionName,
            int actionIndex,
            string actionCate = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "action_name", actionName },
                { "action_index", actionIndex }
            };

            if (actionCate != null) eventParams["action_cate"] = actionCate;

            DataBucketWrapper.Record("tutorial_action", eventParams);
        }

        // ============================================================
        // BUTTON CLICK
        // ============================================================

        /// <summary>
        /// [button_click] User bấm vào một button quan trọng.
        /// Trigger: Khi user bấm button cần theo dõi (không trigger với button đã có event riêng).
        /// KPI: Phân tích hành vi/lựa chọn của user.
        /// </summary>
        /// <param name="buttonName">Tên button. VD: "Accept", "Quit", "Back"</param>
        /// <param name="screenName">Màn mà user bấm button. VD: "setting", "lose_confirm"</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#button_click</remarks>
        public static void ButtonClick(string buttonName, string screenName)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "button_name", buttonName },
                { "screen_name", screenName }
            };

            DataBucketWrapper.Record("button_click", eventParams);
        }

        // ============================================================
        // SCREEN SHOW
        // ============================================================

        /// <summary>
        /// [screen_show] User đến/mở ra/nhìn thấy một màn nào đó.
        /// Trigger: Khi user đến một màn quan trọng cần theo dõi.
        /// Không trigger với màn đã có event riêng (VD: iap_show, ad_impression).
        /// KPI: Phân tích thời gian user ở mỗi màn.
        /// </summary>
        /// <param name="screenName">Tên màn. VD: "setting", "lose_confirm"</param>
        /// <param name="buttonName">Button user bấm để đến màn này. Nullable.</param>
        /// <param name="prevScreenName">Màn trước đó. Nullable.</param>
        /// <param name="durationPrevScreen">Thời gian ở màn trước, msec. Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#screen_show</remarks>
        public static void ScreenShow(
            string screenName,
            string buttonName = null,
            string prevScreenName = null,
            long? durationPrevScreen = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "screen_name", screenName }
            };

            if (buttonName != null) eventParams["button_name"] = buttonName;
            if (prevScreenName != null) eventParams["prev_screen_name"] = prevScreenName;
            if (durationPrevScreen.HasValue) eventParams["duration_prev_screen"] = durationPrevScreen.Value;

            DataBucketWrapper.Record("screen_show", eventParams);
        }

        // ============================================================
        // SCREEN EXIT
        // ============================================================

        /// <summary>
        /// [screen_exit] User thoát game.
        /// Trigger: Khi user thoát game (kill app, đưa app về background, remove app).
        /// </summary>
        /// <param name="prevScreenName">Màn hình trước khi thoát game</param>
        /// <param name="durationPrevScreen">Thời gian ở màn trước khi thoát, msec. Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#screen_exit</remarks>
        public static void ScreenExit(
            string prevScreenName,
            long? durationPrevScreen = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "prev_screen_name", prevScreenName }
            };

            if (durationPrevScreen.HasValue) eventParams["duration_prev_screen"] = durationPrevScreen.Value;

            DataBucketWrapper.Record("screen_exit", eventParams);
        }
    }
}
