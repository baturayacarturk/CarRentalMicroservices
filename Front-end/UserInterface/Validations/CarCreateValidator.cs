using FluentValidation;
using UserInterface.Models.CatalogModels;

namespace UserInterface.Validations
{
    public class CarCreateValidator : AbstractValidator<CarCreate>
    {

        public CarCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Araba isim alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Araba açıklama alanı boş olamaz");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori alanı seçiniz");
            RuleFor(x => x.Location).NotEmpty().WithMessage("Lokasyon alanı boş olamaz");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Araba açıklama alanı boş olamaz");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("Lütfen geçerli bir sayı giriniz");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Fiyat alanı boş olamaz").ScalePrecision(2, 6).WithMessage("Hatalı format");
        }
    }
}
