using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.shop
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="اجباری می باشد")]
        public  string Name { get; set; }
        [Required]
        public  string Description { get; set; }
        
        [Range(10000,500000,ErrorMessage ="مبلغ باید بین {0} و {1} باشد")]
        public long Price { get; set; }
        public long PriceOld { get; set; } = 0;
        public string? Feauchers { get; set; }
        public string? ShortFeauchers { get; set; }
        public bool IsPublish { get; set; } = false;
        public  string Images { get; set; }
       
        public  string Cdate { get; set; }
       
        public  string Mdate { get; set; }
        public int Seen { get; set; } = 0;
        public int CatalogId { get; set; }
        public virtual Catalog? Catalog { get; set; }


      

    }
}
