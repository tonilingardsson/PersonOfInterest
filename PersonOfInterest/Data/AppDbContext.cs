using Microsoft.EntityFrameworkCore;
using PersonOfInterest.Models;

namespace PersonOfInterest.Data
{
    public class AppDbContext : DbContext
    {
        // Explain: AppDbContext heritates from DbContext and its options are taken from base options?
        public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
        { 
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<PersonInterest> PersonInterests { get; set; }
        public DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite key for join entity
            modelBuilder.Entity<PersonInterest>()
                .HasKey(pi => new { pi.PersonId, pi.InterestId });

            modelBuilder.Entity<PersonInterest>()
                .HasOne(pi => pi.Person)
                .WithMany(p => p.PersonInterests)
                .HasForeignKey(pi => pi.PersonId);

            modelBuilder.Entity<PersonInterest>()
                .HasOne(pi => pi.Interest)
                .WithMany(i => i.PersonInterests)
                .HasForeignKey(pi => pi.InterestId);

            // Seed data for testing purposes
            // Creating two persons
            modelBuilder.Entity<Person>()
                .HasData(
                new Person { Id = 1, Name = "Antonio", PhoneNumber = "070-1111111" },
                new Person { Id = 2, Name = "Moa", PhoneNumber = "070-2222222" }
                );

            // Creating three Interests
            modelBuilder.Entity<Interest>()
                .HasData(
                new Interest { Id = 1, Title = "Music", Description = "Listening and playing music" },
                new Interest { Id = 2, Title = "Coding", Description = "Programming and learning new tech" },
                new Interest { Id = 3, Title = "Reading", Description = "Reading novels and poetry" }
                );

            // Connecting person1 and person2 to some interests
            modelBuilder.Entity<PersonInterest>().HasData(
                new PersonInterest { PersonId = 1, InterestId = 1},
                new PersonInterest { PersonId = 1, InterestId = 2},
                new PersonInterest { PersonId = 1, InterestId = 3},
                new PersonInterest { PersonId = 2, InterestId = 3}
                );

            modelBuilder.Entity<Link>().HasData(
                new Link { Id = 1, Url = "https://spotify.com", PersonId = 1, InterestId = 1 },
                new Link { Id = 2, Url = "https://github.com", PersonId = 1, InterestId = 2 },
                new Link { Id = 3, Url = "https://bokus.se", PersonId = 1, InterestId = 3 },
                new Link { Id = 4, Url = "https://bokus.se", PersonId = 2, InterestId = 3 }               
                );
        }
    }
}