using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.IO;

namespace OTMS.Models
{
    public class DBContext
    {
        private static DBContext instance = null;
        private MySqlConnection DbConnection;
        public static DBContext GetInstance()
        {
            if (instance == null)
                instance = new DBContext();
              
            
            if (instance.DbConnection.State == System.Data.ConnectionState.Open)
                instance.DbConnection.Close();
                return instance;

        }

        private void CheckIfConeectionOpen()
        {
            if (instance.DbConnection.State == System.Data.ConnectionState.Open)
                instance.DbConnection.Close();

        }

        private DBContext()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            DbConnection = new MySqlConnection(connectionString);
        }


        
        public void CreateFieldListEntry(FieldListModel model) 
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO fieldlist (username,fieldname) values(@username,@fieldname)";
            command.Parameters.AddWithValue("@username", model.UserName);
            command.Parameters.AddWithValue("@fieldname", model.FieldName);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public void CreateNewBid(BidModel bid) 
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO bid (owner,noticeid,pdfdoc) values(@owner,@noticeid,@pdfdoc)";
            command.Parameters.AddWithValue("@owner", bid.Owner);
            command.Parameters.AddWithValue("@noticeid", bid.NoticeId);
            command.Parameters.AddWithValue("@pdfdoc", bid.PdfDoc);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public void createInquiry(InquiryModel inq) 
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO inquiry (noticeid,user,question) values(@noticeid,@user,@question)";
            command.Parameters.AddWithValue("@user",inq.User);
            command.Parameters.AddWithValue("@noticeid", inq.NoticeId);
            command.Parameters.AddWithValue("@question", inq.Question);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        #region newUser

        public void CreateBidder(BidderModel model) 
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO bidder (name,address,tpNo,profilePic,userName,password) values(@name,@address,@tpNo,@profilePic,@userName,@password)";
            command.Parameters.AddWithValue("@name", model.Name);
            command.Parameters.AddWithValue("@address", model.Address);
            command.Parameters.AddWithValue("@tpNo", model.tpNo);
            command.Parameters.AddWithValue("@profilePic", model.ProfilePic);
            command.Parameters.AddWithValue("@userName", model.userName);
            char[] temp = model.password.ToCharArray();
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] =(char)( (int)Convert.ToChar(temp[i])+5);   // Hash Function
            }
            command.Parameters.AddWithValue("@password", temp);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public void CreateOrganization(OrganizationModel model)
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO Organization (name,address,tpNo,email,web,logo,userName,password) values(@name,@address,@tpNo,@email,@web,@logo,@userName,@password)";
            command.Parameters.AddWithValue("@name", model.Name);
            command.Parameters.AddWithValue("@address", model.Address);
            command.Parameters.AddWithValue("@tpNo", model.TPNo);
            command.Parameters.AddWithValue("@email", model.Email);
            command.Parameters.AddWithValue("@web", model.Web);
            command.Parameters.AddWithValue("@logo", model.Logo);
            command.Parameters.AddWithValue("@userName", model.userName);
            
            char[] temp = model.password.ToCharArray();
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = (char)((int)Convert.ToChar(temp[i]) + 5);   // Hash Function
            }
            command.Parameters.AddWithValue("@password", temp);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }
        #endregion

        #region tenderNotice

        public void createTenderNotice(TenderNoticeModel model) 
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO tenderNotice (pdfDoc,organization,expdatetime,fieldname,subdatetime) values(@pdfDoc,@organization,@expdatetime,@fieldname,@subdatetime)";
            command.Parameters.AddWithValue("@pdfDoc", model.PdfDoc);
            command.Parameters.AddWithValue("@organization", model.Organization);
            command.Parameters.AddWithValue("@expdatetime", model.ExpDateTime);
            command.Parameters.AddWithValue("@fieldname", model.FieldName);
            command.Parameters.AddWithValue("@subdatetime", model.SubDateTime);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }


       
        #endregion

        #region find
        public BidderModel FindOneInBidder(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT *,length(profilePic) as p_size FROM bidder WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            BidderModel existing = null;
            if (reader.Read())
            {
                existing = new BidderModel();
                existing.userName = reader.GetString("username");
                existing.password = reader.GetString("password");
                existing.Name = reader.GetString("name");

                int statusIndex = reader.GetOrdinal("p_size");
                int size = reader.IsDBNull(statusIndex) ? 0 : (int)reader.GetUInt32(statusIndex);

                byte[] picture;
                if (size > 0)
                {
                    picture = new byte[size];
                    reader.GetBytes(reader.GetOrdinal("profilePic"), 0, picture, 0, picture.Length);
                }
                else
                    picture = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Content/empty_profile.gif"));
                existing.ProfilePic = picture;
            }

            DbConnection.Close();
            return existing;
        }
        public OrganizationModel FindOneInOrganization(string columnName, string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT *,length(logo) as p_size FROM organization WHERE " + columnName + "=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            OrganizationModel existing = null;
            if (reader.Read())
            {
                existing = new OrganizationModel();
                existing.userName = reader.GetString("username");
                existing.password = reader.GetString("password");
                existing.Name = reader.GetString("name");

                int statusIndex = reader.GetOrdinal("p_size");
                int size = reader.IsDBNull(statusIndex) ? 0 : (int)reader.GetUInt32(statusIndex);

                byte[] picture;
                if (size > 0)
                {
                    picture = new byte[size];
                    reader.GetBytes(reader.GetOrdinal("logo"), 0, picture, 0, picture.Length);
                }
                else
                    picture = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/Content/empty_profile.gif"));

                existing.Logo = picture;
            }

            DbConnection.Close();
            return existing;
        }

 #endregion

        public List<OrganizationModel> getOrganizationsOfNotice(String userName) 
        {
            DbConnection.Open(); 
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "select * from organization where username" + "=@value";
            command.Parameters.AddWithValue("@value", userName);
            MySqlDataReader reader = command.ExecuteReader();
            List<OrganizationModel> orglist = new List<OrganizationModel>();
            while (reader.Read())
            {
                OrganizationModel org = new OrganizationModel();
                org.Name = reader.GetString("name");
                org.Address = reader.GetString("address");
                org.Email = reader.GetString("email");
                org.TPNo = reader.GetString("tpno");
                org.Web = reader.GetString("web");
                orglist.Add(org);
            }
            DbConnection.Close();
            return orglist;
        }
        public List<TenderNoticeModel> findTenders(string columnName, string value) 
        {
           
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "select * from fieldlist natural join tendernotice WHERE " + columnName + "=@value order by SubDateTime";  
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            List<TenderNoticeModel> tenderList = new List<TenderNoticeModel>();
            while (reader.Read())
            {
                TenderNoticeModel tender = new TenderNoticeModel();
                tender.ExpDateTime = reader.GetDateTime("expdatetime");
                TimeSpan differnce = tender.ExpDateTime - DateTime.Now;
                if (Convert.ToDecimal(differnce.TotalMinutes) < 1) { continue; }
                tender.FieldName = reader.GetString("fieldname");
                tender.NoticeId = reader.GetInt32("noticeId");
                tender.SubDateTime = reader.GetDateTime("SubDateTime");
                tender.Organization = reader.GetString("organization");
                tenderList.Add(tender);
            }
            DbConnection.Close();
            return tenderList;
        }

        public List<BiddersOfNotice> FindBiddersOfTender(string tenderId)
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "select * from bid left join bidder on bid.owner = bidder.username where noticeid =@value";
            command.Parameters.AddWithValue("@value", tenderId);
            MySqlDataReader reader = command.ExecuteReader();
            List<BiddersOfNotice> biderList = new List<BiddersOfNotice>();
            while (reader.Read())
            {
                BiddersOfNotice bidder = new BiddersOfNotice();
                bidder.userName = reader.GetString("userName");
                bidder.BidId = reader.GetInt32("BidId");
                bidder.Name = reader.GetString("name");
                bidder.Address = reader.GetString("address");
                bidder.tpNo = reader.GetString("tpNo");
                biderList.Add(bidder);
            }
            DbConnection.Close();
            return biderList;
        }

        public List<InquiryModel> FindAllInquiries(String tenderID) 
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM inquiry where noticeid =@value";
            command.Parameters.AddWithValue("@value", tenderID);
            MySqlDataReader reader = command.ExecuteReader();
            List<InquiryModel> inqList = new List<InquiryModel>();
            while (reader.Read())
            {
                InquiryModel inq = new InquiryModel();
                try { reader.GetString("answer"); }
                catch
                {
                    inq.InquiryId = reader.GetInt32("inquiryId");
                    inq.Question = reader.GetString("question");
                    inq.User = reader.GetString("user");
                    inqList.Add(inq);
                }
            }
            DbConnection.Close();
            return inqList;
        }
        public byte[] getBidPdf(String bidId) 
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT pdfdoc,length(pdfdoc) as p_size FROM bid where bidid" + "=@value";
            command.Parameters.AddWithValue("@value", bidId);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {

                int statusIndex = reader.GetOrdinal("p_size");
                int size = reader.IsDBNull(statusIndex) ? 0 : (int)reader.GetUInt32(statusIndex);

                byte[] pdf = null;
                if (size > 0)
                {
                    pdf = new byte[size];
                    reader.GetBytes(reader.GetOrdinal("pdfdoc"), 0, pdf, 0, pdf.Length);
                }
                return pdf;
            }
            return null;
        }
        public List<TenderNoticeModel> findTendersOfOrg(string columnName, string value) 
        {
            
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "select * from tendernotice WHERE " + columnName + "=@value order by SubDateTime desc";  
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader =null;
            List<TenderNoticeModel> tenderList = new List<TenderNoticeModel>();
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TenderNoticeModel tender = new TenderNoticeModel();
                    tender.ExpDateTime = reader.GetDateTime("expdatetime");
                    TimeSpan differnce = tender.ExpDateTime - DateTime.Now;
                    tender.FieldName = reader.GetString("fieldname");
                    tender.NoticeId = reader.GetInt32("noticeId");
                    tender.SubDateTime = reader.GetDateTime("SubDateTime");
                    tender.Organization = reader.GetString("organization");
                    try
                    {
                        tender.AcceptedBidder = reader.GetString("acceptedbidder");
                        tender.AcceptanceNotice = reader.GetString("acceptancenotice");
                    }
                    catch
                    {
                        tender.AcceptedBidder = "";
                        tender.AcceptanceNotice = "";
                    }
                    tenderList.Add(tender);
                }
            }
            catch { }
            DbConnection.Close();
            return tenderList;
        }

        public byte[] findTenderPDF(String tenderID)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT pdfdoc,length(pdfdoc) as p_size FROM tendernotice where noticeid" + "=@value";
            command.Parameters.AddWithValue("@value", tenderID);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {

                int statusIndex = reader.GetOrdinal("p_size");
                int size = reader.IsDBNull(statusIndex) ? 0 : (int)reader.GetUInt32(statusIndex);

                byte[] pdf=null;
                if (size > 0)
                {
                    pdf = new byte[size];
                    reader.GetBytes(reader.GetOrdinal("pdfdoc"), 0, pdf, 0, pdf.Length);
                }
                return pdf;
            }
            return null;
        }

        

        public void answerInqiry(string inqId, string ans) 
        {
            
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "update inquiry set answer =@ans where inquiryid =@id";
            command.Parameters.AddWithValue("@ans", ans);
            command.Parameters.AddWithValue("@id", inqId);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }
       
        public void updateWinningBid(string tenderId, String acceptedbidder,String notice)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "update tendernotice set acceptedBidder=@bidder, acceptancenotice=@notice  where noticeID =@id";
            command.Parameters.AddWithValue("@bidder",acceptedbidder);
            command.Parameters.AddWithValue("@id", tenderId);
            command.Parameters.AddWithValue("@notice", notice);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public List<InquiryModel> findInquries(string value)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "select * from inquiry where noticeid=@value";
            command.Parameters.AddWithValue("@value", value);
            MySqlDataReader reader = command.ExecuteReader();
            List<InquiryModel> inqList = new List<InquiryModel>();
            while (reader.Read())
            {
                InquiryModel inq = new InquiryModel();
                inq.Question = reader.GetString("question");
                try
                {
                    inq.Answer = reader.GetString("answer");
                }
                catch
                {
                    inq.Answer = "";
                }
                inqList.Add(inq);
            }
            DbConnection.Close();
            return inqList;
        }

        public void createNotificationType1(NotificationModel notifi)
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO notification (type,caught,notificationtext,noticeid,date,organization) values(@type,@caught,@notificationtext,@noticeid,@date,@organization)";
            command.Parameters.AddWithValue("@type",1);
            command.Parameters.AddWithValue("@caught", false);
            command.Parameters.AddWithValue("@notificationtext","New inquiry about one of your tender : Tender ID "+notifi.NoticeId);
            command.Parameters.AddWithValue("@noticeid",notifi.NoticeId);
            command.Parameters.AddWithValue("@organization",notifi.Organization);
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public void createNotificationType2(NotificationModel notifi)
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO notification (type,caught,notificationtext,noticeid,date,user) values(@type,@caught,@notificationtext,@noticeid,@date,@user)";
            command.Parameters.AddWithValue("@type", 2);
            command.Parameters.AddWithValue("@caught", false);
            command.Parameters.AddWithValue("@notificationtext", "Your inquery about Tender ID " + notifi.NoticeId+" is now answered");
            command.Parameters.AddWithValue("@noticeid", notifi.NoticeId);
            command.Parameters.AddWithValue("@user", notifi.User);
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }
        public void createNotificationType3(NotificationModel notifi)
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "INSERT INTO notification (type,caught,notificationtext,noticeid,date,user) values(@type,@caught,@notificationtext,@noticeid,@date,@user)";
            command.Parameters.AddWithValue("@type", 3);
            command.Parameters.AddWithValue("@caught", false);
            command.Parameters.AddWithValue("@notificationtext", "Congratulations your bid selected as the winning bid for Tender ID " + notifi.NoticeId);
            command.Parameters.AddWithValue("@noticeid", notifi.NoticeId);
            command.Parameters.AddWithValue("@user", notifi.User);
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }

        public List<NotificationModel> findNotificationsOfUser(string UserName)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "select * from notification WHERE user =@value order by date asc";
            command.Parameters.AddWithValue("@value", UserName);
            MySqlDataReader reader = command.ExecuteReader();
            List<NotificationModel> NotificationList = new List<NotificationModel>();
            while (reader.Read())
            {
                if (reader.GetBoolean("caught") == true) continue;
                NotificationModel noti = new NotificationModel();
                noti.NotificationId = reader.GetInt32("notificationId");
                noti.NotificationText = reader.GetString("notificationText");
                NotificationList.Add(noti);
            }
            DbConnection.Close();
            return NotificationList;
        }
        public List<NotificationModel> findNotificationsOfOrg(string UserName)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "select * from notification WHERE organization =@value order by date asc";
            command.Parameters.AddWithValue("@value", UserName);
            MySqlDataReader reader = command.ExecuteReader();
            List<NotificationModel> NotificationList = new List<NotificationModel>();
            while (reader.Read())
            {
                if (reader.GetBoolean("caught") == true) continue;
                NotificationModel noti = new NotificationModel();
                noti.NoticeId = reader.GetInt32("noticeId");
                noti.NotificationId = reader.GetInt32("notificationId");
                noti.NotificationText = reader.GetString("notificationText");
                NotificationList.Add(noti);
            }
            DbConnection.Close();
            return NotificationList;
        }

        

        public Boolean verifyUserName(string UserName)
        {

            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM bidder where username =@value"; 
            command.Parameters.AddWithValue("@value", UserName);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                try { String name = reader.GetString("username"); return true; }
                catch { return false; }
            }
            DbConnection.Close();
            return false;
        }

        public Boolean verifyUserNameOrg(string UserName)
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM organization where username =@value";
            command.Parameters.AddWithValue("@value", UserName);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                try { String name = reader.GetString("username"); return true; }
                catch { return false; }
            }
            DbConnection.Close();
            return false;
        }
        public String getOrganizationOfNotice(String noticeID) 
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "SELECT organization from tendernotice where noticeid=@value";
            command.Parameters.AddWithValue("@value", noticeID);
            MySqlDataReader reader = command.ExecuteReader();
            String org="";
            while (reader.Read())
            {
                
              org = reader.GetString("organization");
            
            }
            DbConnection.Close();
            return org;
        }

        public void caughtNotification(string notificationId)
        {
            DbConnection.Open();
            MySqlCommand command = DbConnection.CreateCommand();
            command.CommandText = "update notification set caught = true where notificationid =@id";
            command.Parameters.AddWithValue("@id", notificationId);
            command.ExecuteNonQuery();
            DbConnection.Close();
        }
       

    }
}