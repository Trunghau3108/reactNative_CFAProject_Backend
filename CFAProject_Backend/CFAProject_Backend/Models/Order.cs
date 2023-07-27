using System;
using System.Collections.Generic;

namespace CFAProject_Backend.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        /// <summary>
        /// Mã hóa đơn
        /// </summary>
        public int Id { get; set; }
        public int ProductId { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Ngày đặt hàng
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// Ngày cần có hàng
        /// </summary>
        public DateTime ReturnDate { get; set; }
        /// <summary>
        /// Tên người nhận
        /// </summary>
        public string Receipt { get; set; } = null!;
        /// <summary>
        /// Địa chỉ nhận
        /// </summary>
        public string Address { get; set; } = null!;
        /// <summary>
        /// Ghi chú thêm
        /// </summary>
        public string? Description { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
