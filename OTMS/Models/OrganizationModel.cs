using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTMS.Models
{
    public class OrganizationModel
    {
        public String Name { get; set; }
        public String Address { get; set; }
        public String TPNo { get; set; }
        public String Email { get; set; }
        public String Web { get; set; }
        public byte[] Logo { get; set; }
        public String userName { get; set; }
        public String password { get; set; }
    }
}