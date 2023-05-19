using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class ChannelAmount
    {
        public int Day { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int ChannelAmountID { get; set; }
        public int ChannelAmountName { get; set; }
        public long Costs { get; set; }
    }
}
