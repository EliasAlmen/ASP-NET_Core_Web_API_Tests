using WebApi.Enums;

namespace WebApi.Models
{
    public class ServiceResponse<T>
    {
        public StatusCode StatusCode { get; set; }
        public T? Content { get; set; }
    }
}
