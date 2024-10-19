using DogFriendly.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Infrastructure.Context
{
    public class DogFriendlyContextFactory : IDesignTimeDbContextFactory<DogFriendlyContext>
    {
        public DogFriendlyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DogFriendlyContext>();
            optionsBuilder.UseNpgsql("");
            return new DogFriendlyContext(optionsBuilder.Options);
        }
    }
}
