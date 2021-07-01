using System.ComponentModel.DataAnnotations;
using Sh.Domain.Entities.Base;
using Sh.Domain.Interfaces;

namespace Sh.Domain.Entities.DossierModel
{
    public class Dossier : IEntity<string>
    {
        public Dossier()
        {
            CoverPhotoPath = "/img/person_image.jpg";
            IsAboutInfo = false;
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        public bool IsAboutInfo { get; set; }

        [StringLength(64)]
        public string CoverPhotoPath { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        [StringLength(30, ErrorMessage = "Characters must be less than 30")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите Имя")]
        [StringLength(30, ErrorMessage = "Символов должно быть меньше 30")]
        [Display(Name = "Имя")]
        public string FirstNameRu { get; set; }

        [Required(ErrorMessage = "Iltimos, ismingizni kiriting")]
        [StringLength(30, ErrorMessage = "Belgilar 30 dan kam bo'lishi kerak")]
        [Display(Name = "Ism")]
        public string FirstNameUz { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [StringLength(30, ErrorMessage = "Characters must be less than 30")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите Фамилию")]
        [StringLength(30, ErrorMessage = "Символов должно быть меньше 30")]
        [Display(Name = "Фамилия")]
        public string LastNameRu { get; set; }

        [Required(ErrorMessage = "Iltimos, familiyangizni kiriting")]
        [StringLength(30, ErrorMessage = "Belgilar 30 dan kam bo'lishi kerak")]
        [Display(Name = "Familiya")]
        public string LastNameUz { get; set; }

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

        public string CVFilePath { get; set; }

        [StringLength(20, ErrorMessage = "Characters must be less than 20")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Characters must be less than 400")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [StringLength(30, ErrorMessage = "Characters must be less than 30")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
