using CodeGenEngine.Abstract;
using CodeGenEngine.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEngine
{
    public class Property : Variable, IElement, IMapped
    {
        public Property(AccessOperator accessOperator, bool generateGetter, bool generateSetter, string name, DataType dataType, string defaultValue) : base(name, dataType, defaultValue)
        {
            AccessOperator = accessOperator;
            GenerateGetter = generateGetter;
            GenerateSetter = generateSetter;
        }

        public AccessOperator AccessOperator { get; set; }

        public bool GenerateGetter { get; set; }
        public bool GenerateSetter { get; set; }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Dictionary<string, string> GetMapping()
        {
            return new Dictionary<string, string>()
            {
                { "NAME", this.Name },
                { "DATATYPE", this.DataType.Key },
                { "ACCESSOPERATOR", this.AccessOperator.ToString().ToLower() },
            };
        }
    }
}
