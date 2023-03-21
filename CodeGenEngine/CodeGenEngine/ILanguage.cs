using System.Xml.Linq;

namespace CodeGenEngine
{
    interface ILanguage
    {
        Dictionary<Keyword, string> Keywords { get; set; }
        Dictionary<AccessOperator, string> AccessOperators { get; set; }

        string GetCode(Class @class, string nameSpace);
        void AddNamespace();
        void AddInheritance(Class c);
        void AddProperties(Class c);
        void AddConstructor(Class c);
        void AddClassDefinition(Class c);
    }
}