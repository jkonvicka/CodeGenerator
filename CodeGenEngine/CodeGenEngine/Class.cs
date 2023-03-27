using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenEngine.Interface;

namespace CodeGenEngine
{
    public class Class : IElement, IMapped
    {
        public string Name { get; set; }
        public string NameSpace {get; set; }
        public AccessOperator AccessOperator { get; set; }
        public List<Include> Includes {get; set;} = new();
        public List<BaseClass> BaseClasses { get; set;} = new();
        public List<Property> Properties {get; set;} = new();
        public List<Method> Methods { get; set;} = new();

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Dictionary<string, string> GetMapping()
        {
            return new Dictionary<string, string>()
            {
                { "NAMESPACE", NameSpace },
                { "ACCESSOPERATOR", AccessOperator.ToString().ToLower() },
                { "CLASSNAME", Name },
                { "<BASECLASES>", string.Join(", ", BaseClasses.Select(x=>x.Name)) }
            };
        }
    }
}
