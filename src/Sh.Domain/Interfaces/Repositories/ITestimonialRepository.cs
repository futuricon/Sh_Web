using Sh.Domain.Entities.TestimonialModel;
using System.Threading.Tasks;

namespace Sh.Domain.Interfaces.Repositories
{
    public interface ITestimonialRepository : IRepository
    {
        Task AddTestimonialAsync(Testimonial testimonial);
        Task UpdateTestimonialAsync(Testimonial testimonial);
        Task DeleteTestimonialAsync(Testimonial testimonial);
    }
}
