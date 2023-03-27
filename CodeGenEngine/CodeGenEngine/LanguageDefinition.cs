using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class LanguageDeclaration
    {
        public string IncludeTemplate { get; set; } = string.Empty;
        public string NamespaceTemplate { get; set; } = string.Empty;
        public string ClassDeclarationWithoutBaseClassTemplate { get; set; } = string.Empty;
        public string ClassDeclarationWithBaseClassTemplate { get; set; } = string.Empty;
        public string PropertyDefinititonTemplate { get; set; } = string.Empty;
        public string PropertyGetterTemplate { get; set; } = string.Empty;
        public string PropertySetterTemplate { get; set; } = string.Empty;
        public string OpenDefinitonBodyTemplate { get; set; } = string.Empty;
        public string CloseDefinitonBodyTemplate { get; set; } = string.Empty;
        public string DefaultConstructorDeclarationTemplate { get; set; } = string.Empty;
        public string ParameterizedConstructorDeclarationTemplate { get; set; } = string.Empty;
        public string MethodDeclarationTemplate { get; set; } = string.Empty;
        public string ArgumentWithoutDefaultValueTemplate { get; set; } = string.Empty;
        public string ArgumentWithDefaultValueTemplate { get; set; } = string.Empty;
        public string ClassTemplate { get; set; } = string.Empty;
        public string SelfReferenceKeywordTemplate { get; set; } = string.Empty;

    }
}
