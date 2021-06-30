using System.ComponentModel.DataAnnotations;
using Sh.Domain.Entities.Base;
using Sh.Domain.Interfaces;

namespace Sh.Domain.Entities.TestimonialModel
{
    public class Testimonial : IEntity<string>
    {
        public Testimonial()
        {
            CoverPhotoPath = "/img/person_image.jpg";
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(64)]
        public string CoverPhotoPath { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        [StringLength(30, ErrorMessage = "Characters must be less than 30")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [StringLength(30, ErrorMessage = "Characters must be less than 30")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a positiron")]
        [StringLength(80, ErrorMessage = "Characters must be less than 80")]
        [Display(Name = "Position")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите должность")]
        [StringLength(80, ErrorMessage = "Символов должно быть меньше 80")]
        [Display(Name = "Должность")]
        public string PositionRu { get; set; }

        [Required(ErrorMessage = "Iltimos, mansabni kiriting")]
        [StringLength(80, ErrorMessage = "Belgilar 80 dan kam bo'lishi kerak")]
        [Display(Name = "Mansab")]
        public string PositionUz { get; set; }

        [Required(ErrorMessage = "Please enter a testimonial description")]
        [StringLength(200, ErrorMessage = "Characters must be less than 200")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите описание характеристики")]
        [StringLength(200, ErrorMessage = "Символов должно быть меньше 200")]
        [Display(Name = "Описание")]
        public string DescriptionRu { get; set; }

        [Required(ErrorMessage = "Iltimos, xususiyatning tavsifini kiriting")]
        [StringLength(200, ErrorMessage = "Belgilar 200 dan kam bo'lishi kerak")]
        [Display(Name = "Tavsif")]
        public string DescriptionUz { get; set; }
    }
}
