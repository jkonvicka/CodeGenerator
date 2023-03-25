using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CodeGenEngine.Interface;

namespace CodeGenEngine
{
    public class Language : IVisitor, ILanguage
    {
        private StringBuilder sb { get; set; } = new StringBuilder();
        public LanguageDefinition Definition { get; set; }

        private int TabNum { get; set; } = 0;

        static string Tabs(int n)
        {
            return new string('\t', n);
        }

        public Language(LanguageDefinition definition)
        {
            Definition = definition;
        }

        public string GetCode(Class @class)
        {
            @class.Accept(this);
            return sb.ToString();
        }

        public void AddNamespace(Class c)
        {
            if (!string.IsNullOrEmpty(c.NameSpace))
            {
                sb.AppendLine(UseTemplate(c, Definition.NamespaceTemplate));
                TabNum += 1;
            }
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
            /*sb.AppendLine($"\n\t\t{AccessOperators[c.AccessOperator]} {c.Name}(){Keywords[Keyword.DEFINITIONSTART]}");
            sb.AppendLine($"\t\t{Keywords[Keyword.OPENBODY]}");
            sb.AppendLine($"\t\t{Keywords[Keyword.CLOSEBODY]}");*/
        }

        public void AddClassDefinition(Class c)
        {
            sb.Append(Tabs(TabNum));
            if (c.BaseClasses.Any())
            {
                sb.AppendLine(UseTemplate(c, Definition.ClassDefinitionWithBaseClassTemplate));
            }
            else
            {
                sb.AppendLine(UseTemplate(c, Definition.ClassDefinitionWithoutBaseClassTemplate));
            }
            sb.Append(Tabs(TabNum));
            sb.AppendLine($"{Definition.OpenDefinitonBodyTemplate}");
            var tabNumLocal = TabNum;
            TabNum++;
            AddProperties(c);
            AddConstructor(c);
            sb.Append(Tabs(tabNumLocal));
            sb.AppendLine($"{Definition.CloseDefinitionBodyTemplate}");
            /*sb.Append($"\t{AccessOperators[c.AccessOperator]} {Keywords[Keyword.CLASS]} {c.Name}{Keywords[Keyword.DEFINITIONSTART]}");
            AddInheritance(c);

            sb.AppendLine($"\t{Keywords[Keyword.OPENBODY]}");
            

            sb.AppendLine($"\t{Keywords[Keyword.CLOSEBODY]}");*/
        }

        public void Visit(IElement element)
        {
            switch (element)
            {
                case Class c:
                    {
                        c.Includes.ForEach(i => i.Accept(this));
                        sb.AppendLine($"");

                        AddNamespace(c);

                        sb.AppendLine($"{Definition.OpenDefinitonBodyTemplate}");

                        AddClassDefinition(c);


                        sb.AppendLine($"{Definition.CloseDefinitionBodyTemplate}");

                    }
                    break;
                case Include include:
                    {
                        sb.AppendLine(UseTemplate(include, Definition.IncludeTemplate));
                    }
                    break;
                case BaseClass baseClass:
                    {
                        //sb.Append($"{baseClass.Name}");
                    }
                    break;
                case Property property:
                    {
                        sb.Append(Tabs(TabNum));
                        sb.AppendLine(UseTemplate(property, Definition.PropertyDefinititonTemplate));

                        if (property.GenerateGetter)
                        {
                            sb.Append(Tabs(TabNum));
                            sb.AppendLine(UseTemplate(property, Definition.PropertyGetterTemplate));
                        }

                        if (property.GenerateSetter)
                        {
                            sb.Append(Tabs(TabNum));
                            sb.AppendLine(UseTemplate(property, Definition.PropertyGetterTemplate));
                        }


                        /*
                        sb.Append($"\t\t{AccessOperators[property.AccessOperator]} {property.DataType.Key} {property.Name}");
                        if (!string.IsNullOrEmpty(property.DefaultValue))
                        {
                            sb.Append($" = {property.DefaultValue}");
                        }
                        sb.AppendLine(";");

                        if (property.GenerateGetter)
                        {
                            sb.AppendLine($"\t\t{AccessOperators[property.AccessOperator]} {property.DataType.Key} {Keywords[Keyword.GETTERPREFIX]}{property.Name}()");
                            sb.AppendLine($"\t\t{Keywords[Keyword.OPENBODY]}");
                            sb.AppendLine($"\t\t\treturn {Keywords[Keyword.SELFCLASSPOINTER]}.{property.Name}{Keywords[Keyword.ENDOFCOMMAND]}");
                            sb.AppendLine($"\t\t{Keywords[Keyword.CLOSEBODY]}");
                        }

                        if (property.GenerateSetter)
                        {
                            sb.AppendLine($"\t\t{AccessOperators[property.AccessOperator]} void {Keywords[Keyword.SETTERPREFIX]}{property.Name}({property.DataType.Key} _{property.Name})");
                            sb.AppendLine($"\t\t{Keywords[Keyword.OPENBODY]}");
                            sb.AppendLine($"\t\t\t{Keywords[Keyword.SELFCLASSPOINTER]}.{property.Name} = _{property.Name}{Keywords[Keyword.ENDOFCOMMAND]}");
                            sb.AppendLine($"\t\t{Keywords[Keyword.CLOSEBODY]}");
                        }*/
                    }
                    break;
            }
        }

        private string UseTemplate(IMapped mapped, string template)
        {
            foreach ((var keyword, var value) in mapped.GetMapping())
            {
                template = template.Replace(keyword, value);
            }
            return template;
        }
    }
}
