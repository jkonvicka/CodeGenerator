using CodeGenEngine.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class Property : Variable, IElement
    {
        public Property(AccessOperator accessOperator, string name, DataType dataType, string defaultValue) : base(name, dataType, defaultValue)
        {
            AccessOperator = accessOperator;
        }

        public AccessOperator AccessOperator { get; set; }
        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
