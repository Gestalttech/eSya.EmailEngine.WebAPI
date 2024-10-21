using eSya.EmailEngine.DL.Repository;
using eSya.EmailEngine.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.EmailEngine.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonDataController : ControllerBase
    {
        private readonly ICommonDataRepository _commondataRepository;
        public CommonDataController(ICommonDataRepository commondataRepository)
        {
            _commondataRepository = commondataRepository;
        }
          /// <summary>
         /// Getting  Business Key.
         /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var ds = await _commondataRepository.GetBusinessKey();
            return Ok(ds);
        }

        /// <summary>
        /// Get Form Detail.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFormDetails()
        {
            var ds = await _commondataRepository.GetFormDetails();
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetFormDetailsbyBusinessKey(int businesskey)
        {
            var ds = await _commondataRepository.GetFormDetailsbyBusinessKey(businesskey);
            return Ok(ds);
        }

        /// <summary>
        /// Get Application Codes.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codeType)
        {
            var ds = await _commondataRepository.GetApplicationCodesByCodeType(codeType);
            return Ok(ds);
        }

        [HttpGet]
        public async Task<IActionResult> GetBusinessKeyByEmailIntegration()
        {
            var ds = await _commondataRepository.GetBusinessKeyByEmailIntegration();
            return Ok(ds);
        }
    }
}
