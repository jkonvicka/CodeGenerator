namespace CodeGenEngine
{
    public class Include : IElement
    {
        public string Name { get; set; }

        public Include(string name)
        {
            Name = name;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}