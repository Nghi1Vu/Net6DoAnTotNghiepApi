using Net6WebApiTemplate.Application.Products.Commands.CreateProduct;
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
        List<RLSemester> GetRLSemester(int UserId);
        List<RLForm> GetRLForm();
        int PostRLForm(PostRLForm model);
        List<ProgramSemester> GetProgramSemester();
        List<ModuleDetail> GetModuleDetail(int ModulesID);
        List<IndependentClass> GetIC(int ModulesID);
        List<KQHT> GetKQHTByClass(int IndependentClassID);
        List<KQHT> GetKQHTByUser(int UserID);
        List<TradeHistory> GetTradeHistory(int UserID);
        List<DsGtHs> GetDsGtHs(int UserID);
        List<Certificate> GetCertificateByUser(int UserID);
        List<Message> GetMessage(int ClassID);
        List<ChannelAmount> GetChannelAmount(int ClassID);
        List<StudentAmount> GetStudentAmount(int UserID);
        List<TTCN> GetTTCNDone(int UserID);
        List<TTCN> GetTTCN(int UserID);
        List<ExamResult> GetExamResult(int UserID);
        List<ExamByClass> GetExamByClass(int IndependentClassID);
        List<ExamCalendar> GetExamCalendar(int UserID);
        List<TeachCalendar> GetTeachCalendar(int UserID);
        List<TeachCalendarDetail> GetTeachCalendarDetail(int IndependentClassID, int UserID);
        List<TBCHKModel> GetTBCHK(int UserID);
        List<TKB> GetTKB(int UserID, string aDate, string eDate);
        List<IndependentClass> GetICByTKB(int TimesInDay, int DayStudy);
        List<LogDKHP> GetLogDKHP(int UserID);
        int ChangePassword(string username, string oldpass, string newpass);
        string PostTTCN(int UserID, string ttcnid, decimal amount);
        string PostOneDoor(int UserID, string odid, decimal amount);
        string PostMessage(int UserID, string content);
    }
}