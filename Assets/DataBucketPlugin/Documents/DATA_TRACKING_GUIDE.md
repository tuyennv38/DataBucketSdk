# DATA TRACKING GUIDE — DataBucketPlugin v1.0.1

> **Nguồn dữ liệu:** Sheet "Data Tracking Plan | Level-base" từ file `Databuckets _ Data Tracking Plan Template.xlsx`
>
> **⚠️ Lưu ý quan trọng:** Cần log đúng Data Type (String hoặc Number) để không ảnh hưởng đến kết quả truy vấn.

---

## Mục lục

- [User Properties — `DataBucketUserProperties`](#user-properties)
- [Level Analytics — `DataBucketLevel`](#level-analytics)
  - [level_start](#level_start)
  - [level_end](#level_end)
  - [level_exit](#level_exit)
  - [level_reopen](#level_reopen)
- [Resource Analytics — `DataBucketResource`](#resource-analytics)
  - [resource_earn](#resource_earn)
  - [resource_spend](#resource_spend)
- [IAP — `DataBucketIAP`](#iap)
  - [iap_show](#iap_show)
  - [iap_click](#iap_click)
  - [iap_purchase_success](#iap_purchase_success)
  - [iap_purchase_failed](#iap_purchase_failed)
  - [iap_close](#iap_close)
- [IAA — `DataBucketAd`](#iaa)
  - [ad_request](#ad_request)
  - [ad_impression](#ad_impression)
  - [ad_click](#ad_click)
  - [ad_complete](#ad_complete)
- [Notifications — `DataBucketNotification`](#notifications)
  - [noti_send](#noti_send)
  - [noti_receive](#noti_receive)
  - [noti_open](#noti_open)
- [Live Ops — `DataBucketLiveOps`](#live-ops)
  - [feature_first_show](#feature_first_show)
  - [feature_open](#feature_open)
  - [feature_close](#feature_close)
- [Other Metrics — `DataBucketMetrics`](#other-metrics)
  - [tutorial_action](#tutorial_action)
  - [button_click](#button_click)
  - [screen_show](#screen_show)
  - [screen_exit](#screen_exit)
- [Technical Performance — `DataBucketTechnical`](#technical-performance)
  - [loading_start](#loading_start)
  - [loading_finish](#loading_finish)

---

<a id="user-properties"></a>

## User Properties — `DataBucketUserProperties`

> **Khái niệm:** User Properties là thuộc tính mô tả đặc điểm/trạng thái của user, không gắn với event cụ thể mà tồn tại ở cấp độ người dùng. Hệ thống tự động đính kèm các user property hiện tại vào mọi event khi được ghi nhận.

> **Lưu ý Firebase:** Khi bắn data qua Firebase, property sẽ bị ép data type từ number sang string. Do đó, property Number cần hậu tố `_n` để Databuckets auto convert (trừ property có chữ "level" trong tên).

**Trigger chung:** Gán/cập nhật ngay khi user bắt đầu sử dụng game (D0). Cập nhật khi trạng thái thay đổi. Chỉ lưu giá trị hiện tại tại thời điểm event xảy ra.

| Param | Level | Type | Values | Value Requirement | Definition |
|-------|-------|------|--------|-------------------|------------|
| `current_level` | 2 | Number | 1,2,3... | >=0 | Level hiện tại. Cập nhật vào thời điểm user bắt đầu chơi level **sau**, không phải lúc hoàn thành level trước. |
| `ua_network` | 3 | String | — | — | User tải game từ network quảng cáo nào (callback data từ MMP). |
| `ua_campaign` | 3 | String | — | — | User tải game từ campaign nào. |
| `ua_adgroup` | 3 | String | — | — | User tải game từ adgroup/adset nào. |
| `ua_creative` | 3 | String | — | — | User tải game từ creative nào. |
| `ua_tracker_name` | 3 | String | — | — | Tổng hợp nguồn user (chỉ có nếu dùng Adjust). |
| `firebase_exp` | 3 | Array | `["firebase_exp_1:0","firebase_exp_2:1"]` | null hoặc array | User thuộc A/B test Firebase nào. Chỉ cần log khi bắn trực tiếp lên Databuckets. |
| `balance_{resource}_n` | 3 | Number | 0,1,2... | >=0 | Số lượng resource tại thời điểm event. VD: `balance_coin_n`, `balance_ticket_n`. |
| `user_id` | 4 | String | UUID | — | Mã định danh do client tạo, dùng mapping data với third-party/backend. |
| `current_mode` | 4 | String | normal,challenge,endless | Giá trị enum | Mode chơi hiện tại. |
| `current_event` | 4 | String | race,double_reward | Giá trị enum | Event (live ops) mà user đang tham gia. |
| `is_iap_user_n` | 4 | Number | 0,1 | 0 hoặc 1 | True (=1), False (=0). Cập nhật từ 0 → 1 sau lần mua IAP đầu tiên. |
| `iap_count_n` | 4 | Number | 0,1,2... | >=0 | Số lần mua IAP. |
| `active_day_n` | 4 | Number | 0,1,2... | >=0 | Số ngày active (chỉ tính ngày user thực sự mở game). |
| `connection_type` | 4 | String | offline,wifi,mobile_data,unknown | — | Loại kết nối internet. |
| `win_streak_n` | 4 | Number | 0,1,2... | >=0 | Chuỗi thắng liên tiếp. |
| `lose_streak_n` | 4 | Number | 0,1,2... | >=0 | Chuỗi thua liên tiếp. |

```csharp
// Ví dụ sử dụng
DataBucketUserProperties.SetCurrentLevel(15);
DataBucketUserProperties.SetUaAttribution(network: "Facebook", campaign: "ROAS_US");
DataBucketUserProperties.SetResourceBalance("coin", 5000);
DataBucketUserProperties.SetIsIapUser(true);
DataBucketUserProperties.SetActiveDay(3);
```

---

<a id="level-analytics"></a>

## Level Analytics — `DataBucketLevel`

<a id="level_start"></a>

### level_start

| Field | Value |
|-------|-------|
| **Event Definition** | User bắt đầu chơi 1 level |
| **Trigger** | Trigger khi user bắt đầu chơi 1 level |
| **KPI** | — |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `level` | 2 | Number | 1,2,3... | >=1 | Level mà user chơi |
| `duration_total_start` | 2 | Number | msec | >0 | Thời gian cho phép chơi ban đầu |
| `loop_by` | 3 | Number | 0,1,2... | >=0 | Level lấy content từ level nào (0 = không lặp) |
| `play_type` | 3 | String | home,next,restart | Giá trị enum | Hoàn cảnh chơi: home=bấm play từ home, next=hoàn thành level trước, restart=chơi lại |
| `play_index` | 3 | Number | 1,2,3... | >=1 | Lần thứ mấy user chơi level này |
| `lose_index` | 3 | Number | 0,1,2... | >=0 | Số lần thua trước đó (lần đầu = 0) |
| `mode` | 4 | String | normal,challenge,endless | Giá trị enum | Mode chơi |

```csharp
DataBucketLevel.LevelStart(
    level: 5,
    durationTotalStart: 60000,
    playType: "home",
    playIndex: 1,
    loseIndex: 0
);
```

---

<a id="level_end"></a>

### level_end

| Field | Value |
|-------|-------|
| **Event Definition** | User kết thúc quá trình chơi 1 level |
| **Trigger** | Trigger khi user kết thúc màn chơi |
| **KPI** | — |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `level` | 2 | Number | 1,2,3... | >=1 | Level mà user chơi |
| `result` | 2 | String | win,lose,quit,exit | Giá trị enum | Kết quả: win=thắng, lose=thua, quit=thoát level (không thoát game), exit=thoát game giữa chừng |
| `duration_play` | 2 | Number | msec | >=0 | Thời gian chơi (không tính xem ads) |
| `lose_by` | 2 | String | null,out_of_move,out_of_time | — | Lý do thua. null nếu win/quit/exit |
| `loop_by` | 3 | Number | 0,1,2... | >=0 | Level lấy content từ level nào |
| `play_type` | 3 | String | home,next,restart | Giá trị enum | Hoàn cảnh chơi |
| `play_index` | 3 | Number | 1,2,3... | >=1 | Lần thứ mấy user chơi |
| `lose_index` | 3 | Number | 0,1,2... | >=0 | Số lần thua trước đó |
| `duration_total_start` | 2 | Number | msec | >0 | Thời gian cho phép chơi ban đầu |
| `duration_total_end` | 2 | Number | msec | >0 | Tổng thời gian cho phép (có thể > start nếu dùng booster time) |
| `duration_remain` | 2 | Number | msec | >=0 | Thời gian còn lại khi end = duration_total_end - duration_play |
| `items_total` | 3 | Number | 1,2,3... | >=1 | Tổng item cần xử lý trong level |
| `items_cleared` | 3 | Number | 0,1,2... | >=0 | Số item đã xử lý |
| `action_seq` | 4 | String | "3,6,1,2,4" | — | Chuỗi action alias (không dấu cách). "0" nếu không action. |
| `mode` | 4 | String | normal,challenge,endless | Giá trị enum | Mode chơi |

```csharp
DataBucketLevel.LevelEnd(
    level: 5,
    result: "win",
    durationPlay: 45000,
    playType: "home",
    playIndex: 1,
    loseIndex: 0,
    durationTotalStart: 60000,
    durationTotalEnd: 60000,
    durationRemain: 15000,
    itemsTotal: 30,
    itemsCleared: 30
);
```

---

<a id="level_exit"></a>

### level_exit

| Field | Value |
|-------|-------|
| **Event Definition** | User thoát game khi đang chơi dở level |
| **Trigger** | Khi user rời khỏi game giữa màn chơi (kill app, background). Chỉ log nếu KHÔNG log level_end với result="exit" |
| **KPI** | — |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `level` | 2 | Number | 1,2,3... | >=1 | Level đang chơi |
| `loop_by` | 3 | Number | 0,1,2... | >=0 | Level lấy content |
| `play_type` | 3 | String | home,next,restart | Giá trị enum | Hoàn cảnh chơi |
| `play_index` | 3 | Number | 1,2,3... | >=1 | Lần thứ mấy |
| `lose_index` | 3 | Number | 0,1,2... | >=0 | Số lần thua trước |
| `exit_index` | 3 | Number | 1,2,3... | >=1 | Lần exit thứ mấy (reset mỗi level_start) |
| `duration_total_start` | 2 | Number | msec | >0 | Thời gian cho phép ban đầu |
| `duration_total_end` | 2 | Number | msec | >0 | Tổng thời gian cho phép |
| `duration_remain` | 2 | Number | msec | >=0 | Thời gian còn lại |
| `duration_play` | 2 | Number | msec | >=0 | Thời gian đã chơi |
| `items_total` | 3 | Number | 1,2,3... | >=1 | Tổng item |
| `items_cleared` | 3 | Number | 0,1,2... | >=0 | Số item đã xử lý |
| `action_seq` | 4 | String | "3,6,1,2,4" | — | Chuỗi action |
| `mode` | 4 | String | normal,challenge,endless | Giá trị enum | Mode chơi |

---

<a id="level_reopen"></a>

### level_reopen

| Field | Value |
|-------|-------|
| **Event Definition** | User quay lại game và chơi tiếp level đang dở |
| **Trigger** | Khi user quay lại game sau level_exit. Nếu phải chơi lại từ đầu → dùng LevelStart(). Chỉ log nếu KHÔNG log level_end với result="exit" |
| **KPI** | — |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `level` | 2 | Number | 1,2,3... | >=1 | Level đang chơi |
| `loop_by` | 3 | Number | 0,1,2... | >=0 | Level lấy content |
| `play_index` | 3 | Number | 1,2,3... | >=1 | Lần thứ mấy |
| `lose_index` | 3 | Number | 0,1,2... | >=0 | Số lần thua trước |
| `duration_total_start` | 2 | Number | msec | >0 | Thời gian cho phép ban đầu |
| `mode` | 4 | String | normal,challenge,endless | Giá trị enum | Mode chơi |

---

<a id="resource-analytics"></a>

## Resource Analytics — `DataBucketResource`

<a id="resource_earn"></a>

### resource_earn

| Field | Value |
|-------|-------|
| **Event Definition** | User nhận được tài nguyên |
| **Trigger** | Khi user nhận được tài nguyên |
| **KPI** | — |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `resource_type` | 2 | String | currency,item,booster | Giá trị enum | Nhóm tài nguyên. Nhiều loại: "currency,booster" |
| `resource_name` | 2 | String | gold,coin,hammer... | — | Tên tài nguyên. Nhiều loại: "gold,hammer" |
| `resource_amount` | 2 | Number/String | 50 hoặc "50,2" | — | Số lượng. Number nếu 1 loại, String nếu nhiều: "50,2" (không dấu cách sau phẩy) |
| `placement` | 2 | String | level_win,daily_quest_reward | — | Vị trí/hoàn cảnh nhận |
| `reason` | 2 | String | reward,exchange,purchase,watch_ads | Giá trị enum | Lý do nhận |
| `placement_detail` | 3 | String | daily_reward_1 | — | Chi tiết vị trí |
| `resource_balance` | 3 | Number/String | 100 hoặc "5,15" | — | Số tài nguyên sau khi nhận |

```csharp
DataBucketResource.Earn("currency", "gold", 50, "level_win", "reward", resourceBalance: 5050);
DataBucketResource.Earn("currency,booster", "gold,hammer", "50,2", "daily_quest_reward", "reward");
```

---

<a id="resource_spend"></a>

### resource_spend

| Field | Value |
|-------|-------|
| **Event Definition** | User sử dụng tài nguyên |
| **Trigger** | Khi user sử dụng tài nguyên |
| **KPI** | — |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `resource_type` | 2 | String | currency,item,booster | Giá trị enum | Nhóm tài nguyên |
| `resource_name` | 2 | String | gold,coin,hammer... | — | Tên tài nguyên |
| `resource_amount` | 2 | Number/String | 50 hoặc "50,2" | — | Số lượng |
| `reason` | 2 | String | exchange,use | Giá trị enum | Lý do sử dụng |
| `placement` | 2 | String | buy_hammer,use_booster | — | Vị trí/hoàn cảnh |
| `placement_detail` | 3 | String | — | — | Chi tiết vị trí |
| `resource_balance` | 3 | Number/String | — | — | Số tài nguyên sau khi sử dụng |

---

<a id="iap"></a>

## IAP — `DataBucketIAP`

> **KPI chung:** Tỉ lệ purchase/show, click/show của từng gói IAP tại từng vị trí.

<a id="iap_show"></a>

### iap_show

| Field | Value |
|-------|-------|
| **Event Definition** | Màn/banner/popup về IAP được show |
| **Trigger** | Khi màn/banner/popup IAP show ra trên giao diện user |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `placement` | 2 | String | home_icon,home_shop,home_popup,ingame_booster,ingame_icon,ingame_popup | — | Vị trí hiển thị |
| `show_type` | 2 | String | shop,pack | Giá trị enum | shop=show cả shop, pack=show riêng gói |
| `trigger_type` | 2 | String | popup,click | Giá trị enum | popup=tự show, click=user bấm |
| `pack_name` | 2 | Array | ["null"] hoặc ["removeads","starterpack"] | — | Tên gói. ["null"] nếu show cả shop. Nhiều gói cách bằng phẩy. |

```csharp
DataBucketIAP.Show("home_shop", "shop", "click", new[] { "null" });
DataBucketIAP.Show("home_popup", "pack", "popup", new[] { "starterpack" });
```

---

<a id="iap_click"></a>

### iap_click

| Field | Value |
|-------|-------|
| **Event Definition** | User click mua gói IAP (chưa mua thành công) |
| **Trigger** | Khi user click vào button mua 1 gói |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `placement` | 2 | String | — | — | Vị trí |
| `show_type` | 2 | String | shop,pack | Giá trị enum | Kiểu show |
| `trigger_type` | 2 | String | popup,click | Giá trị enum | Cách show |
| `pack_name` | 2 | String | removeads,starterpack | — | Tên gói được click |

---

<a id="iap_purchase_success"></a>

### iap_purchase_success

| Field | Value |
|-------|-------|
| **Event Definition** | User mua IAP thành công (client-side) |
| **Trigger** | Khi user mua IAP thành công |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `placement` | 2 | String | — | — | Vị trí |
| `show_type` | 2 | String | shop,pack | Giá trị enum | Kiểu show |
| `trigger_type` | 2 | String | popup,click | Giá trị enum | Cách show |
| `pack_name` | 2 | String | — | — | Tên gói mua |
| `price` | 2 | Number | 1.99,2.99,9.99... | >0 | Giá trị tiền |
| `currency` | 2 | String | USD,JPY,KRW | — | Đơn vị tiền tệ |

```csharp
DataBucketIAP.PurchaseSuccess("home_shop", "pack", "click", "starterpack", 4.99, "USD");
```

---

<a id="iap_purchase_failed"></a>

### iap_purchase_failed

| Field | Value |
|-------|-------|
| **Event Definition** | User mua IAP thất bại |
| **Trigger** | Khi mua IAP thất bại |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `placement` | 2 | String | — | — | Vị trí |
| `trigger_type` | 2 | String | popup,click | Giá trị enum | Cách show |
| `pack_name` | 2 | String | — | — | Tên gói mua |
| `price` | 2 | Number | — | >0 | Giá trị tiền |
| `currency` | 2 | String | — | — | Đơn vị tiền tệ |
| `fail_reason` | 2 | String | user_cancelled,payment_declined,network_error | — | Lý do thất bại |
| `error_code` | 3 | String | — | — | Mã lỗi từ store |

---

<a id="iap_close"></a>

### iap_close

| Field | Value |
|-------|-------|
| **Event Definition** | Màn/banner/popup IAP bị đóng |
| **Trigger** | Khi user đóng màn IAP |
| **KPI** | Thời gian user ở màn IAP cho bài toán IAP prediction |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `placement` | 2 | String | — | — | Vị trí |
| `show_type` | 2 | String | shop,pack | Giá trị enum | Kiểu show |
| `trigger_type` | 2 | String | popup,click | Giá trị enum | Cách show |
| `pack_name` | 2 | String | — | — | Tên gói. "null" nếu shop. |
| `duration_iap` | 2 | Number | msec | >0 | Thời gian ở màn IAP |

---

<a id="iaa"></a>

## IAA — `DataBucketAd`

> **KPI chung:** Đánh giá chất lượng kịch bản & kỹ thuật load/show quảng cáo.

<a id="ad_request"></a>

### ad_request

| Field | Value |
|-------|-------|
| **Event Definition** | Request/load quảng cáo |
| **Trigger** | Khi bắt đầu request QC, bắn lên khi kết thúc request |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `ad_format` | 2 | String | native,banner,interstitial,video_rewarded | — | Format QC |
| `ad_platform` | 2 | String | Admob,Max,IronSource | — | Mediation platform |
| `ad_network` | 2 | String | Admob,Applovin,Mintegral,Unity | — | Network cung cấp QC |
| `placement` | 2 | String | game_open,level_end,add_time,buy_booster,claim_extra_reward | — | Vị trí load QC |
| `is_load` | 2 | Number | 0,1 | 0 hoặc 1 | Success (=1), Fail (=0) |
| `load_time` | 2 | Number | msec | >=0 | Thời gian load |
| `ad_unit_id` | 3 | String | — | — | Ad unit ID |

```csharp
DataBucketAd.Request("video_rewarded", "Admob", "Admob", "buy_booster", isLoad: 1, loadTime: 1500);
```

---

<a id="ad_impression"></a>

### ad_impression

| Field | Value |
|-------|-------|
| **Event Definition** | Hiển thị quảng cáo |
| **Trigger** | Khi QC được show thành công hoặc kết thúc request show |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `ad_format` | 2 | String | — | — | Format QC |
| `ad_platform` | 2 | String | — | — | Mediation platform |
| `ad_network` | 2 | String | — | — | Network |
| `placement` | 2 | String | — | — | Vị trí show |
| `value` | 2 | Number | — | >0 | Giá trị tiền của QC (từ mediation/Firebase) |
| `ad_unit_id` | 3 | String | — | — | Ad unit ID |
| `is_show` | 3 | Number | 0,1 | 0 hoặc 1 | Success (=1), Fail (=0) |

---

<a id="ad_click"></a>

### ad_click

| Field | Value |
|-------|-------|
| **Event Definition** | User click vào quảng cáo |
| **Trigger** | Khi user click vào QC |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `ad_format` | 2 | String | — | — | Format QC |
| `ad_platform` | 2 | String | — | — | Mediation platform |
| `ad_network` | 2 | String | — | — | Network |
| `placement` | 2 | String | — | — | Vị trí show |
| `ad_unit_id` | 3 | String | — | — | Ad unit ID |

---

<a id="ad_complete"></a>

### ad_complete

| Field | Value |
|-------|-------|
| **Event Definition** | Quảng cáo kết thúc |
| **Trigger** | Khi user xem xong hoặc tắt QC full màn (open ad, interstitial, rewarded, fullscreen native) |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `ad_format` | 2 | String | interstitial,video_rewarded | — | Format QC |
| `ad_platform` | 2 | String | — | — | Mediation platform |
| `ad_network` | 2 | String | — | — | Network |
| `placement` | 2 | String | — | — | Vị trí show |
| `ad_unit_id` | 3 | String | — | — | Ad unit ID |
| `end_type` | 3 | String | quit,done | — | Chỉ rewarded: quit=tắt giữa chừng, done=xem hết+nhận thưởng |
| `duration_ad` | 3 | Number | msec | >0 | Thời gian xem QC |

---

<a id="notifications"></a>

## Notifications — `DataBucketNotification`

<a id="noti_send"></a>

### noti_send

| Field | Value |
|-------|-------|
| **Event Definition** | Game gửi notification |
| **Trigger** | Khi game gửi noti |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `noti_cate` | 2 | String | remind,event,daily_quest | — | Nhóm nội dung noti |
| `noti_name` | 2 | String | — | — | Tên tóm gọn nội dung noti (có thể dùng headline) |

<a id="noti_receive"></a>

### noti_receive

| Field | Value |
|-------|-------|
| **Event Definition** | User nhận được notification |
| **Trigger** | Khi user nhận được noti |

Params giống `noti_send`.

<a id="noti_open"></a>

### noti_open

| Field | Value |
|-------|-------|
| **Event Definition** | User mở notification |
| **Trigger** | Khi user mở noti |

Params giống `noti_send`.

---

<a id="live-ops"></a>

## Live Ops — `DataBucketLiveOps`

<a id="feature_first_show"></a>

### feature_first_show

| Field | Value |
|-------|-------|
| **Event Definition** | Feature được show lần đầu (icon/popup gợi ý) |
| **Trigger** | Khi icon/popup gợi ý feature show lần đầu. Không log với features mặc định show từ đầu. |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `feature_name` | 2 | String | daily_reward,lucky_wheel | — | Tên feature |
| `placement` | 3 | String | home_icon,home_popup,end_level_icon | — | Vị trí show |

<a id="feature_open"></a>

### feature_open

| Field | Value |
|-------|-------|
| **Event Definition** | User mở feature |
| **Trigger** | Khi user mở tính năng |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `feature_name` | 2 | String | — | — | Tên feature |
| `placement` | 3 | String | — | — | Vị trí mở |
| `open_type` | 3 | String | popup,click | Giá trị enum | popup=tự động, click=user chủ động |
| `open_index` | 3 | Number | 1,2,3... | >=1 | Lần thứ mấy user mở loại feature này |

<a id="feature_close"></a>

### feature_close

| Field | Value |
|-------|-------|
| **Event Definition** | User thoát/đóng feature |
| **Trigger** | Khi user click X hoặc back để đóng feature |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `feature_name` | 2 | String | — | — | Tên feature |
| `placement` | 3 | String | — | — | Vị trí mở feature |
| `open_index` | 3 | Number | 1,2,3... | >=1 | Lần thứ mấy mở |
| `duration_feature` | 3 | Number | msec | >0 | Thời gian ở trong feature |

---

<a id="other-metrics"></a>

## Other Metrics — `DataBucketMetrics`

<a id="tutorial_action"></a>

### tutorial_action

| Field | Value |
|-------|-------|
| **Event Definition** | User thực hiện action theo tutorial |
| **Trigger** | Khi user thực hiện bất kỳ action nào theo tutorial. "start" khi bắt đầu, "finish" khi hoàn thành. |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `action_name` | 2 | String | start,finish,click_1,use_booster_A | — | Tên action |
| `action_index` | 2 | Number | 0,1,2... | >=0 | Thứ tự action (0=start) |
| `action_cate` | 3 | String | click,use_booster | — | Nhóm action |

```csharp
DataBucketMetrics.TutorialAction("start", 0);
DataBucketMetrics.TutorialAction("click_1", 1, actionCate: "click");
DataBucketMetrics.TutorialAction("finish", 10);
```

---

<a id="button_click"></a>

### button_click

| Field | Value |
|-------|-------|
| **Event Definition** | User bấm button quan trọng (không trigger với button đã có event riêng) |
| **Trigger** | Khi user bấm button cần theo dõi |
| **KPI** | Phân tích hành vi/lựa chọn |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `button_name` | 2 | String | Accept,Quit,Back | — | Tên button |
| `screen_name` | 2 | String | setting,lose_confirm | — | Màn hiện tại |

---

<a id="screen_show"></a>

### screen_show

| Field | Value |
|-------|-------|
| **Event Definition** | User đến/mở/nhìn thấy một màn |
| **Trigger** | Khi user đến màn quan trọng. Không trigger với màn đã có event riêng (iap_show, ad_impression). |
| **KPI** | Phân tích thời gian user ở mỗi màn |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `screen_name` | 2 | String | setting,lose_confirm | — | Tên màn |
| `button_name` | 3 | String | — | — | Button user bấm để đến màn |
| `prev_screen_name` | 3 | String | — | — | Màn trước |
| `duration_prev_screen` | 3 | Number | msec | >=0 | Thời gian ở màn trước |

---

<a id="screen_exit"></a>

### screen_exit

| Field | Value |
|-------|-------|
| **Event Definition** | User thoát game |
| **Trigger** | Khi user thoát game (kill app, background, remove app) |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `prev_screen_name` | 2 | String | — | — | Màn hình trước khi thoát |
| `duration_prev_screen` | 3 | Number | msec | >=0 | Thời gian ở màn trước khi thoát |

---

<a id="technical-performance"></a>

## Technical Performance — `DataBucketTechnical`

> **KPI chung:** Time to data, Success rate.

<a id="loading_start"></a>

### loading_start

| Field | Value |
|-------|-------|
| **Event Definition** | Bắt đầu tiến trình tải dữ liệu (API/CDN/SDK) |
| **Trigger** | Khi app phát động gọi dữ liệu |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `data_source` | 2 | String | api,cdn,sdk,store,remote_config | Giá trị enum | Nguồn dữ liệu |
| `resource_type` | 2 | String | feed,content_detail,search,user_profile,paywall | Giá trị enum | Loại dữ liệu |
| `load_context` | 2 | String | app_open,home,content_detail | Giá trị enum | Màn hình/luồng nơi gọi |
| `trigger_source` | 3 | String | screen_appear,user_action,background,prefetch | Giá trị enum | Nguyên nhân khởi phát |
| `load_id` | 3 | String | UUID | — | UUID duy nhất (cần nếu nhiều request song song) |
| `priority` | 3 | String | critical,high,normal,low,prefetch | Giá trị enum | Độ ưu tiên |

---

<a id="loading_finish"></a>

### loading_finish

| Field | Value |
|-------|-------|
| **Event Definition** | Kết thúc tiến trình tải dữ liệu (thành công hoặc thất bại) |
| **Trigger** | Khi nhận được kết quả request |

| Param | Level | Type | Values | Value Req | Definition |
|-------|-------|------|--------|-----------|------------|
| `data_source` | 2 | String | api,cdn,sdk,store,remote_config | Giá trị enum | Nguồn dữ liệu |
| `resource_type` | 2 | String | — | Giá trị enum | Loại dữ liệu |
| `load_context` | 2 | String | — | Giá trị enum | Màn hình/luồng |
| `result` | 2 | String | success,fail,canceled,timeout,offline | Giá trị enum | Kết quả |
| `load_time` | 2 | Number | msec | >=0 | Thời gian từ start đến finish |
| `trigger_source` | 3 | String | — | Giá trị enum | Nguyên nhân |
| `load_id` | 3 | String | UUID | — | UUID duy nhất |
| `priority` | 3 | String | — | Giá trị enum | Độ ưu tiên |
| `error_code` | 3 | String | network_timeout,server_error,http_404_not_found | — | Mã lỗi khi fail |
| `response_bytes` | 3 | Number | bytes | >=0 | Kích thước response |
| `retry_count` | 3 | Number | 0,1,2... | >=0 | Số lần thử lại (0=thành công lần đầu) |

```csharp
DataBucketTechnical.LoadingStart("api", "feed", "home", triggerSource: "screen_appear");
DataBucketTechnical.LoadingFinish("api", "feed", "home", "success", 350L, triggerSource: "screen_appear");
```
