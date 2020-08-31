using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CGSCIMB.Models
{
    public class FileInfo
    {
        public int ID { get; set; }
        public string ArticleTitle { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<IFormFile> files { get; set; }
        public int countReader { get; set; }

    }
}
