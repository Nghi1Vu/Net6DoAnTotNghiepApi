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
        public StudentInfo GetStudentInfo(string Username, string Password)
        {
            string hash = "";

            var objHash = new { Username, Password };
            hash = objHash.GetHashCode().ToString();

            using var sqlconnection = _connectionFactory.CreateConnection();
            StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select UserId,(Lastname+' '+Firstname) Fullname, Usercode,
(select (Lastname+' '+Firstname) Fullname from vnk_User where UserId=(select UserId from [ClassTeacher] where ClassID = (select ClassID from Class where ClassID=(select ClassID from ClassUser where UserID=vr.UserID)))) TeacherName,
(select ClassName from Class where ClassID= (select ClassID from ClassUser where UserID=vr.UserID)) Classname,
(select IndustryName FROM Industry where IndustryID= (select IndustryID from [CourseIndustry] where CourseIndustryID= (select CourseIndustryID from Class where ClassID=(select ClassID from ClassUser where UserID=vr.UserID)))) as IndustryName,
(select DepartmentName from vnk_Department where DepartmentId= (select DepartmentId FROM Industry where IndustryID= (select IndustryID from [CourseIndustry] where CourseIndustryID= (select CourseIndustryID from Class where ClassID=(select ClassID from ClassUser where UserID=vr.UserID))))) as DepartmentName,
(select TBCTL from UserMark where UserID=vr.UserId) as TBCTL,
Email, Phone, Address, Images
from vnk_User vr where Username=@Username and Password=@Password",
            new { @Username = Username, @Password = hash }).FirstOrDefault();
            if (obj != null)
            {
                StudentInfo a = new StudentInfo();
                return obj;
            }
            else
            {
                return new StudentInfo();
            }

        }
        public StudentInfo GetStudentInfoByEmail(string email)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select UserId, (Lastname+' '+Firstname) Fullname, Usercode,
(select (Lastname+' '+Firstname) Fullname from vnk_User where UserId=(select UserId from [ClassTeacher] where ClassID = (select ClassID from Class where ClassID=(select ClassID from ClassUser where UserID=vr.UserID)))) TeacherName,
(select ClassName from Class where ClassID= (select ClassID from ClassUser where UserID=vr.UserID)) Classname,
(select IndustryName FROM Industry where IndustryID= (select IndustryID from [CourseIndustry] where CourseIndustryID= (select CourseIndustryID from Class where ClassID=(select ClassID from ClassUser where UserID=vr.UserID)))) as IndustryName,
(select DepartmentName from vnk_Department where DepartmentId= (select DepartmentId FROM Industry where IndustryID= (select IndustryID from [CourseIndustry] where CourseIndustryID= (select CourseIndustryID from Class where ClassID=(select ClassID from ClassUser where UserID=vr.UserID))))) as DepartmentName,
(select TBCTL from UserMark where UserID=vr.UserId) as TBCTL,
Email, Phone, Address, Images
from vnk_User vr where email=@email",
            new { @email = email }).FirstOrDefault();
            if (obj != null)
            {
                StudentInfo a = new StudentInfo();
                return obj;
            }
            else
            {
                return new StudentInfo();
            }

        }
        public StudentDetail GetStudentDetail(int UserId)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            StudentDetail obj = sqlconnection.Query<StudentDetail>(@"select vi.School, vi.BirthPlace, vi.CMND, iif(vi.Gender=1,'Nữ','Nam') as Gender, vo.ReligionName,
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
                new { UserId = UserId }).FirstOrDefault();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new StudentDetail();
            }

        }
        public FamilyDetail GetFamilyDetail(int UserId)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            FamilyDetail obj = sqlconnection.Query<FamilyDetail>(@"select MotherName, MotherAge, MotherPhone1, MotherJob,
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
                return new FamilyDetail();
            }

        }
        public List<RLSemester> GetRLSemester(int UserId)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<RLSemester> obj = sqlconnection.Query<RLSemester>(@"select SemesterID, sum(Score) Score from RLUser where UserID=@UserId
group by SemesterID
",
                new { UserId = UserId }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<RLSemester>();
            }

        }
        public List<RLForm> GetRLForm()
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<RLForm> obj = sqlconnection.Query<RLForm>(@"select rn.RLQuestionID, rn.Title as BigTitle, rr.Title, rr.MaxScore, rr.RLAnswerID from RLQuestion rn join RLAnswer rr on rn.RLQuestionID= rr.RLQuestionID",
                new { }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<RLForm>();
            }

        }
        public int PostRLForm(PostRLForm model)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            sqlconnection.Open();
            var trans = sqlconnection.BeginTransaction();
            int obj = 0;
            int count = 0;
            foreach (var item in model.detail)
            {
                count = sqlconnection.Execute(@"if not exists(select * from RLUser where UserID=@UserId and SemesterID=@SemesterID and RLAnswerID=@RLAnswerID)
insert into RLUser ( UserID, RLAnswerID,Score,SemesterID) values(@UserID,@RLAnswerID,@Score,@SemesterID)
else
update RLUser set Score=@Score where UserID=@UserID and RLAnswerID=@RLAnswerID AND SemesterID=@SemesterID ",
           new { UserId = model.UserId, SemesterID = model.SemesterID, RLAnswerID = item.RLAnswerID, Score = item.Score }, trans);
                if (count > 0)
                {
                    obj++;
                }
            }
            if (obj == model.detail.Count)
            {
                trans.Commit();
                return 1;
            }
            else
            {
                trans.Rollback();
                return 0;
            }
        }
        public List<ProgramSemester> GetProgramSemester()
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ProgramSemester> obj = sqlconnection.Query<ProgramSemester>(@"select COUNT(*) CreatedClass, md.ModulesTypeID,pm.ProgramGroupID,md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName, (select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=0) as Credits0,(select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=1) as Credits1  from vnk_Modules md
join Program p on p.StructProgramID = md.StructProgramID and p.CourseIndustryID = 751
left join ProgramGroup pg on pg.ProgramID = p.ProgramID
join ProgramModules pm on(pm.ProgramGroupID= pg.ProgramGroupID or (pm.ProgramGroupID = 0)) and p.ProgramID = pm.ProgramID and md.ModulesID = pm.ModulesID and isnull(pm.Del, 0)= 0
left join IndependentClass ic on ic.ModulesID = md.ModulesID and pm.Semester = ic.Semester and ic.CourseID = 100
join Semester s on s.SemesterID=pm.Semester
where pm.Semester = 2
group by md.ModulesTypeID,pm.ProgramGroupID, md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName,pm.Semester,p.CourseIndustryID",
                new { }).ToList();
            //List<RLForm> obj = sqlconnection.Query<RLForm>(@"select ('HP'+ cast(md.ModulesID as varchar(10))) as HPModules, md.ModulesName,md.CreditsLT,md.CreditsLT, md.Credits,* from vnk_Modules md 
            //left join vnk_ModulesGroupJoin gj on md.ModulesID= gj.ModulesID
            //left join vnk_ModulesGroup g on g.ModulesGroupID = gj.ModulesGroupID
            //left join vnk_ModulesGroupIndustry gi on g.ModulesGroupID=gi.ModulesGroupID and g.DepartmentID=5 and gi.IndustryID=30
            //left join ModulesType mt on mt.TypeID= md.ModulesTypeID",
            //    new { }).ToList();

            //----------------------------------//

            //            select cid.CourseIndustryID, c.CourseName, i.IndustryName, cid.Credits, cis.Credits,s.SemesterName from vnk_Course c
            //join CourseIndustry ci on c.CourseID = ci.CourseID and c.CourseID = 100  and ci.IndustryID = 30
            //join CourseIndustryDetail cid on cid.CourseIndustryID = ci.CourseIndustryID
            //join Industry i on i.IndustryID = ci.IndustryID
            //join CourseIndustrySemester cis on cis.CourseIndustryID = ci.CourseIndustryID and cis.TypeID = 0
            //join Semester s on s.SemesterID = cis.SemesterID

            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<ProgramSemester>();
            }

        }
        public List<ModuleDetail> GetModuleDetail(int ModulesID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ModuleDetail> obj = sqlconnection.Query<ModuleDetail>(@"select d.DepartmentName,('HP'+CAST(m.ModulesID as varchar(10)))as HPModules,
ModulesName,ModulesNameSort,ModulesCode,md.Descriptions,m.Credits,
md.Purposely,md.PurposelyKN,md.PurposelyYT,ms.ChapterName,ms.ContentChapter,
ms.TimesST,ms.TimesLT,ms.TimesBT,ms.TimesTL,ms.TimesTH,ms.TimesK,ms.TimesTest,
md.[References],tl.TranningLevelName,m.NumberStPerClass,et.ExamTypeName, et.ExamRateTL,
et.ExamRateTN, et.ExamRateK from vnk_Modules m
join ModulesDetail md on md.ModulesID= m.ModulesID and m.ModulesID=39
join ModulesStruct ms on ms.ModulesID=m.ModulesID
join Department d on d.DepartmentID=m.DepartmentID
join TranningLevel tl on  tl.TranningLevelID=m.TranningLevelID
join vnk_ExamType et on et.ExamTypeID=m.ExamTypeID",
                new { }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<ModuleDetail>();
            }
        }
    }
}