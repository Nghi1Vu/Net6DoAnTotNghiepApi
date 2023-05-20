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
            StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select vr.UserId, (vr.Lastname + ' ' + vr.Firstname) Fullname, vr.Usercode,
			(select (Lastname + ' ' + Firstname) Fullname from vnk_User where UserID= ct.UserID) as TeacherName,
			c.ClassName,
c.ClassID,
			I.IndustryName,
i.IndustryCode,
tl.TranningLevelName,
ci.CourseIndustryID,
			d.DepartmentName,
			um.TBCTL,
            vr.Email, vr.Phone, vr.[Address], vr.Images, co.CourseName,co.CourseCode,cid.Credits
            from vnk_User vr
			join ClassUser cu on cu.UserID=vr.UserID and vr.Username = @Username and vr.Password = @Password
			left join Class c on c.ClassID= cu.ClassID
		 left	join ClassTeacher ct on ct.ClassID = cu.ClassID
           left join CourseIndustry ci on ci.CourseIndustryID = c.CourseIndustryID
			left join Industry i on i.IndustryID=ci.IndustryID
			left join Department d on d.DepartmentID=i.DepartmentID
			left join UserMark um on um.UserID=vr.UserID
		left	join vnk_Course co on co.CourseID= ci.CourseID
           left join CourseIndustryDetail cid on cid.CourseIndustryID = ci.CourseIndustryID
left join TranningLevel tl on tl.TranningLevelID=i.TranningLevelID",
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
            StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select vr.UserId, (vr.Lastname + ' ' + vr.Firstname) Fullname, vr.Usercode,
			(select (Lastname + ' ' + Firstname) Fullname from vnk_User where UserID= ct.UserID) as TeacherName,
			c.ClassName,
c.ClassID,
			I.IndustryName,
i.IndustryCode,
tl.TranningLevelName,
ci.CourseIndustryID,
			d.DepartmentName,
			um.TBCTL,
            vr.Email, vr.Phone, vr.[Address], vr.Images, co.CourseName,co.CourseCode,cid.Credits
            from vnk_User vr
			join ClassUser cu on cu.UserID=vr.UserID and vr.email = @email
			left join Class c on c.ClassID= cu.ClassID
		 left	join ClassTeacher ct on ct.ClassID = cu.ClassID
           left join CourseIndustry ci on ci.CourseIndustryID = c.CourseIndustryID
			left join Industry i on i.IndustryID=ci.IndustryID
			left join Department d on d.DepartmentID=i.DepartmentID
			left join UserMark um on um.UserID=vr.UserID
		left	join vnk_Course co on co.CourseID= ci.CourseID
           left join CourseIndustryDetail cid on cid.CourseIndustryID = ci.CourseIndustryID
left join TranningLevel tl on tl.TranningLevelID=i.TranningLevelID",
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
            List<ProgramSemester> obj = sqlconnection.Query<ProgramSemester>(@"select tbl.ModulesID,tbl.TimesK,tbl.SemesterID,tbl.MinCreditsLT,tbl.MinCreditsTH,tbl.MinCreditsK,tbl.GroupName,tbl.CreditsS,string_agg(tbl.ModulesTQ,', <br>')as ModulesTQ,string_agg(tbl.ModulesHT,', <br>') as ModulesHT, tbl.TimesBT,tbl.TypeName,tbl.CreatedClass, tbl.ModulesTypeID,tbl.ProgramGroupID,tbl.ModulesCode, tbl.ModulesName,tbl.CreditsLT,tbl.CreditsTH,tbl.CreditsK, tbl.Credits,TimesLT,TimesTH,TimesTL, tbl.SemesterName, tbl.Credits0,tbl.Credits1  from

(select md.ModulesID,md.TimesK,s.SemesterID,pg.MinCreditsLT,pg.MinCreditsTH,pg.MinCreditsK,pg.GroupName,cis.Credits as CreditsS,(select ModulesCode from vnk_Modules where ModulesID=mp.ModulesIDPremise) as ModulesTQ,(select ModulesCode from vnk_Modules where ModulesID=mb.ModulesIDBefore) as ModulesHT, md.TimesBT,mt.TypeName,COUNT(*) CreatedClass, md.ModulesTypeID,pm.ProgramGroupID,md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName, (select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=0) as Credits0,(select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=1) as Credits1  from vnk_Modules md
join Program p on p.StructProgramID = md.StructProgramID and p.CourseIndustryID = 751
left join ProgramGroup pg on pg.ProgramID = p.ProgramID
join ProgramModules pm on(pm.ProgramGroupID= pg.ProgramGroupID or (pm.ProgramGroupID = 0)) and p.ProgramID = pm.ProgramID and md.ModulesID = pm.ModulesID and isnull(pm.Del, 0)= 0
left join IndependentClass ic on ic.ModulesID = md.ModulesID and pm.Semester = ic.Semester and ic.CourseID = 100
join Semester s on s.SemesterID=pm.Semester
join ModulesType mt on mt.TypeID=md.ModulesTypeID
left join ModulesBefore mb on mb.ModulesID=md.ModulesID and isnull(mb.Del,0)=0
left join ModulesPremise mp on mp.ModulesID=md.ModulesID and isnull(mp.Del,0)=0
join CourseIndustrySemester cis on cis.CourseIndustryID = p.CourseIndustryID and cis.TypeID = 0 and cis.SemesterID=pm.Semester
group by md.ModulesID,md.TimesK,s.SemesterID,pg.MinCreditsLT,pg.MinCreditsTH,pg.MinCreditsK,pg.GroupName,cis.Credits,mp.ModulesIDPremise,mb.ModulesIDBefore,md.TimesBT,mt.TypeName, md.ModulesTypeID,pm.ProgramGroupID, md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName,pm.Semester,p.CourseIndustryID) as tbl
group by tbl.ModulesID,tbl.TimesK,tbl.SemesterID,tbl.MinCreditsLT,tbl.MinCreditsTH,tbl.MinCreditsK,tbl.GroupName,tbl.CreditsS, tbl.TimesBT,tbl.TypeName,tbl.CreatedClass, tbl.ModulesTypeID,tbl.ProgramGroupID,tbl.ModulesCode, tbl.ModulesName,tbl.CreditsLT,tbl.CreditsTH,tbl.CreditsK, tbl.Credits,TimesLT,TimesTH,TimesTL, tbl.SemesterName, tbl.Credits0,tbl.Credits1",
                new { }).ToList();
//            List<ProgramSemester> obj = sqlconnection.Query<ProgramSemester>(@"select md.TimesK,s.SemesterID,pg.MinCreditsLT,pg.MinCreditsTH,pg.MinCreditsK,pg.GroupName,cis.Credits as CreditsS,(select ModulesCode from vnk_Modules where ModulesID=mp.ModulesIDPremise) as ModulesTQ,(select ModulesCode from vnk_Modules where ModulesID=mb.ModulesIDBefore) as ModulesHT, md.TimesBT,mt.TypeName,COUNT(*) CreatedClass, md.ModulesTypeID,pm.ProgramGroupID,md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName, (select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=0) as Credits0,(select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=1) as Credits1  from vnk_Modules md
//join Program p on p.StructProgramID = md.StructProgramID and p.CourseIndustryID = 751
//left join ProgramGroup pg on pg.ProgramID = p.ProgramID
//join ProgramModules pm on(pm.ProgramGroupID= pg.ProgramGroupID or (pm.ProgramGroupID = 0)) and p.ProgramID = pm.ProgramID and md.ModulesID = pm.ModulesID and isnull(pm.Del, 0)= 0
//left join IndependentClass ic on ic.ModulesID = md.ModulesID and pm.Semester = ic.Semester and ic.CourseID = 100
//join Semester s on s.SemesterID=pm.Semester
//join ModulesType mt on mt.TypeID=md.ModulesTypeID
//left join ModulesBefore mb on mb.ModulesID=md.ModulesID and isnull(mb.Del,0)=0
//left join ModulesPremise mp on mp.ModulesID=md.ModulesID and isnull(mp.Del,0)=0
//join CourseIndustrySemester cis on cis.CourseIndustryID = p.CourseIndustryID and cis.TypeID = 0 and cis.SemesterID=pm.Semester
//group by md.TimesK,s.SemesterID,pg.MinCreditsLT,pg.MinCreditsTH,pg.MinCreditsK,pg.GroupName,cis.Credits,mp.ModulesIDPremise,mb.ModulesIDBefore,md.TimesBT,mt.TypeName, md.ModulesTypeID,pm.ProgramGroupID, md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName,pm.Semester,p.CourseIndustryID",
//                new { }).ToList();

            //            List<ProgramSemester> obj = sqlconnection.Query<ProgramSemester>(@"select COUNT(*) CreatedClass, md.ModulesTypeID,pm.ProgramGroupID,md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName, (select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=0) as Credits0,(select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=1) as Credits1  from vnk_Modules md
            //join Program p on p.StructProgramID = md.StructProgramID and p.CourseIndustryID = 751
            //left join ProgramGroup pg on pg.ProgramID = p.ProgramID
            //join ProgramModules pm on(pm.ProgramGroupID= pg.ProgramGroupID or (pm.ProgramGroupID = 0)) and p.ProgramID = pm.ProgramID and md.ModulesID = pm.ModulesID and isnull(pm.Del, 0)= 0
            //left join IndependentClass ic on ic.ModulesID = md.ModulesID and pm.Semester = ic.Semester and ic.CourseID = 100
            //join Semester s on s.SemesterID=pm.Semester
            //where pm.Semester = 2
            //group by md.ModulesTypeID,pm.ProgramGroupID, md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName,pm.Semester,p.CourseIndustryID",
            //                new { }).ToList();

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
            List<ModuleDetail> obj = sqlconnection.Query<ModuleDetail>(@"select mt.TypeName,d.DepartmentName,('HP'+CAST(m.ModulesID as varchar(10)))as HPModules,
ModulesName,ModulesNameSort,ModulesCode,md.Descriptions,m.Credits,
md.Purposely,md.PurposelyKN,md.PurposelyYT,ms.ChapterName,ms.ContentChapter,
ms.TimesST,ms.TimesLT,ms.TimesBT,ms.TimesTL,ms.TimesTH,ms.TimesK,ms.TimesTest,
md.[References],tl.TranningLevelName,m.NumberStPerClass,et.ExamTypeName, et.ExamRateTL,
et.ExamRateTN, et.ExamRateK from vnk_Modules m
join ModulesDetail md on md.ModulesID= m.ModulesID and m.ModulesID=@ModulesID
join ModulesStruct ms on ms.ModulesID=m.ModulesID
join Department d on d.DepartmentID=m.DepartmentID
join TranningLevel tl on  tl.TranningLevelID=m.TranningLevelID
join vnk_ExamType et on et.ExamTypeID=m.ExamTypeID
left join ModulesType mt on mt.TypeID= m.ModulesTypeID",
                new { @ModulesID = ModulesID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<ModuleDetail>();
            }
        }
        //        public List<ModuleDetail> GetProgram()
        //        {
        //            using var sqlconnection = _connectionFactory.CreateConnection();
        //            List<ModuleDetail> obj = sqlconnection.Query<ModuleDetail>(@"select (select ModulesCode from vnk_Modules where ModulesID=mp.ModulesIDPremise),(select ModulesCode from vnk_Modules where ModulesID=mb.ModulesIDBefore), md.TimesBT,mt.TypeName,COUNT(*) CreatedClass, md.ModulesTypeID,pm.ProgramGroupID,md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName, (select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=0) as Credits0,(select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=1) as Credits1  from vnk_Modules md
        //join Program p on p.StructProgramID = md.StructProgramID and p.CourseIndustryID = 751
        //left join ProgramGroup pg on pg.ProgramID = p.ProgramID
        //join ProgramModules pm on(pm.ProgramGroupID= pg.ProgramGroupID or (pm.ProgramGroupID = 0)) and p.ProgramID = pm.ProgramID and md.ModulesID = pm.ModulesID and isnull(pm.Del, 0)= 0
        //left join IndependentClass ic on ic.ModulesID = md.ModulesID and pm.Semester = ic.Semester and ic.CourseID = 100
        //join Semester s on s.SemesterID=pm.Semester
        //join ModulesType mt on mt.TypeID=md.ModulesTypeID
        //left join ModulesBefore mb on mb.ModulesID=md.ModulesID and isnull(mb.Del,0)=0
        //left join ModulesPremise mp on mp.ModulesID=md.ModulesID and isnull(mp.Del,0)=0
        //group by mp.ModulesIDPremise,mb.ModulesIDBefore,md.TimesBT,mt.TypeName, md.ModulesTypeID,pm.ProgramGroupID, md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName,pm.Semester,p.CourseIndustryID
        //",
        //                new { }).ToList();
        //            if (obj != null)
        //            {
        //                return obj;
        //            }
        //            else
        //            {
        //                return new List<ModuleDetail>();
        //            }
        //        }
        public List<KQHT> GetKQHTByUser(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<KQHT> obj = sqlconnection.Query<KQHT>(@"select ic.IndependentClassID,m.ModulesName,ic.ClassName,ic.ClassCode,us.Score,us.ScoreType from vnk_UserScore us
join IndependentClass ic on ic.IndependentClassID=us.IndependentClassID and UserID=@UserID
join Modules m on m.ModulesID= ic.ModulesID",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<KQHT>();
            }
        }
        public List<KQHT> GetKQHTByClass(int IndependentClassID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<KQHT> obj = sqlconnection.Query<KQHT>(@"select u.UserID,u.Usercode, (u.Lastname + ' ' + u.Firstname) Fullname,m.ModulesName,ic.ClassName,ic.ClassCode,us.Score,us.ScoreType from vnk_UserScore us
join IndependentClass ic on ic.IndependentClassID=us.IndependentClassID and us.IndependentClassID=@IndependentClassID
join Modules m on m.ModulesID= ic.ModulesID 
join vnk_User u on u.UserID= us.UserID",
                new { @IndependentClassID = IndependentClassID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<KQHT>();
            }
        }
        public List<Certificate> GetCertificateByUser(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<Certificate> obj = sqlconnection.Query<Certificate>(@"select cc.CertificateName,cc.CertificateCode,cu.ID from CertificateChannel cc
join CertificateCI cci on cci.CertificateID=cc.CertificateID and cci.CourseIndustryID=751
left join CertificateUser cu on cu.CertificateID=cc.CertificateID and UserID=@UserID",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<Certificate>();
            }
        }
        public List<IndependentClass> GetIC(int ModulesID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<IndependentClass> obj = sqlconnection.Query<IndependentClass>(@"SELECT TBL.COSTS,STRING_AGG(TBL.RoomName,',') AS RoomName, TBL.Description, TBL.ClassName, TBL.ClassCode, TBL.Teachername, TBL.StartDate, TBL.Credits, TBL.MaxStudent FROM
(select (SELECT TOP 1 Costs FROM vnk_IndependentClassUser WHERE IndependentClassID=ICTP.IndependentClassID AND UserID=32783 ) AS COSTS,R.RoomName,ET.Description,ic.ClassName,ic.ClassCode, (select (Lastname + ' ' + Firstname) from vnk_User where UserID=ict.UserID) as Teachername,ic.StartDate,m.Credits,ic.MaxStudent from vnk_IndependentClass ic
join vnk_Modules m on m.ModulesID=ic.ModulesID and CourseID=100 and Semester=10 and ic.ModulesID=@ModulesID
join IndependentClassTeacher ict on ict.IndependentClassID=ic.IndependentClassID
join IndependentClassTimesPlan ictp on ictp.IndependentClassID=ic.IndependentClassID
JOIN RoomStudy RS ON RS.IndependentClassID=ICTP.IndependentClassID
JOIN ROOM R ON R.RoomID=RS.RoomID
JOIN vnk_ExamTime ET ON ET.ExamTimeID= ICTP.Times
group by ICTP.IndependentClassID,R.RoomName,ET.Description,ic.ClassName,ic.ClassCode,ict.UserID,ic.StartDate,m.Credits,ic.MaxStudent) AS TBL
GROUP BY TBL.COSTS,TBL.Description, TBL.ClassName, TBL.ClassCode, TBL.Teachername, TBL.StartDate, TBL.Credits, TBL.MaxStudent",
                new { @ModulesID = ModulesID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<IndependentClass>();
            }
        }
        public List<DsGtHs> GetDsGtHs(int UserID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<DsGtHs> obj = sqlconnection.Query<DsGtHs>(@"select RL.RevenuesListName,RL.Num,RU.Num as NumU from RevenuesList RL
JOIN RevenuesUser RU ON RL.RevenuesListID= RU.RevenuesListID where RU.UserID=@UserID",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<DsGtHs>();
            }
        }      
        public List<TradeHistory> GetTradeHistory(int UserID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TradeHistory> obj = sqlconnection.Query<TradeHistory>(@"select CreatedTime, iif(LEFT(Costs,1)!='-','+',LEFT(Costs,1)) AS Status,iif(left(Costs,1)='-',SUBSTRING(CAST(Costs AS VARCHAR),1,LEN(CAST(Costs AS VARCHAR))),Costs) AS Costs,Description from LogCharge where UserID=@UserID",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<TradeHistory>();
            }
        }
        public List<Message> GetMessage(int ClassID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<Message> obj = sqlconnection.Query<Message>(@"select c.CommentID,c.ParentID,c.OwnerID,c.Content,(select Lastname+' '+Firstname from vnk_User where UserID=c.OwnerID)as fromuser,(select Lastname+' '+Firstname from vnk_User where UserID=c.FieldID)as touser,c.CreatedTime from vnk_Comment c
join ClassUser cu on (cu.UserID=c.OwnerID or cu.UserID=c.PeopleID or c.FieldID=cu.UserID) and cu.ClassID=@ClassID
join vnk_User u on u.UserID=cu.UserID",
                new { @ClassID = ClassID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<Message>();
            }
        }
        public List<ChannelAmount> GetChannelAmount(int ClassID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ChannelAmount> obj = sqlconnection.Query<ChannelAmount>(@"SELECT Day,ModifiedTime,ChannelAmountID,ChannelAmountName,(SELECT TOP 1 Amount FROM StudentAmount WHERE ChannelAmountID=CA.ChannelAmountID) AS Costs FROM ChannelAmount CA
WHERE TypeID=2 AND IsLock=0",
                new { @ClassID = ClassID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {   
                return new List<ChannelAmount>();
            }
        }
        public List<StudentAmount> GetStudentAmount(int UserID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<StudentAmount> obj = sqlconnection.Query<StudentAmount>(@"select ca.ChannelAmountName,sa.Amount,sa.Quantity,(sa.Quantity*sa.Amount) as TotalAmount,sa.CreatedTime,
sa.StatusID,ca.Day from StudentAmount sa
join ChannelAmount ca on ca.ChannelAmountID=sa.ChannelAmountID
where sa.UserID=@UserID and ca.IsLock=0 and ca.TypeID=2
order by sa.CreatedTime desc",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<StudentAmount>();
            }
        }
        public List<TTCN> GetTTCNDone(int UserID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TTCN> obj = sqlconnection.Query<TTCN>(@"select ic.ClassCode,ic.ClassName,m.Credits,icu.Costs,icu.Status from vnk_IndependentClassUser icu
join IndependentClass ic on icu.IndependentClassID=ic.IndependentClassID
join Modules m on m.ModulesID= ic.ModulesID
where UserID=32783
union all
select ca.ChannelAmountCode,ca.ChannelAmountName,null,sa.Amount,sa.Paid from StudentAmount sa
join ChannelAmount ca on ca.ChannelAmountID=sa.ChannelAmountID
where sa.UserID=32783 and sa.Paid=1 and (sa.StatusID =3 or sa.StatusID is null)",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<TTCN>();
            }
        }
        public List<TTCN> GetTTCN(int UserID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TTCN> obj = sqlconnection.Query<TTCN>(@"select ic.ClassCode,m.ModulesName,m.Credits,icu.Costs from vnk_IndependentClassUser icu
join IndependentClass ic on ic.IndependentClassID=icu.IndependentClassID 
join Modules m on m.ModulesID=ic.ModulesID
where icu.Paid=0 and icu.UserID=@UserID and icu.Del=0
union all
select ca.ChannelAmountCode,ca.ChannelAmountName,null,sa.Amount from StudentAmount sa
join ChannelAmount ca on ca.ChannelAmountID=sa.ChannelAmountID
where UserID=@UserID and Paid=0 and sa.Del=0",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<TTCN>();
            }
        }
        public List<TTCN> GetExamResult(int UserID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TTCN> obj = sqlconnection.Query<TTCN>(@"select mus.CreatedTime,mus.ModifiedTime,icu.IndependentClassID,m.ModulesCode,('HP'+cast(m.ModulesID as varchar(50))) as PrintCode,m.ModulesName,
mus.SemesterIndex,m.Credits,(select Score from vnk_UserScore where ScoreType=50 and UserID=mus.UserID 
and IndependentClassID=icu.IndependentClassID) as SGKL1,
(select Sum(Score)/COUNT(*) from vnk_UserScore where (ScoreType=1 or ScoreType=2 or ScoreType=3 or ScoreType=4 or
ScoreType=5 or ScoreType=6) and UserID=mus.UserID 
and IndependentClassID=(select IndependentClassID from vnk_IndependentClassUser where ModulesID=mus.ModulesID AND UserID=mus.UserID)) as TBKTTK
,(SELECT Score FROM vnk_UserEnScore WHERE NoID=(select NoID from vnk_UserNoID where UserID=mus.UserID and ExamPlanTimeID=
(select TOP 1 ExamPlanTimeID from vnk_ExamPlanTime where IndependentClassID=icu.IndependentClassID))) AS EXAM,
mus.ScoreFinal,cast(mus.ScoreFinal/10*4 as decimal(2,1)) AS D4,
fd.XH,fd.SymbolName
from ModulesUserScore mus
left join Modules m on m.ModulesID=mus.ModulesID
left join vnk_IndependentClassUser icu on icu.ModulesID=mus.ModulesID AND icu.UserID=mus.UserID
left join FormulaDetail fd on fd.FormulaID=mus.FormulaID and mus.ScoreFinal>=fd.StartScore and mus.ScoreFinal<=fd.EndScore
where mus.UserID=@UserID",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<TTCN>();
            }
        }
        public List<TTCN> GetExamByClass(int IndependentClassID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TTCN> obj = sqlconnection.Query<TTCN>(@"select u.UserID,u.Usercode,(Lastname+' '+Firstname)as Fullname,mus.Score1,mus.Score2 from IndependentClass ic
left join vnk_IndependentClassUser icu on icu.IndependentClassID=ic.IndependentClassID
left join ModulesUserScore mus on mus.ModulesID=ic.ModulesID and mus.UserID=icu.UserID
left join vnk_User u on u.UserID=mus.UserID
where ic.IndependentClassID=@IndependentClassID",
                new { @IndependentClassID = IndependentClassID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<TTCN>();
            }
        }
    }
}