using eSya.EmailEngine.IF;
using eSya.EmailEngine.DO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.SMSEngine.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailEngineController : ControllerBase
    {
        private readonly IEmailEngineRepository _EmailEngineRepository;
        public EmailEngineController(IEmailEngineRepository emailEngineRepository)
        {
            _EmailEngineRepository = emailEngineRepository;
        }

        /// <summary>
        /// Get Email Variable Information.
        /// UI Reffered - Email Variable
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetEmailVariableInformation()
        {
            var sm_sv = await _EmailEngineRepository.GetEmailVariableInformation();
            return Ok(sm_sv);
        }

        /// <summary>
        /// Get Active Email Variable Information.
        /// UI Reffered - Email Information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveEmailVariableInformation()
        {
            var sm_sv = await _EmailEngineRepository.GetActiveEmailVariableInformation();
            return Ok(sm_sv);
        }


        /// <summary>
        /// Insert into Email Variable .
        /// UI Reffered - Email Variable
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoEmailVariable(DO_EmailVariable obj)
        {
            var msg = await _EmailEngineRepository.InsertIntoEmailVariable(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Update Email Variable .
        /// UI Reffered - Email Variable
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateEmailVariable(DO_EmailVariable obj)
        {
            var msg = await _EmailEngineRepository.UpdateEmailVariable(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Email Variable.
        /// UI Reffered - Email Variable
        /// </summary>
        /// <param name="status-smsvariable"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveEmailVariable(bool status, string Emavariable)
        {
            var msg = await _EmailEngineRepository.ActiveOrDeActiveEmailVariable(status, Emavariable);
            return Ok(msg);
        }
    }
}
