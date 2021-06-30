using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sh.Domain.Entities.Base;
using Sh.Domain.Entities.BlogModel;
using Sh.Domain.Interfaces;

namespace Sh.Domain.Entities.GalleryModel
{
    public enum MediaType
    {
        Video,
        Image
    }

    public class Media : IEntity<string>
    {
        [StringLength(32)]
        public string Id { get; set; } = GeneratorId.GenerateLong();

        public string MediaPath { get; set; }

        public MediaType MediaType { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
