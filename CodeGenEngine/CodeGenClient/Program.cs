
using CodeGenEngine;
using Newtonsoft.Json;

LanguageDeclaration CSharp = new LanguageDeclaration()
{
    IncludeTemplate = "using INCLUDE;",
    NamespaceTemplate = "namespace NAMESPACE;",
    ClassDeclarationWithBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME : <BASECLASES>",
    ClassDeclarationWithoutBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME",
    PropertyDefinititonTemplate = "ACCESSOPERATOR DATATYPE NAME { get; set; }",
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
LanguageDeclaration Java = new LanguageDeclaration()
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
LanguageDeclaration Cpp = new LanguageDeclaration()
{
    IncludeTemplate = "#include <INCLUDE>;",
    ClassDeclarationWithBaseClassTemplate = "class CLASSNAME : <BASECLASES>",
    ClassDeclarationWithoutBaseClassTemplate = "class CLASSNAME",
    PropertyDefinititonTemplate = "DATATYPE NAME;",
    PropertyGetterTemplate = "DATATYPE getNAME() { return this.NAME; }",
    PropertySetterTemplate = "void setNAME(DATATYPE _NAME) { this.NAME = _NAME; }",
    OpenDefinitonBodyTemplate = "{",
    CloseDefinitonBodyTemplate = "}",
    DefaultConstructorDeclarationTemplate = "CLASSNAME()",
    ParameterizedConstructorDeclarationTemplate = "CLASSNAME(<ARGUMENTS>)",
    PublicMethodDeclarationTemplate = "DATATYPE NAME(<ARGUMENTS>)",
    PrivateMethodDeclarationTemplate = "DATATYPE NAME(<ARGUMENTS>)",
    ArgumentWithoutDefaultValueTemplate = "DATATYPE NAME",
    ArgumentWithDefaultValueTemplate = "DATATYPE NAME = DEFAULTVALUE",
    ClassTemplate = "INCLUDES_DECLARATION\r\n\r\nCLASS_DECLARATION \r\n{\r\nprivate:\r\nPRIVATE_PROPERTIES_DECLARATION\r\nPRIVATE_METHODS_DECLARATION\r\npublic:\r\nPUBLIC_PROPERTIES_DECLARATION\r\nGETTERS_AND_SETTERS_DECLARATION\r\nDEFAULT_CONSTRUCTOR_DECLARATION\r\nPARAMETRIZED_CONSTRUCTOR_DECLARATION\r\nPUBLIC_METHODS_DECLARATION\r\n}",
    PropertyInitializationTemplate = "this.NAME = NAME;"
};
LanguageDeclaration Python = new LanguageDeclaration()
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

CodeGenerator generator = new CodeGenerator(CSharp);

Class @class = new()
{
    Name = "User",
    NameSpace = "DemoApplication",
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
    Properties = new List<Property>()
                {
                    new Property(AccessOperator.PUBLIC, true, true, "Id", new DataType("long")),
                    new Property(AccessOperator.PRIVATE, true, true,  "Name", new DataType("string"), "string.Empty"),
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
var generatedCode = generator.Generate(@class);
Console.WriteLine(generatedCode);
