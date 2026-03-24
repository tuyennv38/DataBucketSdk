# ADR-002: DataBucketPlugin — Static Wrapper Pattern

<a id="adr-static-wrapper-0001"></a>

`adr:static-wrapper-0001`
> Implements: [`prd:tech-stack-0002`](../PRDs/PRD-002.md#prd-tech-stack-0002)

## Bối cảnh (Context)

Databuckets Unity SDK v1.0.6 cung cấp 7 API qua static class `DatabucketsTracker`. Developer có thể gọi sai thứ tự (ví dụ: `Record()` trước `Init()`), dẫn đến mất event mà không có cảnh báo rõ ràng.

Cần một wrapper layer cung cấp:
- Kiểm tra Init state tự động
- Log Error khi gọi sai thứ tự
- Interface thống nhất, dễ sử dụng

## Quyết định (Decision)

Chọn **Static Class Pattern** cho wrapper:

```csharp
namespace DataBucketPlugin
{
    public static class DataBucketWrapper
    {
        private static bool _isInitialized = false;
        // ... static methods wrapping DatabucketsTracker
    }
}
```

**Lý do chọn Static Class thay vì MonoBehaviour Singleton:**
1. **Nhất quán với SDK gốc** — `DatabucketsTracker` cũng là static, developer đã quen
2. **Không cần GameObject** — giảm setup, gọi được ở bất kỳ đâu
3. **Đơn giản** — không cần quản lý lifecycle, DontDestroyOnLoad
4. **Performance** — không có overhead của MonoBehaviour

## Hệ quả (Consequences)

- ✅ API wrapper giữ nguyên pattern quen thuộc với developer
- ✅ Kiểm tra Init state tránh mất event
- ✅ Log Error rõ ràng giúp debug nhanh
- ⚠️ Không thể dùng Coroutine trong wrapper (nhưng không cần thiết)
- ⚠️ State (`_isInitialized`) tồn tại suốt app lifecycle, không reset khi đổi scene (đây là hành vi mong muốn)

## Trạng thái

- [x] Đề xuất (Proposed)
- [ ] Được chấp thuận (Accepted)
- [ ] Bị thay thế (Superseded)
