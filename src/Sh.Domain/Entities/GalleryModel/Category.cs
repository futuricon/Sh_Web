using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sh.Domain.Entities.Base;
using Sh.Domain.Interfaces;

namespace Sh.Domain.Entities.GalleryModel
{
    public class Category : IEntity<string>, IEqualityComparer<Category>
    {
        public Category() { }

        public Category(string categoryName)
        {
            Name = categoryName.Trim();
        }

        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        [Required]
        [StringLength(40)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public virtual IList<Media> Medias { get; set; }

        public override string ToString() => Name;
        public static implicit operator Category(string categoryName) => new(categoryName);
        public static implicit operator string(Category category) => category.Name;

        public static Category[] ParseCategories(string categoriesString, char separator = ',')
        {
            var categories = categoriesString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var categoriesArray = categories.Select(category => (Category)category).ToArray();
            return categoriesArray;
        }

        public static string ConvertCategoriesToString(IEnumerable<Category> categories, char separator = ' ')
        {
            return string.Join(separator, categories);
        }

        public bool Equals(Category x, Category y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Name.ToLower() == y.Name.ToLower();
        }

        public int GetHashCode(Category obj)
        {
            return (obj.Name != null ? obj.Name.GetHashCode() : 0);
        }
    }
}
