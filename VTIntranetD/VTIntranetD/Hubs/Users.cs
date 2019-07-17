using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranetD.Hubs
{
    public class Users
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public string LoginTime { get; set; }
    }
}