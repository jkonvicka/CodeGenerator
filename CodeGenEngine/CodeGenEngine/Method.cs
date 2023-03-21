using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class Method : IElement
    {
        public string Name { get; set; }
        public DataType ReturnType { get; set; }
        public List<Argument> Arguments { get; set; }
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
