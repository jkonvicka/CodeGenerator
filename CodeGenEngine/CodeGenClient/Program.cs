
using CodeGenEngine;

LanguageDefinition CSharp = new LanguageDefinition()
{
    IncludeTemplate = "using INCLUDE;",
    NamespaceTemplate = "namespace NAMESPACE",
    ClassDefinitionWithBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME : <BASECLASES>",
    ClassDefinitionWithoutBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME",
    PropertyDefinititonTemplate = "ACCESSOPERATOR DATATYPE NAME { get; set; }",
    PropertyGetterTemplate = "ACCESSOPERATOR DATATYPE GetNAME() { return this.NAME; }",
    PropertySetterTemplate = "ACCESSOPERATOR void SetNAME(DATATYPE _NAME) { this.NAME = _NAME; }",
    OpenDefinitonBodyTemplate = "{",
    CloseDefinitionBodyTemplate = "}",

};

LanguageDefinition Java = new LanguageDefinition()
{
    IncludeTemplate = "import INCLUDE;",
    ClassDefinitionWithBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME extends <BASECLASES>",
    ClassDefinitionWithoutBaseClassTemplate = "ACCESSOPERATOR class CLASSNAME",
    PropertyDefinititonTemplate = "ACCESSOPERATOR DATATYPE NAME;",
    PropertyGetterTemplate = "ACCESSOPERATOR DATATYPE getNAME() { return this.NAME; }",
    PropertySetterTemplate = "ACCESSOPERATOR void setNAME(DATATYPE _NAME) { this.NAME = _NAME; }",
    OpenDefinitonBodyTemplate = "{",
    CloseDefinitionBodyTemplate = "}",

};


CodeGenerator generator = new CodeGenerator(Java);

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
