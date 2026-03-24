// @implements task:create-tracking-classes-0006
// @implements feature:notification-tracking-0015

using System.Collections.Generic;
using UnityEngine;

namespace DataBucketPlugin
{
    /// <summary>
    /// DataBucketNotification — Notification tracking events.
    /// Events: noti_send, noti_receive, noti_open.
    /// KPI: Đánh giá hiệu quả của cơ chế gửi/nhận/mở notifications.
    /// </summary>
    /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#notifications</remarks>
    public static class DataBucketNotification
    {
        private const string TAG = "[DataBucketNotification]";

        /// <summary>
        /// [noti_send] Game gửi notification.
        /// Trigger: Khi game gửi noti.
        /// </summary>
        /// <param name="notiCate">Nhóm nội dung noti. VD: "remind", "event", "daily_quest"</param>
        /// <param name="notiName">Tên tóm gọn nội dung noti (để phân biệt). Có thể dùng headline.</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#noti_send</remarks>
        public static void Send(string notiCate, string notiName)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "noti_cate", notiCate },
                { "noti_name", notiName }
            };

            DataBucketWrapper.Record("noti_send", eventParams);
        }

        /// <summary>
        /// [noti_receive] User nhận được notification.
        /// Trigger: Khi user nhận được noti.
        /// </summary>
        /// <param name="notiCate">Nhóm nội dung noti</param>
        /// <param name="notiName">Tên tóm gọn nội dung noti</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#noti_receive</remarks>
        public static void Receive(string notiCate, string notiName)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "noti_cate", notiCate },
                { "noti_name", notiName }
            };

            DataBucketWrapper.Record("noti_receive", eventParams);
        }

        /// <summary>
        /// [noti_open] User mở notification.
        /// Trigger: Khi user mở noti.
        /// </summary>
        /// <param name="notiCate">Nhóm nội dung noti</param>
        /// <param name="notiName">Tên tóm gọn nội dung noti</param>
        /// <remarks>Chi tiết: xem Documents/DATA_TRACKING_GUIDE.md#noti_open</remarks>
        public static void Open(string notiCate, string notiName)
        {
            var eventParams = new Dictionary<string, object>
            {
                { "noti_cate", notiCate },
                { "noti_name", notiName }
            };

            DataBucketWrapper.Record("noti_open", eventParams);
        }
    }
}
