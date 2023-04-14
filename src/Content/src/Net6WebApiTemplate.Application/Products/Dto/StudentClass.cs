using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class StudenClass
    {
        public string Username { get; set; }
        public string Usercode { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }

    }
}
