using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AtworkAPI.Models
{
    public class Image
    {
        public string Id { get; set; }
        public string RelationId { get; set; }
        public string RelationName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] ImageObject { get; set; }
    }
}