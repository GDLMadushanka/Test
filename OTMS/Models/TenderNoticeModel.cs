using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTMS.Models
{
    public class TenderNoticeModel
    {
        public int NoticeId { get; set; }
        public byte[] PdfDoc { get; set; }
        public String Organization { get; set; }
        public DateTime SubDateTime { get; set; }
        public DateTime ExpDateTime { get; set; }
        public String FieldName { get; set; }
        public String AcceptedBidder { get; set; }
        public String AcceptanceNotice { get; set; }

    }
}