using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTMS.Models;
namespace OTMS.Controllers
{
    public class TenderController : Controller
    {
        //
        // GET: /Tender/
        public ActionResult Index()
        {
            return View();
        }
        public FileResult Download(String bidId)    //  donload a tender PDF
        {
            string contentType = "application/pdf";
            return File(DBContext.GetInstance().getBidPdf(bidId), contentType); //  return file result
        }
        public ActionResult tenderDetails(String tenderID)  //  take details of the tender given tender ID
        {
            if (Session["isOrg"] != null)   //  if not in a session 
            {
                if ((bool)Session["isOrg"]) //  If Org session
                {
                    List<TenderNoticeModel> tenderlist = DBContext.GetInstance().findTendersOfOrg("noticeid", tenderID);    //tender
                    List<BiddersOfNotice> bidders = DBContext.GetInstance().FindBiddersOfTender(tenderID);  // bids for that tender
                    List<InquiryModel> inqList = DBContext.GetInstance().FindAllInquiries(tenderID);    //  inquiries about tender
                    ViewData["tender"] = tenderlist.First();
                    ViewData["Bidders"] = bidders;
                    ViewData["inqList"] = inqList;
                    return View();
                }
            }
            return null;     
        }
        public FileStreamResult getPDFofNotice(String noticeID) //  given tender id return its PDF
        {   
            Response.Clear();
            MemoryStream ms = new MemoryStream(DBContext.GetInstance().findTenderPDF(noticeID));    //  PDF as a filestream
            Response.ContentType = "application/pdf";
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
            return new FileStreamResult(ms, "application/pdf"); //  return PDF as a filestream
        }

        public ActionResult addNewInquiry(String noticeId,String Question)  //  ask a question about tender
        {
            InquiryModel inq = new InquiryModel
            {
                NoticeId = int.Parse(noticeId),
                Question = Question,
                User = (String)Session["username"],
                Answered = false
            };
            DBContext.GetInstance().createInquiry(inq); //  create DB 
            return Json(null, JsonRequestBehavior.AllowGet);    //  allow ajax
        }
        public ActionResult answerInquiry(String inqueryId,String answer)   // answer a query about tender
        {
            DBContext.GetInstance().answerInqiry(inqueryId,answer);
            return Json(null, JsonRequestBehavior.AllowGet);    //  allow ajax
        }
        public ActionResult TenderWinner(String tenderID,String acceptedbidder,String notice)   //  organization selects a winner
        {
            DBContext.GetInstance().updateWinningBid(tenderID,acceptedbidder,notice);
            return Json(null, JsonRequestBehavior.AllowGet);    //  allow ajax
        }
        public ActionResult takeAllInquries(String tenderId)    //  Take all inquiries about a tender
        {
            List<InquiryModel> inq =DBContext.GetInstance().findInquries(tenderId);
            return Json(inq, JsonRequestBehavior.AllowGet); //  allow ajax
        }
	}
}