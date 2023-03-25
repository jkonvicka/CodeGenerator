using CodeGenEngine.Abstract;
using CodeGenEngine.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class Argument : Variable, IElement
    {
        public Argument(string name, DataType dataType, string defaultValue) : base(name, dataType, defaultValue) { }

        public void Accept(IVisitor visitor)
        {
            this.Accept(visitor);
        }
    }
}
