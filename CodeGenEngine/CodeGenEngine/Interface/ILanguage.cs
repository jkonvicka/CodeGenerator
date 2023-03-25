using System.Xml.Linq;

namespace CodeGenEngine.Interface
{
    interface ILanguage
    {
        string GetCode(Class c);
        void AddNamespace(Class c);
        void AddInheritance(Class c);
        void AddProperties(Class c);
        void AddConstructor(Class c);
        void AddClassDefinition(Class c);
    }
}