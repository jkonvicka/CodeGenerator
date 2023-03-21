namespace CodeGenEngine
{
    public class BaseClass : IElement
    {
        public string Name { get; set; }
        public BaseClass(string name)
        {
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}