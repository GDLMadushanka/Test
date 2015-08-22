using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTMS.Models;

namespace OTMS.Controllers
{
    public class UserController : Controller
    {
        public ActionResult checkAvailability(String username)  //  check availability of a Bidder username - already taken or not.
        {
            Boolean x = DBContext.GetInstance().verifyUserName(username);
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        public ActionResult checkAvailabilityOrg(String username)//check availability of an organization username - already taken or not.
        {
            Boolean x = DBContext.GetInstance().verifyUserNameOrg(username);
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        public ActionResult registerNewBidder()     //  create a new bidder instance
        {
            if (Request.HttpMethod.Equals("POST"))
            {       
                BidderModel newBidder = new BidderModel()
                {
                    Name = Request.Form["name"],    //  take data from HTML form
                    Address = Request.Form["address"],
                    tpNo = Request.Form["telephoneNo"],
                    userName = Request.Form["username"],
                    password = Request.Form["password"]
                };
                    HttpPostedFileBase file = Request.Files["picture"];

                if (file != null && file.ContentLength > 0) //  upload file
                {
                    System.IO.Stream fileStream = file.InputStream;
                    byte[] data= new byte[file.ContentLength];
                    fileStream.Read(data, 0, data.Length);
                    fileStream.Close();
                    newBidder.ProfilePic= data;
                }

                BidderModel existing = DBContext.GetInstance().FindOneInBidder("username", newBidder.userName);
                if (existing == null)   //  see weather this this user is already existing. 
                {
                    DBContext.GetInstance().CreateBidder(newBidder);    // create db entry

                    if (Request.Form["chq1"] != null && Request.Form["chq1"] == "on")
                    {
                        FieldListModel field = new FieldListModel();    // registered fields
                        field.FieldName = "canteens";
                        field.UserName = newBidder.userName;
                        DBContext.GetInstance().CreateFieldListEntry(field);
                    }
                    if (Request.Form["chq2"] != null && Request.Form["chq2"] == "on")
                    {
                        FieldListModel field = new FieldListModel();
                        field.FieldName = "cleaning services";
                        field.UserName = newBidder.userName;
                        DBContext.GetInstance().CreateFieldListEntry(field);
                    }
                    if (Request.Form["chq3"] != null && Request.Form["chq3"] == "on")
                    {
                        FieldListModel field = new FieldListModel();
                        field.FieldName = "construction";
                        field.UserName = newBidder.userName;
                        DBContext.GetInstance().CreateFieldListEntry(field);
                    }
                    if (Request.Form["chq4"] != null && Request.Form["chq4"] == "on")
                    {
                        FieldListModel field = new FieldListModel();
                        field.FieldName = "delivery services";
                        field.UserName = newBidder.userName;
                        DBContext.GetInstance().CreateFieldListEntry(field);
                    }
                    if (Request.Form["chq5"] != null && Request.Form["chq5"] == "on")
                    {
                        FieldListModel field = new FieldListModel();
                        field.FieldName = "security services";
                        field.UserName = newBidder.userName;
                        DBContext.GetInstance().CreateFieldListEntry(field);
                    }
                    if (Request.Form["chq6"] != null && Request.Form["chq6"] == "on")
                    {
                        FieldListModel field = new FieldListModel();
                        field.FieldName = "vehicles";
                        field.UserName = newBidder.userName;
                        DBContext.GetInstance().CreateFieldListEntry(field);
                    }

                }
                else
                {
                    ViewData["success"] = 0;
                    ViewData["hasError"] = 1;
                    ViewData["errorMsg"] = "Username already exists";
                }
                return RedirectToAction("Bidderlogin", "Bidder");
            }
            return View();
        }

        public ActionResult registerNewOrganization()   //  create new organization
        {
            if (Request.HttpMethod.Equals("POST"))
            {
                OrganizationModel newOrganization = new OrganizationModel
                {                               //  take data from HTML form
                    Name = Request.Form["name"],
                    Address = Request.Form["address"],
                    TPNo = Request.Form["telephoneNo"],
                    Email = Request.Form["email"],
                    Web = Request.Form["web"],
                    userName = Request.Form["username"],
                    password = Request.Form["password"]
                };
                HttpPostedFileBase file = Request.Files["picture"]; //  upload profile picture

                if (file != null && file.ContentLength > 0)
                {
                    System.IO.Stream fileStream = file.InputStream;
                    byte[] data = new byte[file.ContentLength];
                    fileStream.Read(data, 0, data.Length);
                    fileStream.Close();
                    newOrganization.Logo= data;
                }

                OrganizationModel existing = DBContext.GetInstance().FindOneInOrganization("username", newOrganization.userName);
                if (existing == null)   //  if there's already an organization
                {
                    DBContext.GetInstance().CreateOrganization(newOrganization);
                }
                else
                {
                    ViewData["success"] = 0;
                    ViewData["hasError"] = 1;
                    ViewData["errorMsg"] = "Username already exists";
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            if (Session["isOrg"] != null)
            {
                bool isOrg = (bool)Session["isOrg"];
                Session.Clear();        //  clear session data when loggin out
                Session.Abandon();
                if (isOrg)          //  redirect to logig pages accordingly
                {
                    return RedirectToAction("OrganizationLogin", "Organization");
                }
                else
                {
                    return RedirectToAction("BidderLogin", "Bidder");
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}

