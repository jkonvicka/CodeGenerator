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
        [HttpPost("GenerateClass", Name = "GenerateClass")]
        public GeneratedCodeExt GenerateClass(GenerateClassModel generateClassModel)
        {
            _logger.LogInformation("GenerateClass called");
            LanguageDeclaration languageDeclaration = _languageDictionaryService.GetLanguage(generateClassModel.Language);
            _logger.LogInformation($"Language choosen: {generateClassModel.Language}");
            CodeGenerator cg = new CodeGenerator(languageDeclaration);

            string generatedCode = cg.Generate(generateClassModel.ClassSpecification.ConvertToInternal());
            _logger.LogInformation($"CodeGenerator created");
            var result = new GeneratedCodeExt()
            {
                FileName = $"{generateClassModel.ClassSpecification.Name}.{languageDeclaration.FileExtensionType}",
                Code = generatedCode
            };
            return result;
        }

        /// <summary>
        /// Generates classes
        /// </summary>
        /// <returns></returns>
        [HttpPost("GenerateClasses", Name = "GenerateClasses")]
        public GeneratedCodeExt[] GenerateClasses(GenerateClassesModel generateClassesModel)
        {
            LanguageDeclaration languageDeclaration = _languageDictionaryService.GetLanguage(generateClassesModel.Language);
            _logger.LogInformation($"Language choosen: {generateClassesModel.Language}");
            CodeGenerator cg = new CodeGenerator(languageDeclaration);
            List<GeneratedCodeExt> generatedCodeList = new List<GeneratedCodeExt>();
            foreach (var @class in generateClassesModel.ClassSpecification)
            {
                var code = cg.Generate(@class.ConvertToInternal());
                var generatedClass = new GeneratedCodeExt()
                {
                    FileName = $"{@class.Name}.{languageDeclaration.FileExtensionType}",
                    Code = code
                };
                generatedCodeList.Add(generatedClass);
            }

            return generatedCodeList.ToArray();
        }
    }
}