namespace CodeGenEngine.Interface
{
    public interface ILanguageDeclaration
    {
        string ArgumentWithDefaultValueTemplate { get; set; }
        string ArgumentWithoutDefaultValueTemplate { get; set; }
        string ClassDeclarationWithBaseClassTemplate { get; set; }
        string ClassDeclarationWithoutBaseClassTemplate { get; set; }
        string ClassTemplate { get; set; }
        string CloseDefinitonBodyTemplate { get; set; }
        string DefaultConstructorDeclarationTemplate { get; set; }
        string FileExtensionType { get; set; }
        string IncludeTemplate { get; set; }
        string NamespaceTemplate { get; set; }
        string OpenDefinitonBodyTemplate { get; set; }
        string ParameterizedConstructorDeclarationTemplate { get; set; }
        string PrivateMethodDeclarationTemplate { get; set; }
        string PropertyDefinititonTemplate { get; set; }
        string PropertyGetterTemplate { get; set; }
        string PropertyInitializationTemplate { get; set; }
        string PropertySetterTemplate { get; set; }
        string PublicMethodDeclarationTemplate { get; set; }
    }
}