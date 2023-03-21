using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class Class : IElement
    {
        public string Name { get; set; }
        public AccessOperator AccessOperator { get; set; }
        public List<Include> Includes {get; set;} = new();
        public List<BaseClass> BaseClasses { get; set;} = new();
        public List<Property> Properties {get; set;} = new();
        public List<Method> Methods { get; set;} = new();

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
