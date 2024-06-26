﻿using Dapper;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
//using Net6WebApiTemplate.Application.Products.Interfaces;
using Net6WebApiTemplate.Application.Common.Interfaces;
using Net6WebApiTemplate.Application.Products.Dto;
using Net6WebApiTemplate.Application.Shared.Interface;
using Net6WebApiTemplate.Domain.Entities;
using Org.BouncyCastle.Utilities;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Net7studentportal.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private IConfiguration _configuration;

        public class AppSetting
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string ValueOf { get; set; }
        }
        public ProductRepository(ISqlConnectionFactory connectionFactory, IConfiguration configuration)
        {
            _connectionFactory = connectionFactory;
            try
            {
                _configuration = configuration;
                _configuration["TERMID"] = GetAppSettings()[0].ValueOf;
            }
            catch
            {
                _configuration = configuration;
            }

        }
        public List<AppSetting> GetAppSettings()
        {
            using var sqlconnectionLocal = _connectionFactory.CreateConnectionLocal();
            sqlconnectionLocal.Open();
            var TERMID = sqlconnectionLocal.Query<AppSetting>("Select * from AppSetting").ToList();
            return TERMID;
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
            List<News> obj = sqlconnection.Query<News>(@"select * from vnk_News where channelID=40 and statusID=1 order by ModifiedTime desc",
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
        public List<StudenClass> GetStudentClass(int ClassID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<StudenClass> obj = sqlconnection.Query<StudenClass>(@"select Usercode, (Lastname+' '+Firstname) Username, Address, Phone,Email from ClassUser cr join vnk_User vr on cr.UserID=vr.UserID and cr.ClassID=@ClassID order by Firstname",
                new { @ClassID = ClassID }).ToList();
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
            //string hash = "";

            //var objHash = new { Username, Password };
            //hash = objHash.GetHashCode().ToString();

            using var sqlconnection = _connectionFactory.CreateConnection();
            StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select co.CourseID,fd.XH,vr.Amount,vr.UserId, (vr.Lastname + ' ' + vr.Firstname) Fullname, vr.Usercode,
			(select (Lastname + ' ' + Firstname) Fullname from vnk_User where UserID= ct.UserID) as TeacherName,
			c.ClassName,
c.ClassID,
			I.IndustryName,
i.IndustryCode,
tl.TranningLevelName,
ci.CourseIndustryID,
			d.DepartmentName,
			um.TBCTL,
um.TotalCredits,
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
left join TranningLevel tl on tl.TranningLevelID=i.TranningLevelID
left join FormulaDetail fd on fd.FormulaID=co.FormulaID and um.TBCTL/4*10>=fd.StartScore and um.TBCTL/4*10<=fd.EndScore",
            new { @Username = Username, @Password = Password }).FirstOrDefault();
            if (obj != null)
            {
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
            StudentInfo obj = sqlconnection.Query<StudentInfo>(@"select co.CourseID,fd.XH,vr.Amount,vr.UserId, (vr.Lastname + ' ' + vr.Firstname) Fullname, vr.Usercode,
			(select (Lastname + ' ' + Firstname) Fullname from vnk_User where UserID= ct.UserID) as TeacherName,
			c.ClassName,
c.ClassID,
			I.IndustryName,
i.IndustryCode,
tl.TranningLevelName,
ci.CourseIndustryID,
			d.DepartmentName,
			um.TBCTL,
um.TotalCredits,
            vr.Email, vr.Phone, vr.[Address], vr.Images, co.CourseName,co.CourseCode,cid.Credits
            from vnk_User vr
			join ClassUser cu on cu.UserID=vr.UserID and (vr.email = @email or vr.Usercode=@email)
			left join Class c on c.ClassID= cu.ClassID
		 left	join ClassTeacher ct on ct.ClassID = cu.ClassID
           left join CourseIndustry ci on ci.CourseIndustryID = c.CourseIndustryID
			left join Industry i on i.IndustryID=ci.IndustryID
			left join Department d on d.DepartmentID=i.DepartmentID
			left join UserMark um on um.UserID=vr.UserID
		left	join vnk_Course co on co.CourseID= ci.CourseID
           left join CourseIndustryDetail cid on cid.CourseIndustryID = ci.CourseIndustryID
left join TranningLevel tl on tl.TranningLevelID=i.TranningLevelID
left join FormulaDetail fd on fd.FormulaID=co.FormulaID and um.TBCTL/4*10>=fd.StartScore and um.TBCTL/4*10<=fd.EndScore",
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
        public string PostTTCN(int UserID, string ttcnid, decimal amount)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            sqlconnection.Open();
            using var sqlconnectionLocal = _connectionFactory.CreateConnectionLocal();
            sqlconnectionLocal.Open();
            var trans = sqlconnection.BeginTransaction();
            var transLocal = sqlconnectionLocal.BeginTransaction();
            try
            {
                int obj = 0;
                string[] id = ttcnid.Split(',');
                for (int i = 0; i < id.Length; i++)
                {
                    if (id[i].IndexOf('k') == 0)
                    {
                        TTCN getttcn = GetTTCN(UserID).Where(x => x.id == int.Parse(id[i].Substring(1, id[i].Length - 1)) && x.Credits == 0).FirstOrDefault();
                        amount = amount - getttcn.Costs;
                        if (amount < 0)
                        {
                            return "S";
                        }
                        else
                        {
                            sqlconnectionLocal.Execute("insert into logcharge(CreatedTime, Costs, Description, UserID) values(GETUTCDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'SE Asia Standard Time', @Costs, @Description, @UserID)",
                  new { UserID = UserID, @Description = getttcn.ClassName, @Costs = -getttcn.Costs }, transLocal);
                        }
                    }
                    else
                    {
                        TTCN getttcn = GetTTCN(UserID).Where(x => x.id == int.Parse(id[i]) && x.Credits != 0).FirstOrDefault();
                        amount = amount - getttcn.Costs;
                        if (amount < 0)
                        {
                            return "S";
                        }
                        else
                        {
                            sqlconnectionLocal.Execute("insert into logcharge(CreatedTime, Costs, Description, UserID) values(GETUTCDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'SE Asia Standard Time', @Costs, @Description, @UserID)",
                  new { UserID = UserID, @Description = "Thu học phí lớp: " + getttcn.ClassName + " - " + getttcn.ModulesName, @Costs = -getttcn.Costs }, transLocal);
                        }
                    }
                }
                if (amount > 0)
                {
                    for (int i = 0; i < id.Length; i++)
                    {
                        if (id[i].IndexOf('k') == 0)
                        {
                            obj = sqlconnection.Execute(@"update StudentAmount set paid=1 where UserID=@UserID and StudentAmountID=@id and isnull(Del,0)=0
update vnk_User set amount=@amount where UserID=@UserID",
                  new { UserID = UserID, id = int.Parse(id[i].Substring(1, id[i].Length - 1)), amount = amount }, trans);
                        }
                        else
                        {
                            obj = sqlconnection.Execute(@"update vnk_IndependentClassUser set paid=1 where IndependentClassID=@id and UserID=@UserID and isnull(Del,0)=0
update vnk_User set amount=@amount where UserID=@UserID",
                  new { UserID = UserID, id = int.Parse(id[i]), amount = amount }, trans);
                        }
                    }

                    if (obj > 1)
                    {
                        transLocal.Commit();
                        trans.Commit();
                        return "Y";
                    }
                    else
                    {
                        transLocal.Rollback();
                        trans.Rollback();
                        return "N";
                    }
                }
                else
                {
                    transLocal.Rollback();
                    trans.Rollback();
                    return "S";
                }

            }
            catch
            {
                transLocal.Rollback();
                trans.Rollback();
                return "N";
            }

        }
        public string PostOneDoor(int UserID, string odid, decimal amount)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            sqlconnection.Open();
            using var sqlconnectionLocal = _connectionFactory.CreateConnectionLocal();
            sqlconnectionLocal.Open();
            var trans = sqlconnection.BeginTransaction();
            var transLocal = sqlconnectionLocal.BeginTransaction();
            try
            {
                int obj = 0;
                string[] id = odid.Split(',');
                for (int i = 0; i < id.Length; i++)
                {
                    ChannelAmount getod = GetChannelAmountBy(int.Parse(id[i].Split('-')[0])).FirstOrDefault();
                    amount = amount - (getod.Costs * int.Parse(id[i].Split('-')[1]));
                    if (amount < 0)
                    {
                        return "S";
                    }
                    else
                    {
                        sqlconnectionLocal.Execute(@"insert into LogCharge(CreatedTime, Costs, Description, UserID) values(GETUTCDATE() AT TIME ZONE 'UTC' AT TIME ZONE 'SE Asia Standard Time', @Costs, @Description, @UserID)",
                   new { UserID = UserID, @Description = "Đăng ký DVHC: " + getod.ChannelAmountName + "(SL: " + int.Parse(id[i].Split('-')[1]) + ")", @Costs = -(getod.Costs * int.Parse(id[i].Split('-')[1])) }, transLocal);
                    }
                    if (amount > 0)
                    {
                        obj += sqlconnection.Execute(@"insert into StudentAmount(Amount,Quantity,StatusID,UserID,CreatedTime,ChannelAmountID) values(@AmountA,@Quantity,
1,@UserID,GETDATE(),@ChannelAmountID)
update vnk_User set amount=@amount where UserID=@UserID",
             new { @UserID = UserID, @ChannelAmountID = int.Parse(id[i].Split('-')[0]), @amount = amount, AmountA = getod.Costs, @Quantity = int.Parse(id[i].Split('-')[1]) }, trans);

                    }
                    else
                    {
                        transLocal.Rollback();
                        trans.Rollback();
                        return "S";
                    }

                }
                if (obj == id.Length*2)
                {
                    transLocal.Commit();
                    trans.Commit();
                    return "Y";
                }
                else
                {
                    transLocal.Rollback();
                    trans.Rollback();
                    return "N";
                }
            }
            catch
            {
                transLocal.Rollback();
                trans.Rollback();
                return "N";
            }

        }
        public int ChangePassword(string username, string oldpass, string newpass)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            sqlconnection.Open();
            var trans = sqlconnection.BeginTransaction();
            int count = 0;
            count = sqlconnection.Execute(@"update vnk_User set password= @newpass where username=@username and password=@oldpass",
           new { @username = username, @oldpass = @oldpass, @newpass = newpass }, trans);
            if (count > 0)
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
        public List<ProgramSemester> GetProgramSemester(int CourseIndustryID, int CourseID, int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ProgramSemester> obj = sqlconnection.Query<ProgramSemester>(@"select iif((select top 1 UserId from vnk_UserScore us where IndependentClassID=(select IndependentClassID from vnk_IndependentClassUser where ModulesID=mus.ModulesID and UserID= mus.UserID and isnull(Del,0)=0)) is not null, 1,0) kqht,mus.Score1,mus.ScoreFinal,ROUND((cast(fd.d4 as FLOAT)),1) AS D4,
fd.XH,fd.SymbolName,tbl.ModulesID,tbl.TimesK,tbl.SemesterID,tbl.MinCreditsLT,tbl.MinCreditsTH,tbl.MinCreditsK,tbl.GroupName,tbl.CreditsS,string_agg(tbl.ModulesTQ,', <br>')as ModulesTQ,string_agg(tbl.ModulesHT,', <br>') as ModulesHT, tbl.TimesBT,tbl.TypeName,tbl.CreatedClass, tbl.ModulesTypeID,tbl.ProgramGroupID,tbl.ModulesCode, tbl.ModulesName,tbl.CreditsLT,tbl.CreditsTH,tbl.CreditsK, tbl.Credits,TimesLT,TimesTH,TimesTL, tbl.SemesterName, tbl.Credits0,tbl.Credits1,iif(tbl.chk=1, 'x','') chked  from

(select (select top 1 1 from IndependentClass where ModulesID=md.ModulesID AND CourseID = ic.CourseID
and TermID=@TermID) chk,md.ModulesID,md.TimesK,s.SemesterID,pg.MinCreditsLT,pg.MinCreditsTH,pg.MinCreditsK,pg.GroupName,cis.Credits as CreditsS,(select ModulesCode from vnk_Modules where ModulesID=mp.ModulesIDPremise) as ModulesTQ,(select ModulesCode from vnk_Modules where ModulesID=mb.ModulesIDBefore) as ModulesHT, md.TimesBT,mt.TypeName,COUNT(*) CreatedClass, md.ModulesTypeID,pm.ProgramGroupID,md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName, (select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=0) as Credits0,(select Credits from CourseIndustrySemester where CourseIndustryID = p.CourseIndustryID  and SemesterID=pm.Semester and TypeID=1) as Credits1  from vnk_Modules md
join Program p on p.StructProgramID = md.StructProgramID and p.CourseIndustryID = @CourseIndustryID
left join ProgramGroup pg on pg.ProgramID = p.ProgramID
join ProgramModules pm on(pm.ProgramGroupID= pg.ProgramGroupID or (pm.ProgramGroupID = 0)) and p.ProgramID = pm.ProgramID and md.ModulesID = pm.ModulesID and isnull(pm.Del, 0)= 0
left join IndependentClass ic on ic.ModulesID = md.ModulesID and pm.Semester = ic.Semester and ic.CourseID = @CourseID 
join Semester s on s.SemesterID=pm.Semester
join ModulesType mt on mt.TypeID=md.ModulesTypeID
left join ModulesBefore mb on mb.ModulesID=md.ModulesID and isnull(mb.Del,0)=0
left join ModulesPremise mp on mp.ModulesID=md.ModulesID and isnull(mp.Del,0)=0
join CourseIndustrySemester cis on cis.CourseIndustryID = p.CourseIndustryID and cis.TypeID = 0 and cis.SemesterID=pm.Semester
group by ic.CourseID,md.ModulesID,md.TimesK,s.SemesterID,pg.MinCreditsLT,pg.MinCreditsTH,pg.MinCreditsK,pg.GroupName,cis.Credits,mp.ModulesIDPremise,mb.ModulesIDBefore,md.TimesBT,mt.TypeName, md.ModulesTypeID,pm.ProgramGroupID, md.ModulesCode, md.ModulesName,md.CreditsLT,md.CreditsTH,md.CreditsK, md.Credits,TimesLT,TimesTH,TimesTL, s.SemesterName,pm.Semester,p.CourseIndustryID) as tbl
left join ModulesUserScore mus on mus.ModulesID=tbl.ModulesID and UserID=@UserID and exists(select 1 from vnk_IndependentClassUser where UserID=mus.UserID and ModulesID=mus.ModulesID and isnull(Del,0)=0)
left join FormulaDetail fd on fd.FormulaID=mus.FormulaID and mus.ScoreFinal>=fd.StartScore and mus.ScoreFinal<=fd.EndScore
group by  mus.ModulesID,mus.UserID,fd.d4,tbl.chk,mus.Score1,mus.ScoreFinal,fd.XH,fd.SymbolName,tbl.ModulesID,tbl.TimesK,tbl.SemesterID,tbl.MinCreditsLT,tbl.MinCreditsTH,tbl.MinCreditsK,tbl.GroupName,tbl.CreditsS, tbl.TimesBT,tbl.TypeName,tbl.CreatedClass, tbl.ModulesTypeID,tbl.ProgramGroupID,tbl.ModulesCode, tbl.ModulesName,tbl.CreditsLT,tbl.CreditsTH,tbl.CreditsK, tbl.Credits,TimesLT,TimesTH,TimesTL, tbl.SemesterName, tbl.Credits0,tbl.Credits1
order by tbl.SemesterID",
            new { @CourseIndustryID = CourseIndustryID, @CourseID = CourseID, @TermID = _configuration["TERMID"].ToString(), @UserID = UserID }).ToList();

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
        public List<KQHT> GetKQHTByUser(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<KQHT> obj = sqlconnection.Query<KQHT>(@"select m.ModulesID,m.Credits,(u.Lastname+' '+u.Firstname) as fullname,
u.Usercode,c.ClassName as class,ic.IndependentClassID,m.ModulesName,
ic.ClassName,ic.ClassCode,round(us.Score,1) Score,us.ScoreType from vnk_IndependentClassUser icu
left join vnk_UserScore us on us.IndependentClassID=icu.IndependentClassID and us.UserID=icu.UserID
left join IndependentClass ic on ic.IndependentClassID=icu.IndependentClassID
left join Modules m on m.ModulesID= ic.ModulesID
left join vnk_User u on u.UserID=icu.UserID
left join ClassUser cu on cu.UserID=u.UserID
left join Class c on c.ClassID=cu.ClassID
where icu.UserID=@UserID",
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
            List<KQHT> obj = sqlconnection.Query<KQHT>(@"select u.UserID,u.Usercode, (u.Lastname + ' ' + u.Firstname) Fullname,m.ModulesName,ic.ClassName,ic.ClassCode,round(us.Score,1) Score,us.ScoreType from vnk_UserScore us
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
        public List<Certificate> GetCertificateByUser(int UserID, int CourseIndustryID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<Certificate> obj = sqlconnection.Query<Certificate>(@"select cc.CertificateName,cc.CertificateCode,cu.ID from CertificateChannel cc
join CertificateCI cci on cci.CertificateID=cc.CertificateID and cci.CourseIndustryID=@CourseIndustryID
left join CertificateUser cu on cu.CertificateID=cc.CertificateID and UserID=@UserID",
                new { @UserID = UserID, @CourseIndustryID = CourseIndustryID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<Certificate>();
            }
        }
        public List<IndependentClass> GetIC(int ModulesID, int CourseID, int CourseIndustryID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<IndependentClass> obj = sqlconnection.Query<IndependentClass>(@"select tbl2.ModulesName,tbl2.IndependentClassID,tbl2.TimesInDay,tbl2.DayStudy,MIN(tbl2.timeday) AS timeday, max(tbl2.RoomName) roomname,tbl2.SSSV,tbl2.Amount, tbl2.ClassName, tbl2.ClassCode, tbl2.Teachername,tbl2.ModulesID, tbl2.StartDate, tbl2.Credits from (SELECT tbl.TimesInDay,tbl.DayStudy, STRING_AGG(tbl.StudyTime,',') as timeday,tbl.IndependentClassID,tbl.ModulesName,tbl.SSSV,TBL.Amount,TBL.RoomName,tbl.ModulesID, TBL.ClassName, TBL.ClassCode, TBL.Teachername, TBL.StartDate, TBL.Credits FROM
(select rs.TimesInDay,((select cast(count(*) as varchar(50)) from vnk_IndependentClassUser where IndependentClassID=ic.IndependentClassID)+'/'+cast(ic.MaxStudent as varchar(50))) as SSSV , 
 m.ModulesName,rs.DayStudy,rs.StudyTime,cafci.Amount,r.RoomName,ic.IndependentClassID,ic.ClassName,ic.ClassCode,ic.ModulesID, (select (Lastname + ' ' + Firstname) from vnk_User where UserID=ict.UserID) as Teachername,ic.StartDate,m.Credits from vnk_IndependentClass ic
left join vnk_Modules m on m.ModulesID=ic.ModulesID 
left join IndependentClassTeacherChange ictc on ictc.IndependentClassID=ic.IndependentClassID
left JOIN RoomStudy RS ON RS.IndependentClassID=IC.IndependentClassID
left JOIN ROOM R ON R.RoomID=RS.RoomID
left join ChannelAmountFee_CI cafci on cafci.CourseIndustryID=@CourseIndustryID and cafci.ModulesTypeID=m.ModulesTypeID and cafci.ApplicateDate=(select max(ApplicateDate) from ChannelAmountFee_CI where 
CourseIndustryID=@CourseIndustryID and ModulesTypeID=m.ModulesTypeID)
left join IndependentClassTeacher ict on ict.IndependentClassID=ic.IndependentClassID
where ic.ModulesID=@ModulesID and ic.CourseID=@CourseID AND IC.TERMID=@TERMID
group by m.ModulesName,ic.ModulesID,rs.TimesInDay,ic.IndependentClassID,RS.DayStudy,RS.StudyTime,cafci.Amount,R.RoomName,ic.ClassName,ict.UserID,ic.ClassCode,ic.StartDate,m.Credits,ic.MaxStudent) AS TBL
GROUP BY tbl.ModulesName,tbl.IndependentClassID,tbl.TimesInDay,tbl.DayStudy,tbl.SSSV,TBL.Amount,TBL.RoomName, TBL.ClassName, TBL.ClassCode, TBL.Teachername, TBL.StartDate, TBL.Credits,tbl.ModulesID)TBL2
group by tbl2.ModulesName,tbl2.IndependentClassID,tbl2.TimesInDay,tbl2.DayStudy,tbl2.SSSV,tbl2.Amount, tbl2.ClassName, tbl2.ClassCode, tbl2.Teachername, tbl2.StartDate, tbl2.Credits,tbl2.ModulesID",
                new { @ModulesID = ModulesID, @TERMID = _configuration["TERMID"].ToString(), @CourseID = CourseID, @CourseIndustryID = CourseIndustryID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<IndependentClass>();
            }
        }
        public List<DsGtHs> GetDsGtHs(int UserID)
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
        public List<TradeHistory> GetTradeHistory(int UserID)
        {
            using var sqlconnectionLocal = _connectionFactory.CreateConnectionLocal();
            sqlconnectionLocal.Open();
            List<TradeHistory> obj = sqlconnectionLocal.Query<TradeHistory>(@"select CreatedTime, iif(LEFT(Costs,1)='-' or Costs=0,'-','+') AS Status,iif(left(Costs,1)='-',SUBSTRING(CAST(Costs AS VARCHAR),1,LEN(CAST(Costs AS VARCHAR))),Costs) AS Costs,Description from LogCharge where UserID=@UserID",
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
        public List<Message> GetMessage(int ClassID)
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
        public string PostMessage(int UserID, string content)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            int obj = sqlconnection.Execute(@"INSERT INTO VNK_COMMENT(OwnerID,Content,CreatedTime) VALUES(@OwerID, @Content,getdate())",
                new { @OwerID = UserID, @Content = content });
            if (obj > 0)
            {
                return "Y";
            }
            else
            {
                return "N";
            }
        }
        public string PostIC(int UserID, int IndependentClassID, int Costs, int ModulesID, decimal amount)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            sqlconnection.Open();
            var trans = sqlconnection.BeginTransaction();
            try
            {
                int obj = sqlconnection.Execute(@"INSERT INTO IndependentClassUserRegister(UserID,OwnerID,IndependentClassID,CreatedTime) VALUES(@UserID,@OwerID,@IndependentClassID,getdate())
INSERT INTO vnk_IndependentClassUser(IndependentClassID,ModulesID,UserID,Costs,Paid,FormulaID,CreatedTime,OwnerID,PeopleID,ApplicationID) VALUES(@IndependentClassID,@ModulesID,@UserID,@Costs,0,2,getdate(),@OwerID,@PeopleID,1)",
                new { @UserID = UserID, @OwerID = UserID, @IndependentClassID = IndependentClassID, @ModulesID = ModulesID, @Costs = Costs, @PeopleID = UserID }, trans);
                if (obj > 1)
                {
                    trans.Commit();
                    return "Y";

                }
                else
                {
                    trans.Rollback();
                    return "N";
                }
            }
            catch
            {
                trans.Rollback();
                return "N";
            }

        }
        public string DeleteDKHP(int UserID, int IndependentClassID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            sqlconnection.Open();
            var trans = sqlconnection.BeginTransaction();
            try
            {
                int obj = sqlconnection.Execute(@"DELETE vnk_IndependentClassUser WHERE IndependentClassID=@IndependentClassID and UserID=@UserID
INSERT INTO IndependentClassUserRegister(UserID,OwnerID,IndependentClassID,CreatedTime, Del) VALUES(@UserID,@OwerID,@IndependentClassID,getdate(),1)",
                new { @UserID = UserID, @OwerID = UserID, @IndependentClassID = IndependentClassID }, trans);
                if (obj > 1)
                {
                    trans.Commit();
                    return "Y|Hủy thành công";

                }
                else
                {
                    trans.Rollback();
                    return "N|Hủy không thành công";
                }
            }
            catch
            {
                trans.Rollback();
                return "N|Hủy không thành công";
            }

        }
        public List<ChannelAmount> GetChannelAmount(int ClassID)
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
        public List<ChannelAmount> GetChannelAmountBy(int ClassID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ChannelAmount> obj = sqlconnection.Query<ChannelAmount>(@"SELECT Day,ModifiedTime,ChannelAmountID,ChannelAmountName,(SELECT TOP 1 Amount FROM StudentAmount WHERE ChannelAmountID=CA.ChannelAmountID) AS Costs FROM ChannelAmount CA
WHERE TypeID=2 AND IsLock=0 and CA.ChannelAmountID=@ClassID",
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
        public List<StudentAmount> GetStudentAmount(int UserID)
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
        public List<TTCN> GetTTCNDone(int UserID)
        {

            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TTCN> obj = sqlconnection.Query<TTCN>(@"            select ic.ClassCode,ic.ClassName,m.Credits,icu.Costs,icu.Status from vnk_IndependentClassUser icu
join IndependentClass ic on icu.IndependentClassID = ic.IndependentClassID
join Modules m on m.ModulesID = ic.ModulesID
where UserID = @UserID and icu.Paid=1 and isnull(icu.Del,0)=0
union all
select ca.ChannelAmountCode,ca.ChannelAmountName,null,sa.Amount,sa.Paid from StudentAmount sa
join ChannelAmount ca on ca.ChannelAmountID=sa.ChannelAmountID
where sa.UserID=@UserID and sa.Paid=1 and (sa.StatusID =3 or sa.StatusID is null)",
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
        public List<TTCN> GetTTCN(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TTCN> obj = sqlconnection.Query<TTCN>(@"select ic.ClassCode,m.ModulesName,m.Credits,icu.Costs,icu.IndependentClassID as id from vnk_IndependentClassUser icu
join IndependentClass ic on ic.IndependentClassID=icu.IndependentClassID 
join Modules m on m.ModulesID=ic.ModulesID
where icu.Paid=0 and icu.UserID=@UserID and isnull(icu.Del,0)=0
union all
select ca.ChannelAmountCode,ca.ChannelAmountName,null,sa.Amount,sa.StudentAmountID from StudentAmount sa
join ChannelAmount ca on ca.ChannelAmountID=sa.ChannelAmountID
where UserID=@UserID and Paid=0 and isnull(sa.Del,0)=0",
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
        public List<ExamResult> GetExamResult(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ExamResult> obj = sqlconnection.Query<ExamResult>(@"select (u.Lastname+' '+u.Firstname) as fullname,u.Usercode,(select ClassName from(select ClassID from ClassUser where UserID=u.UserID)class) as class ,mus.CreatedTime,mus.ModifiedTime,icu.IndependentClassID,m.ModulesCode,('HP'+cast(m.ModulesID as varchar(50))) as PrintCode,m.ModulesName,
mus.SemesterIndex,m.ModulesID,m.Credits,ROUND((select Score from vnk_UserScore where ScoreType=50 and UserID=mus.UserID 
and IndependentClassID=icu.IndependentClassID),1) as SGKL1,
ROUND((select Sum(Score)/COUNT(*) from vnk_UserScore where (ScoreType=1 or ScoreType=2 or ScoreType=3 or ScoreType=4 or
ScoreType=5 or ScoreType=6) and UserID=mus.UserID 
and IndependentClassID=(select top 1 IndependentClassID from vnk_IndependentClassUser where ModulesID=mus.ModulesID AND UserID=mus.UserID)),1) as TBKTTK
,ROUND((SELECT Score FROM vnk_UserEnScore WHERE NoID=(select NoID from vnk_UserNoID where UserID=mus.UserID and ExamPlanTimeID=
(select TOP 1 ExamPlanTimeID from vnk_ExamPlanTime where IndependentClassID=icu.IndependentClassID))),1) AS EXAM,
ROUND(mus.ScoreFinal,1) ScoreFinal,ROUND((cast(fd.d4 as FLOAT)),1) AS D4,
fd.XH,fd.SymbolName
from ModulesUserScore mus
left join vnk_User u on u.UserID=mus.UserID
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
                return new List<ExamResult>();
            }
        }
        public List<ExamByClass> GetExamByClass(int IndependentClassID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ExamByClass> obj = sqlconnection.Query<ExamByClass>(@"select m.ModulesID,m.ModulesName,ic.ClassCode,m.Credits,ic.ClassName,u.UserID,u.Usercode,(Lastname+' '+Firstname)as Fullname,ROUND(mus.Score1,1) Score1,ROUND(mus.Score2,1) Score2 from IndependentClass ic
left join vnk_IndependentClassUser icu on icu.IndependentClassID=ic.IndependentClassID
left join ModulesUserScore mus on mus.ModulesID=ic.ModulesID and mus.UserID=icu.UserID
left join vnk_User u on u.UserID=mus.UserID
left join Modules m on m.ModulesID=ic.ModulesID
where ic.IndependentClassID=@IndependentClassID",
                new { @IndependentClassID = IndependentClassID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<ExamByClass>();
            }
        }
        public List<ExamCalendar> GetExamCalendar(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<ExamCalendar> obj = sqlconnection.Query<ExamCalendar>(@"select d.DepartmentName,ic.ClassName,ic.ClassCode,ept.CreatedTime,
(select CampusName from vnk_Campus where CampusID=
(select CampusID from Room r where ExamZoomID=
(select top 1 ExamZoomID from RoomExam where DateExam=ept.DateExam and ExamTime=ept.ExamTime and Active=1)))
as Campus,(select ExamZoomName from Room r where 
ExamZoomID=(select top 1 ExamZoomID from RoomExam where DateExam=ept.DateExam and ExamTime=ept.ExamTime 
and Active=1))as Room,(select top 1 RegisterID from IndependentClassUserRegister where
IndependentClassID=icu.IndependentClassID and UserID=icu.UserID) as RegisterID,m.ModulesName,ept.DateExam,et.ExamTimeName 
from vnk_IndependentClassUser icu
join vnk_ExamPlanTime ept on ept.IndependentClassID=icu.IndependentClassID AND ept.CreatedTime = 
(Select Min(CreatedTime) from vnk_ExamPlanTime as B2 where B2.IndependentClassID=icu.IndependentClassID)
join vnk_ExamTime et on et.ExamTimeID=ept.ExamTime
left join IndependentClass ic on ic.IndependentClassID=icu.IndependentClassID
left join Modules m on m.ModulesID=icu.ModulesID
left join vnk_Department d on d.DepartmentID=ic.DepartmentID
where icu.UserID=@UserID
",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<ExamCalendar>();
            }
        }
        public List<TeachCalendar> GetTeachCalendar(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TeachCalendar> obj = sqlconnection.Query<TeachCalendar>(@"select (select Phone from vnk_User where UserID=
(select top 1 UserID from IndependentClassTeacher where IndependentClassID=icu.IndependentClassID)) as teacherphone,
(select top 1 min(StudyDate) from RoomStudy where IndependentClassID=icu.IndependentClassID) minDay,
(select top 1 max(StudyDate) from RoomStudy where IndependentClassID=icu.IndependentClassID) maxDay,
(select Lastname+' '+Firstname from vnk_User where UserID=
(select top 1 UserID from IndependentClassTeacher where IndependentClassID=icu.IndependentClassID)) as teachername,
icu.IndependentClassID,('HP'+m.ModulesCode) as MHP,m.ModulesName,m.Credits from vnk_IndependentClassUser icu
left join Modules m on m.ModulesID=icu.ModulesID
where icu.UserID=@UserID",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<TeachCalendar>();
            }
        }
        public List<TeachCalendarDetail> GetTeachCalendarDetail(int IndependentClassID, int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TeachCalendarDetail> obj = sqlconnection.Query<TeachCalendarDetail>(@"select (select (Lastname+' '+Firstname) from vnk_User where UserID=(select top 1 UserID from IndependentClassTeacher where 
IndependentClassID=ic.IndependentClassID))teachername,m.ModulesName,ictp.Day,ictp.Contents,et.Description,ictp.HaveTest from vnk_IndependentClassUser icu 
left join IndependentClassTimesPlan ictp on ictp.IndependentClassID=icu.IndependentClassID and isnull(ictp.Del,0)=0
left join vnk_ExamTime et on et.ExamTimeID= ictp.TimesInday
left join IndependentClass ic on ic.IndependentClassID=icu.IndependentClassID
left join Modules m on m.ModulesID=ic.ModulesID
where icu.UserID=@UserID and icu.IndependentClassID=@IndependentClassID",
                new { @IndependentClassID = IndependentClassID, @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<TeachCalendarDetail>();
            }
        }
        public List<TBCHKModel> GetTBCHK(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TBCHKModel> obj = sqlconnection.Query<TBCHKModel>(@"select Semester,TBCHK from UserMarkSemester
where UserID=@UserID
",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<TBCHKModel>();
            }
        }
        public List<TKB> GetTKB(int UserID, string aDate, string eDate)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<TKB> obj = sqlconnection.Query<TKB>(@"select IC.IndependentClassID,ic.termid,c.CampusName,r.RoomName,d.DepartmentNameSort,u.Phone,(u.Lastname+' '+u.Firstname) as teachername,rs.TimesInDay,rs.DayStudy,rs.StudyDate,string_Agg(rs.StudyTime,',')StudyTime,m.ModulesName,ic.ClassCode,ic.ClassName from RoomStudy rs
join vnk_IndependentClassUser icu on icu.IndependentClassID=rs.IndependentClassID
left join vnk_IndependentClass ic on ic.IndependentClassID=rs.IndependentClassID
left join Modules m on m.ModulesID=ic.ModulesID
left join IndependentClassTeacher ict on ict.IndependentClassID=rs.IndependentClassID
and ict.CreatedTime=(select max(CreatedTime) from IndependentClassTeacher where
IndependentClassID=rs.IndependentClassID)
left join vnk_User u on u.UserID=ict.UserID
left join Department d on d.DepartmentID=u.DepartmentID
left join Room r on r.RoomID=rs.RoomID
left join Campus c on c.CampusID= r.CampusID
where icu.UserID=@UserID and (rs.StudyDate>=@aDate or @aDate is null or @aDate='') and (rs.StudyDate<=@eDate or @eDate is null or @eDate='') --rs.IndependentClassID=55674
group by IC.IndependentClassID,ic.termid,c.CampusName,r.RoomName,d.DepartmentNameSort,u.Phone,u.Lastname,u.Firstname,rs.TimesInDay,rs.DayStudy,rs.StudyDate,m.ModulesName,ic.ClassCode,ic.ClassName",
                new { @aDate = aDate, @eDate = eDate, @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<TKB>();
            }
        }
        public List<LogDKHP> GetLogDKHP(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<LogDKHP> obj = sqlconnection.Query<LogDKHP>(@"select icur.OwnerID,(u.Lastname+' '+u.Firstname) as register,m.ModulesName,ic.ClassName,ic.ClassCode,isnull(icur.Del,0) as Del,icur.CreatedTime,icur.RegisterID,s.SemesterName from IndependentClassUserRegister icur
left join IndependentClass ic on ic.IndependentClassID=icur.IndependentClassID
left join Modules m on m.ModulesID=ic.ModulesID
left join Semester s on s.SemesterID=ic.Semester
left join vnk_User u on u.UserID=icur.OwnerID
where icur.UserID=@UserID
order by icur.CreatedTime desc",
                new { @UserID = UserID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<LogDKHP>();
            }
        }
        public List<DKHPByTKB> GetDKHPByTKB(int UserID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<DKHPByTKB> obj = sqlconnection.Query<DKHPByTKB>(@"select tbl.IndependentClassID,(select top 1 RoomName from room where RoomID=
(select top 1 RoomID from RoomStudy where IndependentClassID=tbl.IndependentClassID)) room
,(select CampusName from vnk_Campus where CampusID = (select top 1 CampusID from room where RoomID=
(select top 1 RoomID from RoomStudy where IndependentClassID=tbl.IndependentClassID))) campus,tbl.ClassName,
tbl.DayStudy,tbl.ModulesName,tbl.ModulesId,tbl.TimesInDay,string_agg(tbl.StudyTime,',') timeday
from (select m.ModulesName,m.ModulesId,ic.ClassName, rs.DayStudy, rs.TimesInDay,rs.StudyTime ,ic.IndependentClassID
from vnk_IndependentClassUser icu
left join IndependentClass ic on ic.IndependentClassID=icu.IndependentClassID
left join Modules m on m.ModulesID=ic.ModulesID
left join RoomStudy rs on rs.IndependentClassID=ic.IndependentClassID
where icu.UserID=@UserID and ic.TermID=@TermID 
group by m.ModulesName,m.ModulesId,ic.ClassName, rs.DayStudy, rs.TimesInDay, rs.StudyTime,ic.IndependentClassID) tbl
group by tbl.IndependentClassID,tbl.ClassName,tbl.DayStudy,tbl.ModulesName,tbl.ModulesId,tbl.TimesInDay",
                new { @UserID = UserID, @TermID = _configuration["TERMID"].ToString() }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<DKHPByTKB>();
            }
        }
        public List<IndependentClass> GetICByTKB(int TimesInDay, int DayStudy, int CourseIndustryID, int CourseID)
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<IndependentClass> obj = sqlconnection.Query<IndependentClass>(@"select tbl2.IndependentClassID,tbl2.TimesInDay,tbl2.DayStudy,MIN(tbl2.timeday) AS timeday, max(tbl2.RoomName) roomname,tbl2.SSSV,tbl2.Amount, tbl2.ClassName, tbl2.ClassCode, tbl2.Teachername, tbl2.StartDate, tbl2.Credits from (SELECT tbl.IndependentClassID,tbl.TimesInDay,tbl.DayStudy, STRING_AGG(tbl.StudyTime,',') as timeday,tbl.SSSV,TBL.Amount,TBL.RoomName, TBL.ClassName, TBL.ClassCode, TBL.Teachername, TBL.StartDate, TBL.Credits FROM
(select ic.IndependentClassID,rs.TimesInDay,((select cast(count(*) as varchar(50)) from vnk_IndependentClassUser where IndependentClassID=ic.IndependentClassID)+'/'+cast(ic.MaxStudent as varchar(50))) as SSSV , 
 rs.DayStudy,rs.StudyTime,cafci.Amount,r.RoomName,ic.ClassName,ic.ClassCode, (select (Lastname + ' ' + Firstname) from vnk_User where UserID=ict.UserID) as Teachername,ic.StartDate,m.Credits from vnk_IndependentClass ic
left join vnk_Modules m on m.ModulesID=ic.ModulesID 
left join IndependentClassTeacherChange ictc on ictc.IndependentClassID=ic.IndependentClassID
left JOIN RoomStudy RS ON RS.IndependentClassID=IC.IndependentClassID
left JOIN ROOM R ON R.RoomID=RS.RoomID
left join ChannelAmountFee_CI cafci on cafci.CourseIndustryID=@CourseIndustryID and cafci.ModulesTypeID=m.ModulesTypeID and cafci.ApplicateDate=(select max(ApplicateDate) from ChannelAmountFee_CI where 
CourseIndustryID=@CourseIndustryID and ModulesTypeID=m.ModulesTypeID)
left join IndependentClassTeacher ict on ict.IndependentClassID=ic.IndependentClassID
where  ic.CourseID=@CourseID and ic.termid=@termid and rs.TimesInDay=@TimesInDay and DayStudy=@DayStudy
group by rs.TimesInDay,ic.IndependentClassID,RS.DayStudy,RS.StudyTime,cafci.Amount,R.RoomName,ic.ClassName,ict.UserID,ic.ClassCode,ic.StartDate,m.Credits,ic.MaxStudent) AS TBL
GROUP BY tbl.IndependentClassID,tbl.TimesInDay,tbl.DayStudy,tbl.SSSV,TBL.Amount,TBL.RoomName, TBL.ClassName, TBL.ClassCode, TBL.Teachername, TBL.StartDate, TBL.Credits)TBL2
group by tbl2.IndependentClassID,tbl2.TimesInDay,tbl2.DayStudy,tbl2.SSSV,tbl2.Amount, tbl2.ClassName, tbl2.ClassCode, tbl2.Teachername, tbl2.StartDate, tbl2.Credits",
                new { @TimesInDay = TimesInDay, @DayStudy = DayStudy, @termid = _configuration["TERMID"].ToString(), @CourseIndustryID = CourseIndustryID, @CourseID = CourseID }).ToList();
            if (obj != null)
            {
                return obj;
            }
            else
            {
                return new List<IndependentClass>();
            }
        }
    }
}
#region note
///IndependentClassTeacherChange
///Room%
#endregion