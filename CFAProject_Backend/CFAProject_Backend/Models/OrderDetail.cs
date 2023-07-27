using System;
using System.Collections.Generic;

namespace CFAProject_Backend.Models
{
    public partial class OrderDetail
    {
        /// <summary>
        /// Mã chi tiết
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Mã hóa đơn
        /// </summary>
        public int OrderId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Image { get; set; } = null!;
        /// <summary>
        /// Đơn giá bán
        /// </summary>
        public double UnitPrice { get; set; }
        /// <summary>
        /// Số lượng mua
        /// </summary>
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string SupplierId { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;
    }
}
