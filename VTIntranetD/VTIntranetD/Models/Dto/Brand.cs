using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranetD.Models.Dto
{
    public class Brand
    {
        public int IdTag { get; set; }
        public int IdDepto { get; set; }
        public string TagName { get; set; }
        public string TagClabe { get; set; }
        public string DeptoName { get; set; }
        public string DeptoClabe { get; set; }
        public int Active { get; set; }
    }
}