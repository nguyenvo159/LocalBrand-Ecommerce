using System;
using System.ComponentModel;

namespace backend.Text.Enums;

public class Enums
{
    public enum OrderStatus
    {
        [Description("Đang xử lý")]
        Pending = 0,

        [Description("Đã xác nhận")]
        Comfirmed = 1,

        [Description("Đang vận chuyển")]
        Shipping = 2,

        [Description("Đã giao hàng")]
        Done = 3,

        [Description("Đã hủy")]
        Cancelled = 4
    }

    public enum ShipType
    {
        [Description("Giao hàng tiêu chuẩn")]
        Standard = 1,

        [Description("Giao hàng nhanh")]
        Fast = 2,
        [Description("Giao hàng tiết kiệm")]
        Save = 3

    }

}
