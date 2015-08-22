using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTMS.Models;
namespace OTMS.Controllers
{
    public class BidController : Controller
    {
        //
        // GET: /Bid/
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult AddBid() 
        {   //  create a new bid 
            if (Request.HttpMethod.Equals("POST"))
            {
                   BidModel bid = new BidModel();
                   bid.Owner = (String)Session["username"];
                   bid.NoticeId = int.Parse(Request.Form["noticeid"]);
                   HttpPostedFileBase file = Request.Files["PDF"];

                if (file != null && file.ContentLength > 0)     //  upload PDF document
                {
                    System.IO.Stream fileStream = file.InputStream;
                    byte[] data = new byte[file.ContentLength];
                    fileStream.Read(data, 0, data.Length);
                    fileStream.Close();
                    bid.PdfDoc = data;
                }

                DBContext.GetInstance().CreateNewBid(bid);
            }
            return RedirectToAction("BidDashboard", "Bidder");      //  return to dashboard after creating it.
        }
        public ActionResult AddNewBid(String noticeId) 
        {
            if (Session["isOrg"] != null)   //  if not in a session 
            {
                if (!(bool)Session["isOrg"]) //  If bidders session
                {
                    ViewData["noticeId"] = noticeId;
                    return View();  //  return correct view
                }
            }
            return null;    // do nothing
        }
	}
}