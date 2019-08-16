using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranetD.Models.Dto
{
    public class SessionModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }

        public string ProfileID { get; set; }

        public string RolName { get; set; }
        public bool UserActive { get; set; }

    }
}
