using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class FamilyDetail
    {
        public string MotherName { get; set; }
        public string MotherAge { get; set; }
        public string MotherPhone1 { get; set; }
        public string MotherJob { get; set; }
        public string FatherName { get; set; }
        public string FatherAge { get; set; }
        public string FatherPhone1 { get; set; }
        public string FatherJob { get; set; }
        public string HomePhoneContact { get; set; }
        public string HomePhone { get; set; }
        public string IsAddressPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Siblings { get; set; }
    }
}
