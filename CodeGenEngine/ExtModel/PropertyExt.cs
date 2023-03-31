namespace ExtModel
{
    public class PropertyExt
    {
        public string Name { get; set; }
        public DataTypeExt DataType { get; set; }
        public string DefaultValue { get; set; }
        public AccessOperatorExt AccessOperator { get; set; }
        public bool GenerateGetter { get; set; }
        public bool GenerateSetter { get; set; }
    }
}