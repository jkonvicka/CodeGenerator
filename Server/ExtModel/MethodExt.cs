namespace ExtModel
{
    public class MethodExt
    {
        public string Name { get; set; } = string.Empty;
        public DataTypeExt ReturnType { get; set; } = new DataTypeExt();
        public AccessOperatorExt AccessOperator { get; set; }
        public List<ArgumentExt> Arguments { get; set; } = new List<ArgumentExt>();
    }
}