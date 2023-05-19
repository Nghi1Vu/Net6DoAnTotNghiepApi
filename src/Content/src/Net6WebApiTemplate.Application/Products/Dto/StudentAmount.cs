using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class StudentAmount
    {
        public string ChannelAmountName { get; set; }
        public long Amount { get; set; }
        public long TotalAmount { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Quantity { get; set; }
        public int? StatusID { get; set; }
        public int Day { get; set; }
    }
}
