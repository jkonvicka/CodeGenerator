using CodeGenEngine.Abstract;
using CodeGenEngine.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class Argument : Variable, IElement, IMapped
    {
        public Argument(string name, DataType dataType, string defaultValue = null) : base(name, dataType, defaultValue) { }

        public void Accept(IVisitor visitor)
        {
            this.Accept(visitor);
        }

        public Dictionary<string, string> GetMapping()
        {
            return new Dictionary<string, string>()
            {
                { "NAME", Name },
                { "DATATYPE", DataType.Key },
                { "DEFAULTVALUE", DefaultValue }

            };
        }
    }
}
