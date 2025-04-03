using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.shop
{
    public class Order
    {
        public int Id { get; set; }
        [MaxLength(11)]
        [Required]
        public string Phone { get; set; }
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public string Cdate { get; set; }
        public bool IsNew { get; set; } = true;

    }
}
