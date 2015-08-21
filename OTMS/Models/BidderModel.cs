using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace OTMS.Models
{
    public class BidderModel
    {
       
        [Required]
        public String Name { get; set; }
        [Required]
        public String Address { get; set; }
        [Required]
        public String tpNo { get; set; }
        
        public byte[] ProfilePic { get; set; }
        [Required]
        public String userName { get; set; }
        [Required]
        public String password { get; set; }
        
    }
}