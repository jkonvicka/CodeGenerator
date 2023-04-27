using System.Xml.Linq;

namespace CodeGenEngine.Interface
{
    public interface IVisitor
    {
        void Visit(IElement element);
    }
}