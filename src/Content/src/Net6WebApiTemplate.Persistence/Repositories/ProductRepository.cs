using Dapper;
//using Net6WebApiTemplate.Application.Products.Interfaces;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Application.Shared.Interface;
using Net6WebApiTemplate.Domain.Entities;

namespace Net7studentportal.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public ProductRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        //public async Task<Product?> GetById(long id)
        //{
        //    var sql = @"SELECT ProductID, ProductName, ProductDescription, ProductPrice
        //                FROM Products p
        //                INNER JOIN ProductCategories pc ON p.CategoryID
        //                WHERE ProductID = @ProductId"
        //    ;

        //    using var sqlconnection = _connectionFactory.CreateConnection();
        //    var entity = await sqlconnection.QueryAsync<Product, ProductCatergory, Product>
        //        (sql, (product, productCategory) =>
        //        {
        //            product.ProductCatergory = productCategory;
        //            return product;
        //        },
        //        splitOn: "CategoryID");

        //    if (entity == null)
        //    {
        //        return null;
        //    }

        //    return entity.FirstOrDefault();
        //}
        public bool SignIn(string Username, string Password)
        {
            var objHash = new { Username, Password };
            string hash = objHash.GetHashCode().ToString();
            using var sqlconnection = _connectionFactory.CreateConnection();
            //var rowNum =sqlconnection.ExecuteAsync(
            //    @"update vnk_User
            //     set Email='110985026@dntu.edu.vn'
            //     where Username=@Username",
            //    new { Username }).Result;
            var obj = sqlconnection.Query<User>(@"select * from vnk_User
                 WHERE Password = @Password",
                new { @Password = hash });
            if (obj != null && obj.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool LoginWithEmail(string email)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            var obj = sqlconnection.Query<User>(@"select * from vnk_User
                 WHERE email = @email",
                new { email = email });
            if (obj != null && obj.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public List<News> GetNews()
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<News> obj = sqlconnection.Query<News>(@"select * from vnk_News where channelID=40 and statusID=1",
                new { }).ToList();
            if (obj != null && obj.Count() > 0)
            {
                return obj;
            }
            else
            {
                return new List<News>();
            }

        }
        public News GetNewsDetail(int NewsId)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            News obj = sqlconnection.Query<News>(@"select * from vnk_News where channelID=40 and statusID=1 and NewsID=@NewsId",
                new { @NewsId = NewsId }).FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new News();
            }

        }
        public List<StudenClass> GetStudentClass()
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<StudenClass> obj = sqlconnection.Query<StudenClass>(@"select Usercode, (Lastname+' '+Firstname) Username, Address, Phone,Email from ClassUser cr join vnk_User vr on cr.UserID=vr.UserID and cr.ClassID=803 order by Firstname",
                new { }).ToList();
            if (obj != null && obj.Count() > 0)
            {
                return obj;
            }
            else
            {
                return new List<StudenClass>();
            }

        }
        public StudentInfo GetStudentInfo(string Username, string Password, string email)
        {
            string hash = "";
            if (Username != "" && Password != "")
            {
                var objHash = new { Username, Password };
                hash = objHash.GetHashCode().ToString();
            }
            using var sqlconnection = _connectionFactory.CreateConnection();
                StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select (Lastname+' '+Firstname) Fullname, Usercode,
(select (Lastname+' '+Firstname) Fullname from vnk_User where UserId=(select UserId from [ClassTeacher] where ClassID = (select ClassID from Class where ClassID=(select ClassID from ClassUser where UserID=vr.UserID)))) TeacherName,
(select ClassName from Class where ClassID= (select ClassID from ClassUser where UserID=vr.UserID)) Classname,
(select IndustryName FROM Industry where IndustryID= (select IndustryID from [CourseIndustry] where CourseIndustryID= (select CourseIndustryID from Class where ClassID=(select ClassID from ClassUser where UserID=vr.UserID)))) as IndustryName,
(select DepartmentName from vnk_Department where DepartmentId= (select DepartmentId FROM Industry where IndustryID= (select IndustryID from [CourseIndustry] where CourseIndustryID= (select CourseIndustryID from Class where ClassID=(select ClassID from ClassUser where UserID=vr.UserID))))) as DepartmentName,
(select TBCTL from UserMark where UserID=vr.UserId) as TBCTL,
Email, Phone, Address, Images
from vnk_User vr where (Username=@Username and Password=@Password) or email=@email",
                new { @Username = Username, @Password = hash, @email = email }).FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new StudentInfo();
            }

        }
        public StudentInfo GetStudentDetail(int UserId)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select vi.School, vi.BirthPlace, vi.CMND, iif(vi.Gender=1,'Nữ','Nam') as Gender, vo.ReligionName,
vt.DistrictName, va.DistrictSocialName, ve.ProvinceName, vn.NationName, vh.EthnicName, vi.NumberOfHousing,
vm.HomeComponentName, vi.DOB, vj.ObjectName, vi.BHYT, vs.ExemptionsName
from vnk_User vr join vnk_UserDetail vi on vr.UserID=vi.UserID and vr.UserID=@UserId 
left join vnk_Religion vo on vo.ReligionID=vi.ReligionID 
left join vnk_District vt on vt.DistrictID= vi.DistrictID 
left join vnk_DistrictSocial va on va.DistrictSocialID= vi.DistrictSocialID 
left join vnk_Province ve on ve.ProvinceID = vi.ProvinceID 
left join vnk_Nation vn on vn.NationID= vi.NationID 
left join vnk_Ethnic vh on vh.EthnicID= vi.EthnicID  
left join vnk_HomeComponent vm on vm.HomeComponentID= vi.HomeComponentID
left join vnk_Object vj on vj.ObjectID= vi.Object
left join vnk_ExemptionsUser vx on vx.UserID=vr.UserID
left join vnk_Exemptions vs on vs.ExemptionsID= vx.ExemptionsID",
                new { UserId= UserId }).FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new StudentInfo();
            }

        }
        public StudentInfo GetFamilyDetail(int UserId)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select MotherName, MotherAge, MotherPhone1, MotherJob,
FatherName, FatherAge, FatherPhone1, FatherJob, HomePhoneContact,
HomePhone, IsAddressPhone, ContactEmail, Siblings
from vnk_UserDetail where UserId=@UserId",
                new { UserId = UserId }).FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new StudentInfo();
            }

        }
        public StudentInfo GetRLSemester(int UserId)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select SemesterID, SumScoreStudent, SumScoreTeacher, StatusID from RLUserSemester where UserID=@UserId   
order by SemesterID ASC
",
                new { UserId = UserId }).FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new StudentInfo();
            }

        }
    }
}