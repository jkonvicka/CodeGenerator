namespace ExtModel
{
    public class MethodExt
    {
        public string Name { get; set; }
        public DataTypeExt ReturnType { get; set; }
        public AccessOperatorExt AccessOperator { get; set; }
        public List<ArgumentExt> Arguments { get; set; }
    }
}