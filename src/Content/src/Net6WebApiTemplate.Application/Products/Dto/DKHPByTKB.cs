using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class DKHPByTKB
    {

        public string room { get; set; }
        public string campus { get; set; }
        public int IndependentClassID { get; set; }
        public string ClassName { get; set; }
        public string timeday { get; set; }
        public string ModulesName { get; set; }
        public int TimesInDay { get; set; }
        public int DayStudy { get; set; }
    }
}
