using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class LanguageDefinition
    {
        public class AccessOperator
        {
            public string Private { get; set; }
            public string Public { get; set; }
            public string Protected { get; set; }
        }

        public string ClassKeyword { get; set; }
        public string ReturnKeyword { get; set; }
        public string NamespaceDefinition { get; set; }

    }
}
