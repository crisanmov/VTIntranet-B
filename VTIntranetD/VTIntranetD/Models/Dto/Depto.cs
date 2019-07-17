using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranetD.Models.Dto
{
    public class Depto
    {
        public string idTag { get; set; }
        public int idParent { get; set; }
        public int idDepto { get; set; }
    }
}