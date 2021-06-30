using System.Threading.Tasks;
using Sh.Domain.Entities.TestimonialModel;
using Sh.Domain.Interfaces.Repositories;
using Sh.Infrastructure.Data;

namespace Sh.Infrastructure.Repositories
{
    public class TestimonialRepository : Repository, ITestimonialRepository
    {
        private readonly ApplicationDbContext _context;

        public TestimonialRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task AddTestimonialAsync(Testimonial testimonial)
        {
            return AddAsync(testimonial);
        }

        public async Task DeleteTestimonialAsync(Testimonial testimonial)
        {
            await DeleteAsync(testimonial); 
        }

        public Task UpdateTestimonialAsync(Testimonial testimonial)
        {
            return UpdateAsync(testimonial);
        }
    }
}
