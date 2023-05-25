using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace BookingApp.Ultility.Filters
{
    public class SwaggerDownloadFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            operation.responses["200"] = new Response
            {
                description = "Tải file",
                schema = new Schema { format = "byte", type = "file" }
            };
            // Thêm lỗi 400
            operation.responses.Add("400", new Response
            {
                description = "Lỗi tải file"
            });
            // Thêm kiểu dữ liệu trả về, mặc định application/pdf
            operation.produces.Insert(0, "application/pdf");
            operation.produces.Add("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            operation.produces.Add("text/html");
            operation.produces.Add("application/octet-stream");
        }
    }
}