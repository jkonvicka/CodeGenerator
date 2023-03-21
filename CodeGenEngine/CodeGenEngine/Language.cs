using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeGenEngine
{
    public class Language : IVisitor, ILanguage
    {
        private string _nameSpace { get; set; }
        private StringBuilder sb { get; set; } = new StringBuilder();
        public Dictionary<Keyword, string> Keywords { get; set; } = new();
        public Dictionary<AccessOperator, string> AccessOperators { get; set; } = new();

        public Language(string input)
        {

        }

        public string GetCode(Class @class, string nameSpace)
        {
            _nameSpace = nameSpace;
            @class.Accept(this);
            return sb.ToString();
        }

        public void AddNamespace()
        {
            sb.AppendLine($"namespace {_nameSpace}");
        }

        public void AddInheritance(Class c)
        {
            if (c.BaseClasses.Count() >= 1)
            {
                sb.Append(" : ");
                var lastBaseClass = c.BaseClasses.Last();
                foreach (var baseClass in c.BaseClasses)
                {
                    baseClass.Accept(this);
                    if (!baseClass.Equals(lastBaseClass))
                    {
                        sb.Append(", ");
                    }
                }
                sb.Append("\n");
            }
        }

        public void AddProperties(Class c)
        {
            //PROPERTIES
            c.Properties.ForEach(p => p.Accept(this));
        }

        public void AddConstructor(Class c)
        {
            //CONSTRUCTOR
            sb.AppendLine($"\n\t\t{AccessOperators[c.AccessOperator]} {c.Name}()");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t}");
        }

        public void AddClassDefinition(Class c)
        {
            sb.Append($"\t{AccessOperators[c.AccessOperator]} {Keywords[Keyword.CLASS]} {c.Name}");
            AddInheritance(c);

            sb.AppendLine("\t{");
            AddProperties(c);
            AddConstructor(c);

            sb.AppendLine("\t}");
        }

        public void Visit(IElement element)
        {
            switch (element)
            {
                case Class c:
                    {
                        c.Includes.ForEach(i => i.Accept(this));
                        sb.AppendLine($"");
                        AddNamespace();
                        sb.AppendLine("{");

                        AddClassDefinition(c);


                        sb.AppendLine("}");

                    }
                    break;
                case Include include:
                    {
                        sb.AppendLine($"{Keywords[Keyword.INCLUDE]} {include.Name};");
                    }
                    break;
                case BaseClass baseClass:
                    {
                        sb.Append($"{baseClass.Name}");
                    }
                    break;
                case Property property:
                    {
                        sb.Append($"\t\t{AccessOperators[property.AccessOperator]} {property.DataType.Key} {property.Name}");
                        if (!string.IsNullOrEmpty(property.DefaultValue))
                        {
                            sb.Append($" = {property.DefaultValue}");
                        }
                        sb.AppendLine(";");
                    }
                    break;
            }
        }
    }
}
