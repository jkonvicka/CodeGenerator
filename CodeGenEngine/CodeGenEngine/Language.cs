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
        private Dictionary<string, string> ClassDeclaration { get; set; } = new Dictionary<string, string>()
        {
            {"INCLUDES_DECLARATION", string.Empty },
            {"NAMESPACE_DECLARATION", string.Empty },
            {"CLASS_DECLARATION", string.Empty },
            {"PRIVATE_PROPERTIES_DECLARATION", string.Empty },
            {"PRIVATE_METHODS_DECLARATION", string.Empty },
            {"PUBLIC_PROPERTIES_DECLARATION", string.Empty },
            {"DEFAULT_CONSTRUCTOR_DECLARATION", string.Empty },
            {"PARAMETRIZED_CONSTRUCTOR_DECLARATION", string.Empty },
            {"GETTERS_AND_SETTERS_DECLARATION", string.Empty },
            {"PUBLIC_METHODS_DECLARATION", string.Empty },
        };

        public LanguageDeclaration Declaration { get; set; }

        private int TabNum { get; set; } = 0;

        public Language(LanguageDeclaration declaration)
        {
            Declaration = declaration;
        }

        #region Public Methods

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

        #region IVisitor Methods
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
        #endregion

        public void AddNamespace(Class c)
        {
            if (!string.IsNullOrEmpty(Declaration.NamespaceTemplate))
            {
                AddDeclaration("NAMESPACE_DECLARATION", UseTemplate(c, Declaration.NamespaceTemplate));
            }
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

        public void AddConstructor(Class c)
        {
            StringBuilder constructorSb = new StringBuilder();

            // Default constructor
            if (!string.IsNullOrEmpty(Declaration.DefaultConstructorDeclarationTemplate))
            {
                AddLine(constructorSb, UseTemplate(c, Declaration.DefaultConstructorDeclarationTemplate));
                AddNewLine(constructorSb);
                AddLine(constructorSb, $"{Declaration.OpenDefinitonBodyTemplate}");
                AddNewLine(constructorSb);
                AddLine(constructorSb, $"{Declaration.CloseDefinitonBodyTemplate}");
                AddNewLine(constructorSb);
                AddDeclaration("DEFAULT_CONSTRUCTOR_DECLARATION", constructorSb.ToString());
            }

            if (!string.IsNullOrEmpty(Declaration.ParameterizedConstructorDeclarationTemplate))
            {
                List<string> arguments = new();
                List<string> propertyInitialization = new();
                foreach (Property property in c.Properties.OrderBy(x => x.DefaultValue))
                {
                    arguments.Add(string.IsNullOrEmpty(property.DefaultValue) ?
                        UseTemplate(property, Declaration.ArgumentWithoutDefaultValueTemplate) :
                        UseTemplate(property, Declaration.ArgumentWithDefaultValueTemplate));
                    propertyInitialization.Add($"{UseTemplate(property, Declaration.PropertyInitializationTemplate)}");
                }

                constructorSb = new StringBuilder();
                // Parametrized constructor head definition
                string parametrizedConstructorTemplate = MapArguments(arguments, Declaration.ParameterizedConstructorDeclarationTemplate);
                AddLine(constructorSb, UseTemplate(c, parametrizedConstructorTemplate));
                if (!string.IsNullOrEmpty(Declaration.OpenDefinitonBodyTemplate))
                    AddNewLine(constructorSb);
                AddLine(constructorSb, $"{Declaration.OpenDefinitonBodyTemplate}");
                AddNewLine(constructorSb);

                // Property initialization
                TabNum++;
                foreach (string propertyInitializationLine in propertyInitialization)
                {
                    AddLine(constructorSb, propertyInitializationLine);
                    AddNewLine(constructorSb);
                }
                TabNum--;
                AddLine(constructorSb, $"{Declaration.CloseDefinitonBodyTemplate}");
                AddNewLine(constructorSb);
                AddDeclaration("PARAMETRIZED_CONSTRUCTOR_DECLARATION", constructorSb.ToString());
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
            string methodTemplate = string.Empty;
            if (method.AccessOperator == AccessOperator.PRIVATE)
            {
                methodTemplate = MapArguments(arguments, Declaration.PrivateMethodDeclarationTemplate);
            }
            else if (method.AccessOperator == AccessOperator.PUBLIC)
            {
                methodTemplate = MapArguments(arguments, Declaration.PublicMethodDeclarationTemplate);
            }
            else
            {
                throw new NotSupportedException($"Not supported method ACCESS OPERATOR {method.AccessOperator}");
            }
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
        #endregion

        #region Private Methods
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
                if (ClassDeclaration.TryGetValue(keyword, out string value))
                {
                    ClassDeclaration[keyword] += (declaration);
                }
            }
        }

        private void AddLine(StringBuilder sb, string declaration, bool useTab = true)
        {
            if (string.IsNullOrEmpty(declaration))
                return;
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
            sb.AppendLine("");
        }
        #endregion

        #region Private Static Methods
        //todo move to utils?
        static string Tabs(int n)
        {
            return new string('\t', n);
        }
        #endregion
    }
}
