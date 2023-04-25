using Net6WebApiTemplate.Domain.Entities;

namespace Net6WebApiTemplate.Application.Products.Dto
{
    public class RLSemester
    {
        public int SemesterID { get; set; }
        public int SumScoreStudent { get; set; }
        public int SumScoreTeacher { get; set; }
        public int StatusID { get; set; }
      
    }
}
