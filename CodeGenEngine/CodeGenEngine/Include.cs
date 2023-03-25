using CodeGenEngine.Interface;

namespace CodeGenEngine
{
    public class Include : IElement, IMapped
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
        public Dictionary<string, string> GetMapping()
        {
            return new Dictionary<string, string>()
            {
                { "INCLUDE", this.Name }
            };
        }
    }
}