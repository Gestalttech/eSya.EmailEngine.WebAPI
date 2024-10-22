using eSya.EmailEngine.DO;
using System.Net.Mail;
using System.Net;
using eSya.EmailEngine.IF;

namespace eSya.EmailEngine.WebAPI.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task<DO_EmailResponse> SendEmail(DO_EmailModel em, DO_EmailProviderCredential ep)
        {
            var Items = new DO_EmailResponse();
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.To.Add(new MailAddress(em.ToEmail));  // replace with valid value 
                if (!string.IsNullOrEmpty(em.CCEmail))
                    mailMessage.CC.Add(em.CCEmail);
                mailMessage.From = new MailAddress(ep.SenderEmailId);  // replace with valid value
                mailMessage.Subject = em.Subject;
                mailMessage.Body = em.Message + System.Environment.NewLine;
                mailMessage.Body += "<br/>" + "***THIS IS AN SYSTEM GENERATED MESSAGE - PLEASE DO NOT REPLY DIRECTLY TO THIS EMAIL";
                mailMessage.IsBodyHtml = true;
                if (em.Attachment != null)
                    mailMessage.Attachments.Add(em.Attachment);

                Items.RequestMessage = "To" + mailMessage.To + mailMessage.Body;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = ep.UserName,
                        Password = !String.IsNullOrEmpty(ep.PassKey) ? ep.PassKey : ep.Password,
                    };
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Host = ep.OutgoingMailServer;
                    smtp.Port = ep.Port;
                    smtp.EnableSsl = true;
                    smtp.Send(mailMessage);

                    Items.SendStatus = true;
                    Items.ResponseMessage = "";
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                Items.SendStatus = false;
                Items.ResponseMessage = ex.Message;
            }

            return Items;
        }
    }
}
