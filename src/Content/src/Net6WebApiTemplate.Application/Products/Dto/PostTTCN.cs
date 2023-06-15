using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class PostTTCN
    {
        public int UserId { get; set; }
        public string ttcnid { get; set; }
        public decimal amount { get; set; }
      
    }
}
