using System.Xml.Linq;

namespace CodeGenEngine.Interface
{
    interface ILanguage
    {
        string GetCode(Class c);
        void AddNamespace(Class c);
        void AddInheritance(Class c);
        void AddGettersAndSetters(Class c);
        void AddConstructor(Class c);
        void AddClassDeclaration(Class c);
    }
}