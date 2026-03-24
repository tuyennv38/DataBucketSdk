// @implements task:create-tracking-classes-0006
// @implements feature:resource-analytics-0012

using System.Collections.Generic;
using UnityEngine;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketResource — Resource earn/spend tracking events.
    /// Events: resource_earn, resource_spend.
    /// </summary>
    /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#resource-analytics</remarks>
    public static class DataBucketResource
    {
        private const string TAG = "[DataBucketResource]";

        // ============================================================
        // RESOURCE EARN
        // ============================================================

        /// <summary>
        /// [resource_earn] User nhận được tài nguyên.
        /// Trigger: Khi user nhận được tài nguyên.
        /// </summary>
        /// <param name="resourceType">Loại tài nguyên: "currency" | "item" | "booster". Nhiều loại: "currency,booster"</param>
        /// <param name="resourceName">Tên tài nguyên. VD: "gold", "hammer". Nhiều loại: "gold,hammer"</param>
        /// <param name="resourceAmount">Số lượng. Number nếu 1 loại, String nếu nhiều loại: "50,2"</param>
        /// <param name="placement">Vị trí/hoàn cảnh nhận. VD: "level_win", "daily_quest_reward"</param>
        /// <param name="reason">Lý do: "reward" | "exchange" | "purchase" | "watch_ads"</param>
        /// <param name="placementDetail">Chi tiết vị trí. "null" nếu không có. VD: "daily_reward_1". Nullable.</param>
        /// <param name="resourceBalance">Số tài nguyên sau khi nhận. Number hoặc String "5,15". Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#resource_earn</remarks>
        public static void Earn(
            string resourceType,
            string resourceName,
            object resourceAmount,
            string placement,
            string reason,
            string placementDetail = null,
            object resourceBalance = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "resource_type", resourceType },
                { "resource_name", resourceName },
                { "resource_amount", resourceAmount },
                { "placement", placement },
                { "reason", reason }
            };

            if (placementDetail != null) eventParams["placement_detail"] = placementDetail;
            if (resourceBalance != null) eventParams["resource_balance"] = resourceBalance;

            DataBucketWrapper.Record("resource_earn", eventParams);
        }

        // ============================================================
        // RESOURCE SPEND
        // ============================================================

        /// <summary>
        /// [resource_spend] User sử dụng tài nguyên.
        /// Trigger: Khi user sử dụng tài nguyên.
        /// </summary>
        /// <param name="resourceType">Loại tài nguyên: "currency" | "item" | "booster". Nhiều loại: "currency,booster"</param>
        /// <param name="resourceName">Tên tài nguyên. VD: "gold", "hammer". Nhiều loại: "gold,hammer"</param>
        /// <param name="resourceAmount">Số lượng. Number nếu 1 loại, String nếu nhiều loại: "50,2"</param>
        /// <param name="reason">Lý do: "exchange" | "use"</param>
        /// <param name="placement">Vị trí/hoàn cảnh sử dụng. VD: "buy_hammer", "use_booster"</param>
        /// <param name="placementDetail">Chi tiết vị trí. "null" nếu không có. Nullable.</param>
        /// <param name="resourceBalance">Số tài nguyên sau khi sử dụng. Nullable.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#resource_spend</remarks>
        public static void Spend(
            string resourceType,
            string resourceName,
            object resourceAmount,
            string reason,
            string placement,
            string placementDetail = null,
            object resourceBalance = null)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "resource_type", resourceType },
                { "resource_name", resourceName },
                { "resource_amount", resourceAmount },
                { "reason", reason },
                { "placement", placement }
            };

            if (placementDetail != null) eventParams["placement_detail"] = placementDetail;
            if (resourceBalance != null) eventParams["resource_balance"] = resourceBalance;

            DataBucketWrapper.Record("resource_spend", eventParams);
        }
    }
}
