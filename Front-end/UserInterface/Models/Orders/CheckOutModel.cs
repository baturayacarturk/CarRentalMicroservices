using System.ComponentModel.DataAnnotations;

namespace UserInterface.Models.Orders
{
    public class CheckOutModel
    {
        [Display(Name = "İl")]
        public string Province { get; set; }
        [Display(Name = "İlçe")]
        public string District { get; set; }
        [Display(Name = "Cadde")]
        public string Street { get; set; }

        public string ZipCode { get; set; }
        [Display(Name = "Adres")]
        public string Line { get; set; }
        [Display(Name = "Kartta yazan adınız ve soyadınız")]
        public string CardName { get; set; }
        [Display(Name = "Kart numarası")]
        public string CardNumber { get; set; }
        [Display(Name = "Son kullanma tarhi ay/yıl")]
        public string Expiration { get; set; }
        [Display(Name = "CVV")]
        public string CVV { get; set; }
        public string TotalPrice { get; set; }
    }
}
