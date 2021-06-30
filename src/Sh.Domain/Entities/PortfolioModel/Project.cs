using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Sh.Domain.Entities.Base;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Entities.UserModel;
using Sh.Domain.Interfaces;

namespace Sh.Domain.Entities.PortfolioModel
{
    public class Project : ISlugifiedEntity, IEntity<string>
    {
        public Project()
        {
            CoverPhotoPath = "/img/blog_image.jpg";
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [StringLength(80)]
        public string Slug { get; set; }

        [StringLength(64)]
        public string CoverPhotoPath { get; set; }

        public virtual AppUser Author { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter a title for the project")]
        [StringLength(80, ErrorMessage = "Characters must be less than 80")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите заголовок проекта")]
        [StringLength(80, ErrorMessage = "Символов должно быть меньше 80")]
        [Display(Name = "Заголовок")]
        public string TitleRu { get; set; }

        [Required(ErrorMessage = "Iltimos, loyihani sarlavhasini kiriting")]
        [StringLength(80, ErrorMessage = "Belgilar 80 dan kam bo'lishi kerak")]
        [Display(Name = "Sarlavha")]
        public string TitleUz { get; set; }

        [Required(ErrorMessage = "Please enter the content of the project")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите содержание проекта")]
        [Display(Name = "Содержание")]
        public string ContentRu { get; set; }

        [Required(ErrorMessage = "Iltimos, loyihani mazmunini kiriting")]
        [Display(Name = "Mazmuni")]
        public string ContentUz { get; set; }

        public int ViewCount { get; set; }

        public string ProjectFilePath { get; set; }

        public virtual IList<Tag> Tags { get; set; } = new List<Tag>();

        public static string GetShortContent(string articleContent, int length)
        {
            var content = HttpUtility.HtmlDecode(articleContent);
            content = Regex.Replace(content, @"<(.|\n)*?>", "");
            if (content.Length > 500)
            {
                content = content.Substring(0, length).Trim() + "...";
            }

            return content;
        }
    }
}
