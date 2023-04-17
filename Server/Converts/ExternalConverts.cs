using CodeGenEngine;
using ExtModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Converts
{
    public static class ExternalConverts
    {
        public static AccessOperator ConvertToInternal(this AccessOperatorExt ext)
        {
            switch (ext)
            {
                case AccessOperatorExt.PRIVATE:
                    return AccessOperator.PRIVATE;
                case AccessOperatorExt.PUBLIC:
                    return AccessOperator.PUBLIC;
                case AccessOperatorExt.PROTECTED:
                    return AccessOperator.PROTECTED;
                default:
                    throw new ArgumentException($"Invalid AccessOperatorExt value: {ext}", nameof(ext));
            }
        }
        public static Include ConvertToInternal(this IncludeExt ext)
        {
            Include convert = new Include(ext.Name);
            return convert;
        }
        public static BaseClass ConvertToInternal(this BaseClassExt ext)
        {
            BaseClass convert = new BaseClass(ext.Name);
            return convert;
        }
        public static DataType ConvertToInternal(this DataTypeExt ext)
        {
            DataType convert = new DataType(ext.Key);
            return convert;
        }
        public static Argument ConvertToInternal(this ArgumentExt ext)
        {
            Argument convert = new Argument(ext.Name, ext.DataType.ConvertToInternal(), ext.DefaultValue);
            return convert;
        }
        public static Method ConvertToInternal(this MethodExt ext)
        {
            Method convert = new Method(
                ext.AccessOperator.ConvertToInternal(),
                ext.Name,
                ext.ReturnType.ConvertToInternal(),
                ext.Arguments.Select(a => a.ConvertToInternal()).ToList());
            return convert;
        }
        public static Property ConvertToInternal(this PropertyExt ext)
        {
            Property convert = new Property(
                ext.AccessOperator.ConvertToInternal(),
                ext.GenerateGetter,
                ext.GenerateGetter,
                ext.Name,
                ext.DataType.ConvertToInternal(),
                ext.DefaultValue);
            return convert;
        }
        public static Class ConvertToInternal(this ClassExt ext)
        {
            Class convert = new Class()
            {
                Name = ext.Name,
                NameSpace = ext.NameSpace,
                AccessOperator = ext.AccessOperator.ConvertToInternal(),
                BaseClasses = ext.BaseClasses.Select(convert => convert.ConvertToInternal()).ToList(),
                Includes = ext.Includes.Select(convert => convert.ConvertToInternal()).ToList(),
                Methods = ext.Methods.Select(convert => convert.ConvertToInternal()).ToList(),
                Properties = ext.Properties.Select(convert => convert.ConvertToInternal()).ToList(),
            };
            return convert;
        }
    }
}