using ES.Model;
using Microsoft.EntityFrameworkCore;

namespace ES.DataAccess
{
    public class PersonContext: DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options) : base(options) { }

        public DbSet<PersonDbDetails> PersonDbDetails { get; set; }

    }
}