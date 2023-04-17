using CodeGenEngine;
using CodeGenRestAPI.Services.Language;
using CodeGenRestAPI.WrapperModel;
using ExtModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeGenRestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LanguageManagementController : ControllerBase
    {

        private readonly ILogger<LanguageManagementController> _logger;
        private readonly ILanguageDictionaryService _languageDictionaryService;

        public LanguageManagementController(ILogger<LanguageManagementController> logger, ILanguageDictionaryService languageDictionaryService)
        {
            _logger = logger;
            _languageDictionaryService = languageDictionaryService;
        }
        /// <summary>
        /// Get supported languages 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLanguages", Name = "GetLanguages")]
        public string[] GetLanguages()
        {
            return _languageDictionaryService.GetAllKeys();
        }
    }
}
