using System;
using System.Collections.Generic;

namespace CFAProject_Backend.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Mật khẩu đăng nhập
        /// </summary>
        public string? Password { get; set; } = null!;
        /// <summary>
        /// Họ và tên
        /// </summary>
        public string? Fullname { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; } = null!;
        /// <summary>
        /// Hình
        /// </summary>
        public string? Photo { get; set; }
        /// <summary>
        /// Đã kích hoạt hay chưa
        /// </summary>
        public bool? Activated { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
