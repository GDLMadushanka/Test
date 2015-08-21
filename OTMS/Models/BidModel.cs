using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTMS.Models
{
    public class BidModel
    {
        public int BidId { get; set; }
        public String Owner { get; set; }
        public int NoticeId { get; set; }
        public byte[] PdfDoc { get; set; }
    }
}