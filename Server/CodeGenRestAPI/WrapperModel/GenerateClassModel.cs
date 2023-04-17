using ExtModel;

namespace CodeGenRestAPI.WrapperModel
{
    public class GenerateClassModel
    {
        public string Language { get; set; }
        public ClassExt ClassSpecification { get; set; }
    }
}
