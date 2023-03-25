using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public enum Keyword
    {
        CLASS,
        INCLUDE,
        GETTERPREFIX,
        SETTERPREFIX,
        SELFCLASSPOINTER,
        ENDOFCOMMAND,
        OPENBODY,
        CLOSEBODY,
        DEFINITIONSTART,
        PROPERTYTEMPLATE
    }
}
