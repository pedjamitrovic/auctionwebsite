namespace AuctionWebsite.Models
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Net.Mail;

    public static class Utility
    {
        public static string EncodePassword(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            var md5password = md5.ComputeHash(Encoding.Unicode.GetBytes(password));
            return BitConverter.ToString(md5password).Replace("-", string.Empty).ToLower();
        }
        public static bool IsValidEmailAddress(string emailaddress)
        {
            try
            {
                Regex rx = new Regex(@"^[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](\.?[-!#$%&'*+/0-9=?A-Z^_a-z{|}~])*@[a-zA-Z](-?[a-zA-Z0-9])*(\.[a-zA-Z](-?[a-zA-Z0-9])*)+$");
                return rx.IsMatch(emailaddress);
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static void SendEmail(string email, string subject, string body)
        {
            MailMessage mail = new MailMessage("auctionwebsitemp150608@gmail.com", email);
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("auctionwebsitemp150608@gmail.com", "peki1996"),
            };

            client.Send(mail);
        }
    }

    public class InvalidActionException : Exception
    {
        public InvalidActionException()
        {
        }

        public InvalidActionException(string message)
            : base(message)
        {
        }

        public InvalidActionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
