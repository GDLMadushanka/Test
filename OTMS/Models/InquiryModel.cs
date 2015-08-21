using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTMS.Models
{
    public class InquiryModel
    {
        public int InquiryId { get; set; }
        public int NoticeId { get; set; }
        public String User { get; set; }
        public String Question { get; set; }
        public String Answer { get; set; }
        public bool Answered { get; set; }
        
        
    }
}