using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class CodeGenerator
    {
        private Language language { get; set; } = new("");
        public void Generate(string @namespace)
        {
            StringBuilder sb = new StringBuilder();


            Class @class = new()
            {
                Name = "User",
                AccessOperator = AccessOperator.PUBLIC,
                Includes = new List<Include>()
                {
                    new Include("System"),
                    new Include("System.Text")
                },
                BaseClasses = new List<BaseClass>()
                {
                    new BaseClass("IUser"),
                    new BaseClass("DatabaseObject")
                },
                Properties= new List<Property>()
                {
                    new Property(AccessOperator.PUBLIC, "Id", new DataType("long"), null),
                    new Property(AccessOperator.PUBLIC, "Name", new DataType("string"), "string.Empty"),
                    new Property(AccessOperator.PRIVATE, "_workNumber", new DataType("long"), null),
                }
            };


            /*LANG SPECIFICATION*/
            language.Keywords.Add(Keyword.INCLUDE, "using");
            language.Keywords.Add(Keyword.CLASS, "class");


            language.AccessOperators.Add(AccessOperator.PUBLIC, "public");
            language.AccessOperators.Add(AccessOperator.PRIVATE, "private");
            language.AccessOperators.Add(AccessOperator.PROTECTED, "protected");


            string output = language.GetCode(@class, @namespace);



            Console.WriteLine(output);

        }
        public void Generate(Class @class, string @namespace, string outputPath)
        {

        }
    }
}
