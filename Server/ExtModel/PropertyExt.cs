namespace ExtModel
{
    public class PropertyExt
    {
        public string Name { get; set; } = string.Empty;
        public DataTypeExt DataType { get; set; } = new DataTypeExt();
        public string DefaultValue { get; set; } = string.Empty;
        public AccessOperatorExt AccessOperator { get; set; }
        public bool GenerateGetter { get; set; } = true;
        public bool GenerateSetter { get; set; } = true;
    }
}