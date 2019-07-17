using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranetD.Models.Dto
{
    public class Notice
    {
        public int IdNotice { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDateNotice { get; set; }
        public string EndDateNotice { get; set; }
    }
}