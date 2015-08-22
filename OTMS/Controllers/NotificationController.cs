using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTMS.Models;
namespace OTMS.Controllers
{
    public class NotificationController : Controller
    {
        //
        // GET: /Notification/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateType1Notification(String tenderID)    // notification type 1 - "new inquiry about tender notice"
        {
            NotificationModel noti = new NotificationModel();
            noti.NoticeId = int.Parse(tenderID);
            noti.Organization = DBContext.GetInstance().getOrganizationOfNotice(tenderID);
            DBContext.GetInstance().createNotificationType1(noti);  //  create db entry
            return Json(null, JsonRequestBehavior.AllowGet);    // enable ajax request.
        }

        public ActionResult CreateType2Notification(String tenderID,String user)
        {                                                       //  notification type 2 - "your inquiry is now answered"
            NotificationModel noti = new NotificationModel();
            noti.NoticeId = int.Parse(tenderID);
            noti.User = user;
            DBContext.GetInstance().createNotificationType2(noti);
            return Json(null, JsonRequestBehavior.AllowGet);    //  enable ajax
        }
        public ActionResult CreateType3Notification(String tenderID,String bidder)
        {                                            //  notification type 2 - "congratulations, your bid is selected as winning bid"
            NotificationModel noti = new NotificationModel();
            noti.NoticeId = int.Parse(tenderID);
            noti.User = bidder; 
            DBContext.GetInstance().createNotificationType3(noti);
            return Json(null, JsonRequestBehavior.AllowGet);    //  enable ajax
        }

        public ActionResult caughtNotification(String notificationID)
        {                                           //  set notification as caught, so it would not be notified again 
            DBContext.GetInstance().caughtNotification(notificationID);
            return Json(null, JsonRequestBehavior.AllowGet);
        }

	}
}