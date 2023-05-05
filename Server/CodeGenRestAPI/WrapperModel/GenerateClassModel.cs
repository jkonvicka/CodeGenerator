using ExtModel;

namespace CodeGenRestAPI.WrapperModel
{
    public class GenerateClassModel
    {
        public string Language { get; set; } = string.Empty;
        public ClassExt ClassSpecification { get; set; } = new ClassExt();
    }
}
