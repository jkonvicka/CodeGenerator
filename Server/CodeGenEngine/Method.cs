using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenEngine.Interface;

namespace CodeGenEngine
{
    public class Method : IElement, IMapped
    {
        public string Name { get; set; }
        public DataType ReturnType { get; set; }
        public AccessOperator AccessOperator { get; set; }
        public List<Argument> Arguments { get; set; }

        public Method(AccessOperator accessOperator, string name, DataType returnType, List<Argument> arguments)
        {
            Name = name;
            ReturnType = returnType;
            AccessOperator= accessOperator;
            Arguments = arguments;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Dictionary<string, string> GetMapping()
        {
            return new Dictionary<string, string>()
            {
                { "NAME", Name },
                { "DATATYPE", ReturnType.Key.ToString() },
                { "ACCESSOPERATOR", AccessOperator.ToString().ToLower() },

            };
        }
    }
}
