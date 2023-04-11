using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class News
    {
        public int NewsId { get; set; }
        public int? ChannelID { get; set; }
        public string? Title { get; set; }
        public string? subTitle { get; set; }
        public string? Head { get; set; }
        public string? Content { get; set; }
        public string? ContentSearch { get; set; }
        public string? StatusID { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
