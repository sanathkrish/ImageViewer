using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model.Pagination
{
    public class PaginationDataResponse<T> where T : class
    {
        public int TotalRecords { get; set; }
        public List<T> Data { get; set; }
    }
}
