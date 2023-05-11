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
        public List<ModuleDetail> GetKQHTByUser(int ModulesID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ModuleDetail> obj = sqlconnection.Query<ModuleDetail>(@"select m.ModulesName,ic.ClassName,ic.ClassCode,us.Score,us.ScoreType,* from vnk_UserScore us
join IndependentClass ic on ic.IndependentClassID=us.IndependentClassID and UserID=32783
join Modules m on m.ModulesID= ic.ModulesID",
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
        public List<ModuleDetail> GetKQHTByClass(int ModulesID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ModuleDetail> obj = sqlconnection.Query<ModuleDetail>(@"select u.Usercode, (u.Lastname + ' ' + u.Firstname) Fullname,m.ModulesName,ic.ClassName,ic.ClassCode,us.Score,us.ScoreType,* from vnk_UserScore us
join IndependentClass ic on ic.IndependentClassID=us.IndependentClassID and us.IndependentClassID=43698
join Modules m on m.ModulesID= ic.ModulesID 
join vnk_User u on u.UserID= us.UserID",
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
        public List<ModuleDetail> GetCertificateByUser(int ModulesID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ModuleDetail> obj = sqlconnection.Query<ModuleDetail>(@"select cc.CertificateName,cc.CertificateCode,* from CertificateUser cu 
join CertificateChannel cc on cc.CertificateID=cu.CertificateID and cu.UserID=32783",
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
        public List<ModuleDetail> GetCertificateAll(int ModulesID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ModuleDetail> obj = sqlconnection.Query<ModuleDetail>(@"select cc.CertificateName,cc.CertificateCode,* from CertificateChannel cc
join CertificateCI cci on cci.CertificateID=cc.CertificateID and cci.CourseIndustryID=751",
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
        public List<ModuleDetail> GetTC(int ModulesID) //TC=Teacher Calendar
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ModuleDetail> obj = sqlconnection.Query<ModuleDetail>(@"select cc.CertificateName,cc.CertificateCode,* from CertificateChannel cc
join CertificateCI cci on cci.CertificateID=cc.CertificateID and cci.CourseIndustryID=751",
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
    }
}