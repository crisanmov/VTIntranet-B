using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranetD.Models.Dto
{
    public class EventFull
    {
        public int IdEvent { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public int IdActivity { get; set; }
    }
}