using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OTMS
{
    public class RouteConfig
    {   /* 
         * All the routes are configured here.
         * These are the only permitted rotes
         * Default configuration disablesd (controller/action)
         * So users cant guess and enter to unauthorised areas.
         */
  
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(    //  Bidders inquiry is notified to respective organization.
               name: "checkAvailabilityOrg",
               url: "User/checkAvailabilityOrg/{notificationID}",
               defaults: new { controller = "User", action = "checkAvailabilityOrg" }
           );

             routes.MapRoute(    //  Bidders inquiry is notified to respective organization.
                name: "checkAvailability",
                url: "User/checkAvailability/{notificationID}",
                defaults: new { controller = "User", action = "checkAvailability" }
            );
         
           routes.MapRoute(    //  Bidders inquiry is notified to respective organization.
                name: "caughtNotification",
                url: "Notification/caughtNotification/{notificationID}",
                defaults: new { controller = "Notification", action = "caughtNotification" }
            );

            routes.MapRoute(    //  Bidders inquiry is notified to respective organization.
                name: "CreateType1Notification",
                url: "Notification/CreateType1Notification/{tenderID}",
                defaults: new { controller = "Notification", action = "CreateType1Notification" }
            );

             routes.MapRoute(    //  Bidders inquiry is notified to respective organization.
                name: "CreateType2Notification",
                url: "Notification/CreateType2Notification/{tenderID},{user}",
                defaults: new { controller = "Notification", action = "CreateType2Notification" }
            );

            routes.MapRoute(    //  Bidders inquiry is notified to respective organization.
                name: "CreateType3Notification",
                url: "Notification/CreateType3Notification/{tenderID},{bidder}",
                defaults: new { controller = "Notification", action = "CreateType3Notification" }
            );


            routes.MapRoute(    //  Bidder search for past inqeries about selected tender
                name: "takeAllInquries",
                url: "Tender/takeAllInquries/{tenderID}",
                defaults: new { controller = "Tender", action = "takeAllInquries" }
            );

            routes.MapRoute(    //  Organization selects a winign tender
                name: "TenderWinner",
                url: "Tender/TenderWinner/{tenderID},{acceptedbidder},{notice}",
                defaults: new { controller = "Tender", action = "TenderWinner" }
            );

            routes.MapRoute(    //  Organization answer for an inquiry
                name: "answerInquiry",
                url: "Tender/answerInquiry/{inqueryId},{answer}",
                defaults: new { controller = "Tender", action = "answerInquiry" }
            );
            routes.MapRoute(    //  Bidder ask his/her question about the tender notice
                name: "addNewInquiry",
                url: "Tender/addNewInquiry/{noticeId},{Question}",
                defaults: new { controller = "Tender", action = "addNewInquiry" }
            );

            routes.MapRoute(    //  Organization download bidders subbmissions
                name: "Download",
                url: "Tender/Download/{bidId}",
                defaults: new { controller = "Tender", action = "Download" }
            );

            routes.MapRoute(    //  Bidder adding new bid
                name: "AddNewBid",
                url: "Bid/AddNewBid/{noticeID}",
                defaults: new { controller = "Bid", action = "AddNewBid" }
            );

            routes.MapRoute(    //  Organization viewing details of a selected tender
              name: "tenderDetails",
              url: "Tender/tenderDetails/{tenderID}",
              defaults: new { controller = "Tender", action = "tenderDetails" }
            );
            

            routes.MapRoute(    //  Bidder download a tender notice PDF
                name: "getPDFofNotice",
                url: "tender/getPDFofNotice/{noticeID}",
                defaults: new { controller = "tender", action = "getPDFofNotice" }
            );
            
            routes.MapRoute(    //  Take all organization names
                name: "getAllOrganizations",
                url: "organization/getAllOrganizations/{OrgUserName}",
                defaults: new { controller = "Organization", action = "getAllOrganizations", OrgUserName = "Don" }
            );

            routes.MapRoute(    //  default route alwaya point to home 
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
