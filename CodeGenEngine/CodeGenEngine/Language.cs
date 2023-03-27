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
        public LanguageDeclaration Declaration { get; set; }

        private int TabNum { get; set; } = 0;

        static string Tabs(int n)
        {
            return new string('\t', n);
        }

        public Language(LanguageDeclaration declaration)
        {
            Declaration = declaration;
        }

        public string GetCode(Class @class)
        {
            @class.Accept(this);
            return sb.ToString();
        }

        public void AddNamespace(Class c)
        {
            if (!string.IsNullOrEmpty(Declaration.NamespaceTemplate))
            {
                AddDeclaration(UseTemplate(c, Declaration.NamespaceTemplate));
                //TabNum++;
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

        public void AddMetods(Class c)
        {
            c.Methods.ForEach(m => m.Accept(this));
        }

        public void AddConstructor(Class c)
        {
            //CONSTRUCTOR
            AddDeclaration(UseTemplate(c, Declaration.DefaultConstructorDeclarationTemplate));
            AddDeclaration($"{Declaration.OpenDefinitonBodyTemplate}");
            AddDeclaration($"{Declaration.OpenDefinitonBodyTemplate}");
            AddNewLine();

            List<string> arguments = new();
            List<string> propertyInitialization = new();
            foreach (Property property in c.Properties.OrderBy(x => x.DefaultValue))
            {
                arguments.Add(string.IsNullOrEmpty(property.DefaultValue) ?
                    UseTemplate(property, Declaration.ArgumentWithoutDefaultValueTemplate) :
                    UseTemplate(property, Declaration.ArgumentWithDefaultValueTemplate));
            }
            string parametrizedConstructorTemplate = MapArguments(arguments, Declaration.ParameterizedConstructorDeclarationTemplate);
            AddDeclaration(UseTemplate(c, parametrizedConstructorTemplate));
            AddDeclaration($"{Declaration.OpenDefinitonBodyTemplate}");

            AddDeclaration($"{Declaration.OpenDefinitonBodyTemplate}");
            AddNewLine();
        }

        public void AddClassDeclaration(Class c)
        {
            if (c.BaseClasses.Any())
            {
                AddDeclaration(UseTemplate(c, Declaration.ClassDeclarationWithBaseClassTemplate));
            }
            else
            {
                AddDeclaration(UseTemplate(c, Declaration.ClassDeclarationWithoutBaseClassTemplate));
            }
            AddDeclaration($"{Declaration.OpenDefinitonBodyTemplate}");

            TabNum++;
            AddProperties(c);
            AddConstructor(c);
            AddMetods(c);
            TabNum--;
            AddDeclaration($"{Declaration.CloseDeclarationBodyTemplate}");
        }



        public void Visit(IElement element)
        {
            switch (element)
            {
                case Class c:
                    {
                        c.Includes.ForEach(i => i.Accept(this));
                        AddDeclaration($"");
                        AddNamespace(c);
                        AddClassDeclaration(c);
                    }
                    break;
                case Include include:
                    {
                        AddDeclaration(UseTemplate(include, Declaration.IncludeTemplate));
                    }
                    break;
                case Method method:
                    {
                        List<string> arguments = new();
                        foreach (Argument argument in method.Arguments.OrderBy(x => x.DefaultValue))
                        {
                            arguments.Add(string.IsNullOrEmpty(argument.DefaultValue) ?
                                UseTemplate(argument, Declaration.ArgumentWithoutDefaultValueTemplate) :
                                UseTemplate(argument, Declaration.ArgumentWithDefaultValueTemplate));
                        }
                        string methodTemplate = MapArguments(arguments, Declaration.MethodDeclarationTemplate);
                        AddDeclaration(UseTemplate(method, methodTemplate));
                        AddDeclaration($"{Declaration.OpenDefinitonBodyTemplate}");

                        AddDeclaration($"{Declaration.OpenDefinitonBodyTemplate}");
                        AddNewLine();
                    }
                    break;
                case Property property:
                    {
                        AddDeclaration(UseTemplate(property, Declaration.PropertyDefinititonTemplate));

                        if (property.GenerateGetter)
                        {
                            AddDeclaration(UseTemplate(property, Declaration.PropertyGetterTemplate));
                        }

                        if (property.GenerateSetter)
                        {
                            AddDeclaration(UseTemplate(property, Declaration.PropertyGetterTemplate));
                        }
                        AddNewLine();
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

        private string MapArguments(List<string> arguments, string template)
        {
            return template.Replace("<ARGUMENTS>", string.Join(", ", arguments));
        }

        private void AddDeclaration(string declaration)
        {
            if (declaration != null)
            {
                sb.Append(Tabs(TabNum));
                sb.AppendLine(declaration);
            }
        }

        private void AddNewLine()
        {
            //sb.Append(Tabs(TabNum));
            sb.AppendLine("");
        }
    }
}
