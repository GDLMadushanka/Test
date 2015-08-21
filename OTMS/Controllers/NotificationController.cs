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
        public ActionResult CreateType1Notification(String tenderID)
        {
            NotificationModel noti = new NotificationModel();
            noti.NoticeId = int.Parse(tenderID);
            noti.Organization = DBContext.GetInstance().getOrganizationOfNotice(tenderID);
            DBContext.GetInstance().createNotificationType1(noti);

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateType2Notification(String tenderID,String user)
        {
            NotificationModel noti = new NotificationModel();
            noti.NoticeId = int.Parse(tenderID);
            noti.User = user;
            DBContext.GetInstance().createNotificationType2(noti);

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateType3Notification(String tenderID,String bidder)
        {
            NotificationModel noti = new NotificationModel();
            noti.NoticeId = int.Parse(tenderID);
            noti.User = bidder; 
            DBContext.GetInstance().createNotificationType3(noti);

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public ActionResult caughtNotification(String notificationID)
        {
           
            DBContext.GetInstance().caughtNotification(notificationID);

            return Json(null, JsonRequestBehavior.AllowGet);
        }

	}
}