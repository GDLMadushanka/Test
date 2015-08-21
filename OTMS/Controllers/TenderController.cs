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
        public ActionResult tenderDetails(String tenderID)  //  
        {
            List<TenderNoticeModel> tenderlist = DBContext.GetInstance().findTendersOfOrg( "noticeid",tenderID);
            List<BiddersOfNotice> bidders = DBContext.GetInstance().FindBiddersOfTender(tenderID);
            List<InquiryModel> inqList = DBContext.GetInstance().FindAllInquiries(tenderID);
            ViewData["tender"] = tenderlist.First();
            ViewData["Bidders"] = bidders;
            ViewData["inqList"] = inqList;
            return View();
        }

        public FileStreamResult getPDFofNotice(String noticeID)
        {   
            Response.Clear();
            MemoryStream ms = new MemoryStream(DBContext.GetInstance().findTenderPDF(noticeID));
            Response.ContentType = "application/pdf";
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
            return new FileStreamResult(ms, "application/pdf");
        }

        public ActionResult addNewInquiry(String noticeId,String Question)
        {
            InquiryModel inq = new InquiryModel
            {
                NoticeId = int.Parse(noticeId),
                Question = Question,
                User = (String)Session["user"],
                Answered = false
            };

             DBContext.GetInstance().createInquiry(inq);

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult answerInquiry(String inqueryId,String answer)
        {
           
             DBContext.GetInstance().answerInqiry(inqueryId,answer);

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult TenderWinner(String tenderID,String acceptedbidder,String notice)
        {
           
             DBContext.GetInstance().updateWinningBid(tenderID,acceptedbidder,notice);

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult takeAllInquries(String tenderId) 
        {
            List<InquiryModel> inq =DBContext.GetInstance().findInquries(tenderId);
            return Json(inq, JsonRequestBehavior.AllowGet);
        }
	}
}