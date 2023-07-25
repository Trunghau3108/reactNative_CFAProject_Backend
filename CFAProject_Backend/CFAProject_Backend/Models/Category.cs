using System;
using System.Collections.Generic;

namespace CFAProject_Backend.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        /// <summary>
        /// Mã loại
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tên chủng loại
        /// </summary>
        public string NameVn { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? TypeCar { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
