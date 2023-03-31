using Microsoft.AspNetCore.Mvc;

namespace CodeGenRestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeGeneratorController : ControllerBase
    {
        private readonly ILogger<CodeGeneratorController> _logger;

        public CodeGeneratorController(ILogger<CodeGeneratorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Generates class
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "GenerateClass")]
        public string GenerateClass()
        {
            return string.Empty;
        }
    }
}