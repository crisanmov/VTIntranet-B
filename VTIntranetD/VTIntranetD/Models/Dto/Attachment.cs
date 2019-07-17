using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranetD.Models.Dto
{
    public class Attachment
    {
        public int IdAttachment { get; set; }
        public string AttachmentName { get; set; }
        public int IdDepto { get; set; }
        public string DeptoName { get; set; }
        public string AttachmentDirectory { get; set; }
        public string AttachmentActive { get; set; }

    }
}