namespace CodeGenEngine.Interface
{
    public interface IElement
    {
        public void Accept(IVisitor visitor);
    }
}