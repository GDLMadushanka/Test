using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTMS.Models;
namespace OTMS.Controllers
{
    public class BidderController : Controller
    {
        //
        // GET: /Bidder/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BidDashboard() //   View with all basic controllers for bidders
        {
            List<TenderNoticeModel> Tenders = DBContext.GetInstance().findTenders("userName", (String)Session["userName"]);
           
            ViewData["tenders"] = Tenders;
            ViewData["empty"] = "false";
            ViewData["notifications"] = DBContext.GetInstance().findNotificationsOfUser((String)Session["userName"]);
            if (Tenders.Count == 0) { ViewData["empty"] = true; }
            return View();
        }
        public ActionResult Bidderlogin() //    Bidder is logging to the system  
        {
            if (Session["isOrg"] != null)
            {
                if ((bool)Session["isOrg"]) //  If wrong session redirect to correct dashboard
                {
                    return RedirectToAction("OrgDashboard", "Organization");
                }
                else
                {
                    return RedirectToAction("BidDashboard", "Bidder");
                }
            }

            if (Request.HttpMethod.Equals("POST"))
            {
                BidderModel existing = DBContext.GetInstance().FindOneInBidder("username", Request.Form["username"]);
                char[] temp=null; 
                
                    temp = existing.password.ToCharArray();
                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] = (char)((int)Convert.ToChar(temp[i]) - 5);   // ReHash Function
                    }
                    String tem = new String(temp);
                if (existing != null && tem.Equals(Request.Form["password"]))
                {
                    Session["user"] = existing.Name;                //  creating a new session for recognizing users
                    Session["username"] = existing.userName;
                    Session["isOrg"] = false;                       //  identify user type
                    Session["profilePic"] = existing.ProfilePic;
                    ViewData["message"] = "successfull";            //  error hadling messages
                    return RedirectToAction("BidDashboard", "Bidder");
                }
                else
                {
                    ViewData["hasError"] = 1;                       //  error handling messages.
                    ViewData["errorMsg"] = "Username or Password not match";
                }

            }
           
            return View();
        }
	}
}