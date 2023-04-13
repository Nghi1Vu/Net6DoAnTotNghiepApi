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
                new { @Password= hash });
            if(obj != null && obj.Count()>0)
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
        public List<StudenClass> GetStudentClass()
        {
            using var sqlconnection = _connectionFactory.CreateConnection();
            List<StudenClass> obj = sqlconnection.Query<StudenClass>(@"select (Lastname+' '+Firstname) Username from ClassUser cr join vnk_User vr on cr.UserID=vr.UserID and cr.ClassID=803",
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
    }
}