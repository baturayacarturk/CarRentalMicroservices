namespace CarRental.Services.Discount.Model
{
    [Dapper.Contrib.Extensions.Table("discount")]//postgre içindeki tabloya oto maple
    public class Discount
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        
    }
}
