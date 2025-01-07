using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public long Price { get; set; }
        public bool IsPublish { get; set; } = false;
        public required string Images { get; set; }
        public required string Mdate { get; set; }
    }
}
