using Microsoft.EntityFrameworkCore;

namespace DogFriendly.Infrastructure.Context
{
    /// <summary>
    /// Database context for DogFriendly application.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class DogFriendlyContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DogFriendlyContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public DogFriendlyContext(DbContextOptions<DogFriendlyContext> options)
            : base(options)
        {
        }

        /// <ineheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
