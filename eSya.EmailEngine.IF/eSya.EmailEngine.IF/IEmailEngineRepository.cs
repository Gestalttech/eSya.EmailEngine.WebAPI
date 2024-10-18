using eSya.EmailEngine.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.IF
{
    public interface IEmailEngineRepository
    {
        #region Email Variable

        Task<List<DO_EmailVariable>> GetEmailVariableInformation();

        Task<List<DO_EmailVariable>> GetActiveEmailVariableInformation();

        Task<DO_ReturnParameter> InsertIntoEmailVariable(DO_EmailVariable obj);

        Task<DO_ReturnParameter> UpdateEmailVariable(DO_EmailVariable obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveEmailVariable(bool status, string Emavariable);

        #endregion Email Variable

        #region SMS Information
        Task<List<DO_Forms>> GetExistingFormsFromEmailHeader();
        Task<List<DO_EmailHeader>> GetEmailHeaderInformationByFormId(int formId);

        Task<DO_EmailHeader> GetEmailHeaderInformationByEmailId(string emailTempId);

        Task<DO_ReturnParameter> InsertIntoEmailHeader(DO_EmailHeader obj);

        Task<DO_ReturnParameter> UpdateEmailHeader(DO_EmailHeader obj);

        #endregion

        #region Email Recipient
        Task<List<DO_EmailHeader>> GetEmailHeaderForRecipientByFormIdandParamId(int formId, int parameterId);
        Task<List<DO_EmailRecipient>> GetEmailRecipientByBusinessKeyAndEmailTempId(int businessKey, string emailTempId);
        Task<DO_ReturnParameter> InsertIntoEmailRecipient(DO_EmailRecipient obj);
        Task<DO_ReturnParameter> UpdateEmailRecipient(DO_EmailRecipient obj);
        #endregion
        #region Manage Email Location Wise
        Task<List<DO_EmailHeader>> GetEmailInformationFormLocationWise(int businessKey, int formId);
        Task<DO_ReturnParameter> InsertOrUpdateEmailInformationFLW(List<DO_BusinessFormEmailLink> obj);
        #endregion
    }
}
