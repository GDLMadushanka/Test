using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTMS.Models;
using System.Net;
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
            if (Session["isOrg"] != null)
            {
                if (!(bool)Session["isOrg"]) //  if bidder
                {
                    List<TenderNoticeModel> Tenders = DBContext.GetInstance().findTenders("userName", (String)Session["userName"]);
                    ViewData["tenders"] = Tenders;
                    ViewData["empty"] = "false";
                    ViewData["notifications"] = DBContext.GetInstance().findNotificationsOfUser((String)Session["userName"]);
                    if (Tenders.Count == 0) { ViewData["empty"] = true; }
                    return View();   
                }
                else
                {// if org
                    return null; 
                }
            }
            return null;
        }
        public ActionResult Bidderlogin() //    Bidder is logging to the system  
        {
            if (Session["isOrg"] != null)
            {
                if ((bool)Session["isOrg"]) //  if company trying to enter bidders area
                {
                    return RedirectToAction("OrgDashboard", "Organization");    //  redirect to company dashboard
                }
                else
                {
                    return RedirectToAction("BidDashboard", "Bidder");  //  already logged in so redirect
                }
            }

            if (Request.HttpMethod.Equals("POST"))
            {
                BidderModel existing;
                try
                {
                    existing = DBContext.GetInstance().FindOneInBidder("username", Request.Form["username"]);
                    char[] temp = null;

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
                        ViewData["errorMsg"] = "Username or Password not match";
                        return View();
                        //  error handling
                    }
                }
                catch { ViewData["errorMsg"] = "Username or Password not match"; }
            }
            return View();
        }
	}
}