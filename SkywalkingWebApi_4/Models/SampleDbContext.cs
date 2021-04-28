using Microsoft.EntityFrameworkCore;

namespace SkyApm.Sample.Backend.Models
{
    public class SampleDbContext :DbContext
    {
        public  DbSet<Application> Applications { get; set; }

        public SampleDbContext(DbContextOptions options):base(options)
        {

        }
    }
}
