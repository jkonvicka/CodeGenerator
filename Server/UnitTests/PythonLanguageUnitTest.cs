using CodeGenEngine;
using System.Reflection.Emit;

namespace UnitTests
{
    public class PythonLanguageUnitTest
    {
        LanguageDeclaration _language { get; set; }
        CodeGenerator _generator { get; set; }
        [SetUp]
        public void Setup()
        {
            _language = new LanguageDeclaration()
            {
                IncludeTemplate = "import INCLUDE",
                ClassDeclarationWithBaseClassTemplate = "class CLASSNAME(<BASECLASES>):",
                ClassDeclarationWithoutBaseClassTemplate = "class CLASSNAME:",
                PropertyDefinititonTemplate = "",
                PropertyGetterTemplate = "def getNAME() => DATAYPE:\n\t\treturn self.NAME",
                PropertySetterTemplate = "def setNAME(self, _NAME : DATATYPE) => DATAYPE:\n\t\tself.NAME = _NAME\n\t\treturn",
                OpenDefinitonBodyTemplate = "",
                CloseDefinitonBodyTemplate = "",
                DefaultConstructorDeclarationTemplate = "",
                ParameterizedConstructorDeclarationTemplate = "def __init__(self, <ARGUMENTS>):",
                PublicMethodDeclarationTemplate = "def NAME(self, <ARGUMENTS>) => DATAYPE:\n\t\tpass",
                PrivateMethodDeclarationTemplate = "def __NAME(self, <ARGUMENTS>) => DATAYPE:\n\t\tpass",
                ArgumentWithoutDefaultValueTemplate = "NAME : DATATYPE",
                ArgumentWithDefaultValueTemplate = "NAME : DATATYPE = DEFAULTVALUE",
                ClassTemplate = "INCLUDES_DECLARATION\r\nCLASS_DECLARATION\r\nDEFAULT_CONSTRUCTOR_DECLARATION\r\nPARAMETRIZED_CONSTRUCTOR_DECLARATION\r\nGETTERS_AND_SETTERS_DECLARATION\r\nPUBLIC_METHODS_DECLARATION\r\nPRIVATE_METHODS_DECLARATION",
                PropertyInitializationTemplate = "self.NAME = NAME"
            };
            _generator = new CodeGenerator(_language);
        }

        [Test]
        public void Test_CSharp_CodeGenerator()
        {
            CodeGenerator generator = new CodeGenerator(_language);
            Class @class = new()
            {
                Name = "User",
                AccessOperator = AccessOperator.PUBLIC,
                Includes = new List<Include>()
                {
                    new Include("os"),
                    new Include("socket"),
                    new Include("warnings")
                },
                BaseClasses = new List<BaseClass>()
                {
                    new BaseClass("Human")
                },
                Properties = new List<Property>()
                {
                    new Property(AccessOperator.PUBLIC, true, true, "Id", new DataType("long")),
                    new Property(AccessOperator.PRIVATE, true, true,  "Name", new DataType("string"), "\"\""),
                    new Property(AccessOperator.PRIVATE,  true, true, "_workNumber", new DataType("long")),
                },
                Methods = new List<Method>()
                {
                    new Method(AccessOperator.PRIVATE, "Sleep", new DataType("bool"),
                                                                    new List<Argument>(){
                                                                            new Argument("duration", new DataType("long"), "100"),
                                                                            new Argument("something", new DataType("char"))
                                                                                        }
                                                                    )
                }
            };
            string generatedCode = _generator.Generate(@class);
            Assert.IsFalse(string.IsNullOrEmpty(generatedCode));
            Console.WriteLine(generatedCode);
        }
    }
}