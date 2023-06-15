using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class TTCN
    {
        public string ClassCode { get; set; }
        public string ModulesName { get; set; }
        public string ClassName { get; set; }
        public decimal Credits { get; set; }
        public long Costs { get; set; }
        public int? Status { get; set; }
        public int id { get; set; }
    }
}
