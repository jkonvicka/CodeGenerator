using ExtModel;

namespace CodeGenRestAPI.WrapperModel
{
    public class GenerateClassesModel
    {
        public string Language { get; set; }
        public ClassExt[] ClassSpecification { get; set; }
    }
}
