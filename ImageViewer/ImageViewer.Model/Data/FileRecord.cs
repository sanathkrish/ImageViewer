using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Model.Data
{
    public class FileRecord
    {
        public int Id { get; set; }
        public int DriverId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Hash { get; set; }
    }
}

