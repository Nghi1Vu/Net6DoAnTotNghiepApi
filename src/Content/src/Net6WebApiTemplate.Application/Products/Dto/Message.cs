using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class Message
    {
        public string Content { get; set; }
        public int CommentID { get; set; }
        public int ParentID { get; set; }
        public int OwnerID { get; set; }
        public string fromuser { get; set; }
        public string touser { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
