using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTMS.Models
{
    public class NotificationModel
    {
        public int NotificationId { get; set; }
        public int Type { get; set; }
        public bool Caught { get; set; }
        public String NotificationText { get; set; }
        public String Date { get; set; }
        public String User{ get; set; }
        public String Organization { get; set; }
        public int NoticeId { get; set; }
    }
}