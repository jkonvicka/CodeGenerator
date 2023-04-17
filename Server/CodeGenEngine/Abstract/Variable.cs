using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine.Abstract
{
    public abstract class Variable
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public string DefaultValue { get; set; }
        public Variable(string name, DataType dataType, string defaultValue)
        {
            Name = name;
            DataType = dataType;
            DefaultValue = defaultValue;
        }
    }
}
