using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class CodeGenerator
    {
        private Language _language { get; set; }
        public CodeGenerator(LanguageDeclaration languageDefiniton)
        {
            _language = new Language(languageDefiniton);
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
