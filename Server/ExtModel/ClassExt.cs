using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtModel
{
    public class ClassExt
    {
        public string Name { get; set; } = string.Empty;
        public string NameSpace { get; set; } = string.Empty;
        public AccessOperatorExt AccessOperator { get; set; }
        public List<IncludeExt> Includes { get; set; } = new();
        public List<BaseClassExt> BaseClasses { get; set; } = new();
        public List<PropertyExt> Properties { get; set; } = new();
        public List<MethodExt> Methods { get; set; } = new();
    }
}
