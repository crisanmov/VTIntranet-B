using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranetD.Models.Dto
{
    public class Area
    {
        public int IdDepto { get; set; }
        public int IdParent { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }
    }
}