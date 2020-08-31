using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGSCIMB.Models
{
    public class File
    {
        public int ID { get; set; }
        public string ArticleTitle { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string fileName { get; set; }
        public int countReader { get; set; }
    }
}
