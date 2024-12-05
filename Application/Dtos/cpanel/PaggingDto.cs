using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.cpanel
{
    public class PaggingDto<T>
    {
        public int TotalRecords { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
        public int Page { get; set; } = 0;
        public int Take { get; set; } = 1;
        public string? Search { get; set; }
        public List<T>? Values { get; set; }=new List<T>();
    }
}
