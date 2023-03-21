namespace CodeGenEngine
{
    public interface IElement
    {
        public void Accept(IVisitor visitor);
    }
}