using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class TradeHistory
    {
        public DateTime CreatedTime { get; set; }
        public string Status { get; set; }
        public string Costs { get; set; }
        public string Description { get; set; }
    }
}
