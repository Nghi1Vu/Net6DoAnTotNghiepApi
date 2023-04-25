using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Common.Interfaces
{
    public interface IProductRepository
    {
        //Task<Product?> GetById(long id);
        bool SignIn(string Username, string Password);
        bool LoginWithEmail(string email);
        List<News> GetNews();
        List<StudenClass> GetStudentClass();
        News GetNewsDetail(int NewsId);
        StudentInfo GetStudentInfo(string Username, string Password);
        StudentInfo GetStudentInfoByEmail(string email);
        StudentDetail GetStudentDetail(int UserId);
        FamilyDetail GetFamilyDetail(int UserId);
        RLSemester GetRLSemester(int UserId);
        RLForm GetRLForm(int UserId);
    }
}