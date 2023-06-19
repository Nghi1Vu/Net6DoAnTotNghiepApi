using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class KQHT
    {
        public int IndependentClassID { get; set; }
        public int UserID { get; set; }
        public int ModulesID { get; set; }
        public string Usercode { get; set; }
        public string Class { get; set; }
        public string Fullname { get; set; }
        public string ModulesName { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public int Score { get; set; }
        public int ScoreType { get; set; }
        public decimal Credits { get; set; }
      
    }
}
