using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace CareerTech.Utils
{

    public class CommonService
    {
        public static List<string> GetAddresses()
        {
            List<string> list = new List<string>();
            list.Add("Hà Nội");
            list.Add("Hồ Chí Minh");
            list.Add("Bình Dương");
            list.Add("Bắc Ninh");
            list.Add("Đồng Nai");
            list.Add("Hưng Yên");
            list.Add("Hải Dương");
            list.Add("Đà Nẵng");
            list.Add("Hải Phòng");
            list.Add("An Giang");
            return list;
        }

        public static bool Send(string smtpUserName, string smtpPassword, string smtpHost, int smtpPort, string toEmail, string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Host = smtpHost;
                    smtpClient.Port = smtpPort;
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                    var msg = new MailMessage
                    {
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(smtpUserName),
                        Subject = subject,
                        Body = body,
                        Priority = MailPriority.Normal,
                    };
                    msg.To.Add(toEmail);
                    smtpClient.Send(msg);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }



    }
}