using eSya.EmailEngine.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.IF
{
    public interface IEmailStatementRepository
    {
        Task<DO_EmailProviderCredential> EmailProviderCredential(int BusinessKey, int emailType);
        Task<List<DO_EmailStatement>> GetEmailonSaveClick(DO_EmailParameter sp);
    }
}
