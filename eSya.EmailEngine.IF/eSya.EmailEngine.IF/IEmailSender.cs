using eSya.EmailEngine.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.IF
{
    public interface IEmailSender
    {
        Task<DO_EmailResponse> SendEmail(DO_EmailModel em, DO_EmailProviderCredential ep);
    }
}
