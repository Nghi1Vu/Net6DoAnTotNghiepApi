using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class ModuleDkhp
    {
        public string HPModules { get; set; }
        public string ModulesName { get; set; }
        public int CreditsLT { get; set; }
        public int CreditsTH { get; set; }
        public Decimal Credits { get; set; }
    }
}
