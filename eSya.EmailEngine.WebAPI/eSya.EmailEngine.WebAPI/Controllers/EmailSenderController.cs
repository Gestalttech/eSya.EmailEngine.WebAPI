using eSya.EmailEngine.IF;
using eSya.EmailEngine.DO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eSya.EmailEngine.WebAPI.Utility;
using System.Net.Mail;

namespace eSya.EmailEngine.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailSenderController : ControllerBase
    {
        private readonly ICommonDataRepository _CommonDataRepository;
        private readonly IEmailStatementRepository _EmailStatementRepository;
        private readonly IEmailSender _EmailSender;
        public EmailSenderController(ICommonDataRepository commonDataRepository, IEmailStatementRepository emailStatementRepository, IEmailSender emailSender)
        {
            _CommonDataRepository = commonDataRepository;
            _EmailStatementRepository = emailStatementRepository;
            _EmailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> SendeSysEmail(DO_EmailParameter sp_obj)
        {
            var emailModal = new DO_EmailModel();

            var ds = await _CommonDataRepository.GetLocationEmailApplicable(sp_obj.BusinessKey);
            var mail_SP = await _EmailStatementRepository.EmailProviderCredential(sp_obj.BusinessKey, sp_obj.EmailType);
            if (ds == true && mail_SP.SenderEmailId != null)
            {
                var fs = await _EmailStatementRepository.GetEmailonSaveClick(sp_obj);
                string messageText = "";
                foreach (var s in fs)
                {
                    foreach (var r in s.l_EmailParam.Where(w => w.Emailid != null))
                    {
                        if (r.ParameterID == (int)emailParams.User)
                        {
                            sp_obj.LoginID = r.ID;
                            sp_obj.UserName = r.Name;
                            sp_obj.Emailid = r.Emailid;
                        }
                    }
                    messageText = DynamicTextReplaceByVariables(s.EmailBody, sp_obj);
                    foreach (var p in s.l_EmailParam.Where(w => w.Emailid != null))
                    {
                        emailModal.ToEmail = p.Emailid;
                        emailModal.CCEmail = null;
                        emailModal.Subject = s.EmailSubject;
                        emailModal.Message = messageText;

                        if (!string.IsNullOrEmpty(p.Emailid))
                        {
                            var rp = await _EmailSender.SendEmail(emailModal, mail_SP);
                        }
                    }

                }

            }
            return Ok();
        }

        private string DynamicTextReplaceByVariables(string smsTemplate, DO_EmailParameter sv)
        {
            //foreach (var sv in smsVariables)
            //{
            //    smsTemplate = smsTemplate.Replace(sv.Key, sv.Value);
            //}
            if (!string.IsNullOrEmpty(sv.LoginID))
                smsTemplate = smsTemplate.Replace("V00001", sv.LoginID);
            if (!string.IsNullOrEmpty(sv.UserName))
                smsTemplate = smsTemplate.Replace("V00002", sv.UserName);
            if (!string.IsNullOrEmpty(sv.OTP))
                smsTemplate = smsTemplate.Replace("V00003", sv.OTP);
            //if (!string.IsNullOrEmpty(sv.Password))
            //    smsTemplate = smsTemplate.Replace("V0004", sv.Password);
            //if (!string.IsNullOrEmpty(sv.Name))
            //    smsTemplate = smsTemplate.Replace("V0006", sv.Name);
            //if (sv.ScheduleDate != null)
            //    smsTemplate = smsTemplate.Replace("V0007", sv.ScheduleDate.Value.ToString("dd-MMM-yyyy"));

            return smsTemplate;
        }
    }
}
