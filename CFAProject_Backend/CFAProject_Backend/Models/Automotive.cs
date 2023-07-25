using System;
using System.Collections.Generic;

namespace CFAProject_Backend.Models
{
    public partial class Automotive
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Fuel { get; set; }
        /// <summary>
        /// Tiện ích của xe
        /// </summary>
        public bool? Ac { get; set; }
        /// <summary>
        /// Tiện ích của xe
        /// </summary>
        public bool? Gps { get; set; }
        /// <summary>
        /// Tiện ích của xe
        /// </summary>
        public bool? Bluetooth { get; set; }
        /// <summary>
        /// Tiện ích của xe
        /// </summary>
        public bool? Usb { get; set; }
        public int? Driver { get; set; }
        public string? Engine { get; set; }
        public string? Capacity { get; set; }
        public int? Seats { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
