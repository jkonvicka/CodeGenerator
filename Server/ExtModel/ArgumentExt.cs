namespace ExtModel
{
    public class ArgumentExt
    {
        public string Name { get; set; } = string.Empty;
        public DataTypeExt DataType { get; set; } = new DataTypeExt();
        public string DefaultValue { get; set; } = string.Empty;
    }
}