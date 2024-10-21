using eSya.EmailEngine.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.EmailEngine.IF
{
    public interface ICommonDataRepository
    {
        Task<List<DO_BusinessLocation>> GetBusinessKey();
        Task<List<DO_Forms>> GetFormDetails();
        Task<List<DO_Forms>> GetFormDetailsbyBusinessKey(int businesskey);

        Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeType(int codeType);

        Task<List<DO_BusinessLocation>> GetBusinessKeyByEmailIntegration();
    }
}
