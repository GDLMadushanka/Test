using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTMS.Models;
namespace OTMS.Controllers
{
    public class OrganizationController : Controller
    {
        //
        // GET: /Organization/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OrgDashboard()  //  view where all controllers for organization reisdes 
        {
            List<TenderNoticeModel> Tenders = DBContext.GetInstance().findTendersOfOrg("organization", (String)Session["username"]);
            ViewData["org"] = Session["username"];
            ViewData["num"] =Tenders.Count;
            ViewData["tenders"] = Tenders;
            ViewData["notifications"] = DBContext.GetInstance().findNotificationsOfOrg((String)Session["username"]);
            return View();
        }

        public ActionResult CreateNewTender()
        {
            //eryery
            if (Request.HttpMethod.Equals("POST"))  //  create new tender by organization
            {
                TenderNoticeModel tenderModel = new TenderNoticeModel();
                tenderModel.Organization = (String)Session["username"];
                tenderModel.FieldName = Request.Form["select"];
                tenderModel.ExpDateTime = Convert.ToDateTime(Request.Form["dateTime"]); //  expiring date 
                tenderModel.SubDateTime = DateTime.Now;
                HttpPostedFileBase file = Request.Files["PDF"];     //  upload PDF

                if (file != null && file.ContentLength > 0)
                {
                    System.IO.Stream fileStream = file.InputStream;
                    byte[] data = new byte[file.ContentLength];
                    fileStream.Read(data, 0, data.Length);
                    fileStream.Close();
                    tenderModel.PdfDoc = data;
                }
               DBContext.GetInstance().createTenderNotice(tenderModel);
             
            }
            return View();
        }

        public ActionResult OrganizationLogin()
        {
            if (Session["isOrg"] != null)
            {
                if ((bool)Session["isOrg"]) //  if already logged on redirect to appropiate dashboard
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
                OrganizationModel existing = DBContext.GetInstance().FindOneInOrganization("username", Request.Form["username"]);
                char[] temp = null;

                temp = existing.password.ToCharArray();
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = (char)((int)Convert.ToChar(temp[i]) - 5);   // ReHash Function
                }
                String tem = new String(temp);
                if (existing != null && tem.Equals(Request.Form["password"]))
                {
                    Session["username"] = existing.userName;    //  create new session for new user
                    Session["name"] = existing.Name;
                    Session["isOrg"] = true;
                    Session["profilePic"] = existing.Logo;
                    ViewData["message"] = "successfull";
                    return RedirectToAction("OrgDashboard", "Organization");
                }
                else
                {
                    ViewData["hasError"] = 1;               //  error messges
                    ViewData["errorMsg"] = "Username or Password not match";
                }

            }
            return View();
        }

        public ActionResult getAllOrganizations(String OrgUserName)     //  AJAX update to get all organizations
        {
            List<OrganizationModel> orgList = DBContext.GetInstance().getOrganizationsOfNotice(OrgUserName);

            return Json(orgList, JsonRequestBehavior.AllowGet); //  return JSON object
        }


	}
}