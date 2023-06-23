using CarRental.Shared.Dtos;
using Dapper;
using Npgsql;
using System.Data;

namespace CarRental.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _connection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<ResponseDto<NoContent>> Delete(int id)
        {
            var status = await _connection.ExecuteAsync("delete from discount where id=@id", new { Id = id });
            return status > 0 ? ResponseDto<NoContent>.Success(204) : ResponseDto<NoContent>.Fail("İndirim bulunamadı", 404);
        }

        public async Task<ResponseDto<List<Model.Discount>>> GetAll()
        {
            var discounts = await _connection.QueryAsync<Model.Discount>("Select * from discount");
            return ResponseDto<List<Model.Discount>>.Success(discounts.ToList(), 200);
        }

        public async Task<ResponseDto<Model.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = await _connection.QueryAsync<Model.Discount>("select * from discount where userid=@UserId and code = @Code", new { UserId = userId, Code = code });
            var hasDiscount = discount.FirstOrDefault();
            return hasDiscount == null ? ResponseDto<Model.Discount>.Fail("İndirim bulunamadı", 404) : ResponseDto<Model.Discount>.Success(hasDiscount, 200);
        }

        public async Task<ResponseDto<Model.Discount>> GetById(int id)
        {
            var discount = (await _connection.QueryAsync<Model.Discount>("select * from discount where id=@Id", new { id })).SingleOrDefault();
            if (discount == null)
            {
                return ResponseDto<Model.Discount>.Fail("Indirim bulunamadı", 404);
            }
            return ResponseDto<Model.Discount>.Success(discount, 200);

        }

        public async Task<ResponseDto<NoContent>> Save(Model.Discount discount)
        {
            var status = await _connection.ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES(@UserId,@Rate,@Code)", discount);
            if (status > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            return ResponseDto<NoContent>.Fail("Kaydedilirken hata meydana geldi", 500);
        }

        public async Task<ResponseDto<NoContent>> Update(Model.Discount discount)
        {
            var status = await _connection.ExecuteAsync("update discount set userid=@UserId,code=@Code,rate=@Rate wehere id=@Id", new
            {
                Id = discount.Id,
                UserId = discount.UserId,
                Code = discount.Code,
                Rate = discount.Rate

            });
            if (status > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            return ResponseDto<NoContent>.Fail("İndirim bulunamadı", 404);
        }
    }

}
