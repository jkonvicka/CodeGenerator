using CodeGenEngine.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class CodeGenerator
    {
        private ILanguage _language { get; set; }
        public CodeGenerator(ILanguageDeclaration languageDeclaration)
        {
            _language = new Language(languageDeclaration);
        }

        public string Generate(Class @class)
        {
            string output = _language.GetCode(@class);
            return output;
        }
        public void Generate(Class @class, string @namespace, string outputPath)
        {

        }
    }
}
