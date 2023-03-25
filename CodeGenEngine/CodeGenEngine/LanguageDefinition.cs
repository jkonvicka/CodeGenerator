using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class LanguageDefinition
    {
        public string IncludeTemplate { get; set; } = string.Empty;
        public string NamespaceTemplate { get; set; } = string.Empty;
        public string ClassDefinitionWithoutBaseClassTemplate { get; set; } = string.Empty;
        public string ClassDefinitionWithBaseClassTemplate { get; set; } = string.Empty;
        public string PropertyDefinititonTemplate { get; set; } = string.Empty;
        public string PropertyGetterTemplate { get; set; } = string.Empty;
        public string PropertySetterTemplate { get; set; } = string.Empty;
        public string OpenDefinitonBodyTemplate { get; set; } = string.Empty;
        public string CloseDefinitionBodyTemplate { get; set; } = string.Empty;

    }
}
