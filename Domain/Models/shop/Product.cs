using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.shop
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public long PriceOld { get; set; }
        public string Feauchers { get; set; }
        public bool IsPublish { get; set; }
        public string Images { get; set; }

        public int CatalogId { get; set; }
        public virtual Catalog? Catalog { get; set; }
    }
}
