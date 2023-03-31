using ExtModel;
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
        public GeneratedCodeExt GenerateClass()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}