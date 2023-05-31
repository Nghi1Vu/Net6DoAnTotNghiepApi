using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class LogDKHP
    {
        public string register { get; set; }
        public string ModulesName { get; set; }
        public string ClassName { get; set; }
        public DateTime CreatedTime { get; set; }
        public int RegisterID { get; set; }
        public string SemesterName { get; set; }
        public string ClassCode { get; set; }
        public int OwnerID { get; set; }
        public int Del { get; set; }
    }
}
