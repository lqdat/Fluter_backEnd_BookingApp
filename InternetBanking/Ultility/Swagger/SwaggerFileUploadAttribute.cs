using System;

namespace CSDLGiaBinhDinh.WebAPI.Ultility.Swagger
{
    /// <summary>
    /// Mô tả thuộc tính trong model class:
    /// <para>DisplayAttribute - Tên trường trong request body</para>
    /// <para>DescriptionAtrribute - Mô tả trong swagger</para>
    /// <para>virtual - Không hiển thị trong request body</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SwaggerFileUploadAttribute : Attribute
    {
        public SwaggerFileUploadAttribute(Type modelType)
        {
            ModelType = modelType;
        }

        public Type ModelType { get; set; }
    }
}
