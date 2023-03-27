
using CodeGenEngine;

LanguageDeclaration CSharp = new LanguageDeclaration()
{
    IncludeTemplate = "using INCLUDE;",
    NamespaceTemplate = "namespace NAMESPACE;",
    ClassDeclarationWithBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME : <BASECLASES>",
    ClassDeclarationWithoutBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME",
    PropertyDefinititonTemplate = "ACCESSOPERATOR DATATYPE NAME { get; set; }",
    PropertyGetterTemplate = "ACCESSOPERATOR DATATYPE GetNAME() { return this.NAME; }",
    PropertySetterTemplate = "ACCESSOPERATOR void SetNAME(DATATYPE _NAME) { this.NAME = _NAME; }",
    OpenDefinitonBodyTemplate = "{",
    CloseDeclarationBodyTemplate = "}",
    DefaultConstructorDeclarationTemplate = "ACCESSOPERATOR CLASSNAME()",
    ParameterizedConstructorDeclarationTemplate = "ACCESSOPERATOR CLASSNAME(<ARGUMENTS>)"

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
    CloseDeclarationBodyTemplate = "}",
    DefaultConstructorDeclarationTemplate = "ACCESSOPERATOR CLASSNAME()",
    ParameterizedConstructorDeclarationTemplate = "ACCESSOPERATOR CLASSNAME(<ARGUMENTS>)",

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
                    new Property(AccessOperator.PUBLIC, true, true, "Id", new DataType("long"), null),
                    new Property(AccessOperator.PUBLIC, true, true,  "Name", new DataType("string"), "string.Empty"),
                    new Property(AccessOperator.PRIVATE,  true, true, "_workNumber", new DataType("long"), null),
                }
};
generator.Generate(@class);
