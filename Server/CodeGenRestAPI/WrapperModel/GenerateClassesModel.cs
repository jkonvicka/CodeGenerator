using ExtModel;

namespace CodeGenRestAPI.WrapperModel
{
    public class GenerateClassesModel
    {
        public string Language { get; set; } = string.Empty;
        public ClassExt[] ClassSpecification { get; set; } = new ClassExt[0];
    }
}
