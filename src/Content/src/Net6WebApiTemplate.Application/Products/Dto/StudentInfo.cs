using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class StudentInfo
    {
        public string Fullname { get; set; }
        public string Usercode { get; set; }
        public string? Classname { get; set; }
    }
}
