using System.Xml.Linq;

namespace CodeGenEngine
{
    public interface IVisitor
    {
        void Visit(IElement element);
    }
}