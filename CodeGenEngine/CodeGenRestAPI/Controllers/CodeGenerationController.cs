using CodeGenEngine;
using CodeGenRestAPI.Services.Language;
using CodeGenRestAPI.WrapperModel;
using Converts;
using ExtModel;
using Microsoft.AspNetCore.Mvc;

namespace CodeGenRestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeGeneratorController : ControllerBase
    {
        private readonly ILogger<CodeGeneratorController> _logger;
        private readonly ILanguageDictionaryService _languageDictionaryService;

        public CodeGeneratorController(ILogger<CodeGeneratorController> logger, ILanguageDictionaryService languageDictionaryService)
        {
            _logger = logger;
            _languageDictionaryService = languageDictionaryService;
        }

        /// <summary>
        /// Generates class
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "GenerateClass")]
        public string GenerateClass(GenerateClassModel generateClassModel)
        {
            LanguageDeclaration languageDeclaration = _languageDictionaryService.GetLanguage(generateClassModel.Language);

            CodeGenerator cg = new CodeGenerator(languageDeclaration);
            return cg.Generate(generateClassModel.ClassSpecification.ConvertToInternal());
        }
    }
}