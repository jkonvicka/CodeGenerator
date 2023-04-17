using CodeGenEngine;
using System.Reflection.Emit;

namespace UnitTests
{
    public class JavaLanguageUnitTest
    {
        LanguageDeclaration _language { get; set; }
        CodeGenerator _generator { get; set; }
        [SetUp]
        public void Setup()
        {
            _language = new LanguageDeclaration()
            {
                IncludeTemplate = "import INCLUDE;",
                ClassDeclarationWithBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME extends <BASECLASES>",
                ClassDeclarationWithoutBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME",
                PropertyDefinititonTemplate = "ACCESSOPERATOR DATATYPE NAME;",
                PropertyGetterTemplate = "ACCESSOPERATOR DATATYPE getNAME() { return this.NAME; }",
                PropertySetterTemplate = "ACCESSOPERATOR void setNAME(DATATYPE _NAME) { this.NAME = _NAME; }",
                OpenDefinitonBodyTemplate = "{",
                CloseDefinitonBodyTemplate = "}",
                DefaultConstructorDeclarationTemplate = "ACCESSOPERATOR CLASSNAME()",
                ParameterizedConstructorDeclarationTemplate = "ACCESSOPERATOR CLASSNAME(<ARGUMENTS>)",
                PublicMethodDeclarationTemplate = "ACCESSOPERATOR DATATYPE NAME(<ARGUMENTS>)",
                PrivateMethodDeclarationTemplate = "ACCESSOPERATOR DATATYPE NAME(<ARGUMENTS>)",
                ArgumentWithoutDefaultValueTemplate = "DATATYPE NAME",
                ArgumentWithDefaultValueTemplate = "DATATYPE NAME = DEFAULTVALUE",
                ClassTemplate = "INCLUDES_DECLARATION\r\nNAMESPACE_DECLARATION\r\nCLASS_DECLARATION\r\n{\r\nPRIVATE_PROPERTIES_DECLARATION\r\nPUBLIC_PROPERTIES_DECLARATION\r\nDEFAULT_CONSTRUCTOR_DECLARATION\r\nPARAMETRIZED_CONSTRUCTOR_DECLARATION\r\nGETTERS_AND_SETTERS_DECLARATION\r\nPUBLIC_METHODS_DECLARATION\r\nPRIVATE_METHODS_DECLARATION\r\n}",
                PropertyInitializationTemplate = "this.NAME = NAME;"
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
                NameSpace = "DemoApplication",
                AccessOperator = AccessOperator.PUBLIC,
                Includes = new List<Include>()
                {
                    new Include("java.util.ArrayList"),
                    new Include("java.util.HashMap")
                },
                BaseClasses = new List<BaseClass>()
                {
                    new BaseClass("Human"),
                    new BaseClass("DatabaseObject")
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