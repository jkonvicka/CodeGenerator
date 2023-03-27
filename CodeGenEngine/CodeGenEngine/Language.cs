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
        public LanguageDeclaration Declaration { get; set; }

        private Dictionary<string, string> ClassDeclaration { get; set; } = new Dictionary<string, string>()
        {
            {"INCLUDES_DECLARATION", string.Empty },
            {"NAMESPACE_DECLARATION", string.Empty },
            {"CLASS_DECLARATION", string.Empty },
            {"PRIVATE_PROPERTIES_DECLARATION", string.Empty },
            {"PRIVATE_METHODS_DECLARATION", string.Empty },
            {"PUBLIC_PROPERTIES_DECLARATION", string.Empty },
            {"CONSTRUCTORS_DECLARATION", string.Empty },
            {"GETTERS_AND_SETTERS_DECLARATION", string.Empty },
            {"PUBLIC_METHODS_DECLARATION", string.Empty },
        };

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
            string template = Declaration.ClassTemplate;
            foreach ((var keyword, var value) in ClassDeclaration)
            {
                template = template.Replace(keyword, value);
            }
            return template;
        }

        public void AddNamespace(Class c)
        {
            if (!string.IsNullOrEmpty(Declaration.NamespaceTemplate))
            {
                AddDeclaration("NAMESPACE_DECLARATION", UseTemplate(c, Declaration.NamespaceTemplate));
            }
        }

        public void AddInheritance(Class c)
        {
            /*if (c.BaseClasses.Count() >= 1)
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
            }*/
        }

        public void AddGettersAndSetters(Class c)
        {
            StringBuilder methodsSb = new StringBuilder();
            foreach (var property in c.Properties)
            {
                if (property.GenerateGetter)
                {
                    AddLine(methodsSb, UseTemplate(property, Declaration.PropertyGetterTemplate));
                }
                AddNewLine(methodsSb);
                if (property.GenerateSetter)
                {
                    AddLine(methodsSb, UseTemplate(property, Declaration.PropertySetterTemplate));
                }
                AddNewLine(methodsSb);
                AddNewLine(methodsSb);
            }
            AddDeclaration("GETTERS_AND_SETTERS_DECLARATION", methodsSb.ToString());
            c.Methods.ForEach(m => m.Accept(this));
        }

        public void AddConstructor(Class c)
        {
            StringBuilder constructorSb = new StringBuilder();
            //CONSTRUCTOR
            AddLine(constructorSb, UseTemplate(c, Declaration.DefaultConstructorDeclarationTemplate));
            AddNewLine(constructorSb);
            AddLine(constructorSb, $"{Declaration.OpenDefinitonBodyTemplate}");
            AddNewLine(constructorSb);
            AddLine(constructorSb, $"{Declaration.CloseDefinitonBodyTemplate}");
            AddNewLine(constructorSb);

            List<string> arguments = new();
            List<string> propertyInitialization = new();
            foreach (Property property in c.Properties.OrderBy(x => x.DefaultValue))
            {
                arguments.Add(string.IsNullOrEmpty(property.DefaultValue) ?
                    UseTemplate(property, Declaration.ArgumentWithoutDefaultValueTemplate) :
                    UseTemplate(property, Declaration.ArgumentWithDefaultValueTemplate));
            }
            string parametrizedConstructorTemplate = MapArguments(arguments, Declaration.ParameterizedConstructorDeclarationTemplate);
            AddLine(constructorSb, UseTemplate(c, parametrizedConstructorTemplate));
            AddNewLine(constructorSb);
            AddLine(constructorSb, $"{Declaration.OpenDefinitonBodyTemplate}");
            AddNewLine(constructorSb);
            AddLine(constructorSb, $"{Declaration.CloseDefinitonBodyTemplate}");
            AddNewLine(constructorSb);
            AddDeclaration("CONSTRUCTORS_DECLARATION", constructorSb.ToString());
        }

        public void AddClassDeclaration(Class c)
        {
            StringBuilder classSb = new StringBuilder();
            if (c.BaseClasses.Any())
            {
                AddLine(classSb, UseTemplate(c, Declaration.ClassDeclarationWithBaseClassTemplate));
            }
            else
            {
                AddLine(classSb, UseTemplate(c, Declaration.ClassDeclarationWithoutBaseClassTemplate));
            }
            AddDeclaration("CLASS_DECLARATION", classSb.ToString());
        }



        public void Visit(IElement element)
        {
            switch (element)
            {
                case Class c:
                    {
                        c.Includes.ForEach(i => i.Accept(this));
                        AddNamespace(c);
                        AddClassDeclaration(c);
                        TabNum++;
                        c.Properties.ForEach(p => p.Accept(this));
                        AddConstructor(c);
                        AddGettersAndSetters(c);
                    }
                    break;
                case Include include:
                    {
                        AddIncludeDeclaration(include);
                    }
                    break;
                case Method method:
                    {
                        AddMethodDeclaration(method);
                    }
                    break;
                case Property property:
                    {
                        AddPropertyDeclaration(property);
                    }
                    break;
            }
        }

        public void AddPropertyDeclaration(Property property)
        {
            StringBuilder sb = new StringBuilder();
            AddLine(sb, UseTemplate(property, Declaration.PropertyDefinititonTemplate));
            AddNewLine(sb);
            switch (property.AccessOperator)
            {
                case AccessOperator.PUBLIC:
                    AddDeclaration("PUBLIC_PROPERTIES_DECLARATION", sb.ToString());
                    break;
                case AccessOperator.PRIVATE:
                    AddDeclaration("PRIVATE_PROPERTIES_DECLARATION", sb.ToString());
                    break;
                case AccessOperator.PROTECTED:
                    AddDeclaration("PROTECTED_PROPERTIES_DECLARATION", sb.ToString());
                    break;
            }
        }

        public void AddMethodDeclaration(Method method)
        {
            StringBuilder sb = new StringBuilder();
            List<string> arguments = new();
            foreach (Argument argument in method.Arguments.OrderBy(x => x.DefaultValue))
            {
                arguments.Add(string.IsNullOrEmpty(argument.DefaultValue) ?
                    UseTemplate(argument, Declaration.ArgumentWithoutDefaultValueTemplate) :
                    UseTemplate(argument, Declaration.ArgumentWithDefaultValueTemplate));
            }
            string methodTemplate = MapArguments(arguments, Declaration.MethodDeclarationTemplate);
            AddLine(sb, UseTemplate(method, methodTemplate));
            AddNewLine(sb);
            AddLine(sb, $"{Declaration.OpenDefinitonBodyTemplate}");
            AddNewLine(sb);
            AddLine(sb, $"{Declaration.CloseDefinitonBodyTemplate}");
            AddNewLine(sb);
            switch (method.AccessOperator)
            {
                case AccessOperator.PUBLIC:
                    AddDeclaration("PUBLIC_METHODS_DECLARATION", sb.ToString());
                    break;
                case AccessOperator.PRIVATE:
                    AddDeclaration("PRIVATE_METHODS_DECLARATION", sb.ToString());
                    break;
                case AccessOperator.PROTECTED:
                    AddDeclaration("PROTECTED_METHODS_DECLARATION", sb.ToString());
                    break;
            }

        }

        public void AddIncludeDeclaration(Include include)
        {
            StringBuilder sb = new StringBuilder();
            AddLine(sb, UseTemplate(include, Declaration.IncludeTemplate));
            AddNewLine(sb);
            AddDeclaration("INCLUDES_DECLARATION", sb.ToString());
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

        private void AddDeclaration(string keyword, string declaration)
        {
            if (declaration != null)
            {
                //sb.Replace(keyword, declaration);
                if (ClassDeclaration.TryGetValue(keyword, out string value))
                {
                    ClassDeclaration[keyword] += (declaration);
                }

            }
        }

        private void AddLine(StringBuilder sb, string declaration, bool useTab = true)
        {
            if (declaration != null)
            {
                if (useTab)
                {
                    sb.Append(Tabs(TabNum));

                }
                sb.Append(declaration);
            }
        }

        private void AddNewLine(StringBuilder sb)
        {
            //sb.Append(Tabs(TabNum));
            sb.AppendLine("");
        }
    }
}
