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
        public CodeGenerator(LanguageDefinition languageDefiniton)
        {
            _language = new Language(languageDefiniton);
        }

        public void Generate(Class @class)
        {
            StringBuilder sb = new StringBuilder();


            string output = _language.GetCode(@class);

            Console.WriteLine(output);


            /*LANG SPECIFICATION c#*/
            /*            language.Keywords.Add(Keyword.INCLUDE, "using");
                        language.Keywords.Add(Keyword.CLASS, "class");
                        language.Keywords.Add(Keyword.ENDOFCOMMAND, ";");
                        language.Keywords.Add(Keyword.SETTERPREFIX, "Set");
                        language.Keywords.Add(Keyword.GETTERPREFIX, "Get");
                        language.Keywords.Add(Keyword.SELFCLASSPOINTER, "this");
                        language.Keywords.Add(Keyword.OPENBODY, "{");
                        language.Keywords.Add(Keyword.CLOSEBODY, "}");
                        language.Keywords.Add(Keyword.DEFINITIONSTART, "");


                        language.AccessOperators.Add(AccessOperator.PUBLIC, "public");
                        language.AccessOperators.Add(AccessOperator.PRIVATE, "private");
                        language.AccessOperators.Add(AccessOperator.PROTECTED, "protected");*/





            /*LANG SPECIFICATION Python*/
            /* language.Keywords.Add(Keyword.INCLUDE, "import");
             language.Keywords.Add(Keyword.CLASS, "class");
             language.Keywords.Add(Keyword.ENDOFCOMMAND, "");
             language.Keywords.Add(Keyword.SETTERPREFIX, "set");
             language.Keywords.Add(Keyword.GETTERPREFIX, "get");
             language.Keywords.Add(Keyword.SELFCLASSPOINTER, "self");
             language.Keywords.Add(Keyword.OPENBODY, "");
             language.Keywords.Add(Keyword.CLOSEBODY, "");
             language.Keywords.Add(Keyword.DEFINITIONSTART, ":");
             language.Keywords.Add(Keyword.PROPERTYTEMPLATE, "");*/

            /*
                        language.AccessOperators.Add(AccessOperator.PUBLIC, "");
                        language.AccessOperators.Add(AccessOperator.PRIVATE, "");
                        language.AccessOperators.Add(AccessOperator.PROTECTED, "");*/



        }
        public void Generate(Class @class, string @namespace, string outputPath)
        {

        }
    }
}
