using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerTech.Utils
{
    public class CommonConstants
    {
        public static string SUCCESS = "message-success";

        public static string DANGER = "message-danger";

        public static string URL_COVER_DEFAULT = "https://res.cloudinary.com/mockcareertech/image/upload/v1634036755/Default_cover_id3jbf.jpg";
       
        public static string URL_AVATAR_DEFAULT = "https://res.cloudinary.com/mockcareertech/image/upload/v1634036755/Default_avatar_rfwavo.png";

        public static string PDF_TYPE = "application/pdf";

        public static string PENDING_STATUS = "Pending";

        public static string APPROVED_STATUS = "Approved";

        // constant of send email service
        public static string SMTP_USER_NAME = "MockCareerTech@gmail.com";

        public static string SMTP_PASSWORD = "mockproject@123";

        public static string SMTP_HOST = "smtp.gmail.com";

        public static int SMTP_POST = 587;

        public static string EMAIL_SUBJECT = "Bạn vừa nhận được liên hê từ CareerTech";

        public static string PATH_TEMPLATE_EMAIL = "~/Content/EmailTemplate/Approve.html";
    }
}