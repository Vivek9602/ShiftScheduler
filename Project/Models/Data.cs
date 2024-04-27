using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class Location
    { 
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Assignment
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [JsonIgnore]
        public Employee Employee { get; set; }

        public int LocationId { get; set; }

        [JsonIgnore]
        public Location Location { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }


    public class ShiftDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Assignment> Assignments { get; set; }

        public ShiftDbContext(DbContextOptions<ShiftDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(new Employee { Id = 1, Name = "John Doe" });
            modelBuilder.Entity<Employee>().HasData(new Employee { Id = 2, Name = "Jane Smith" });
            modelBuilder.Entity<Employee>().HasData(new Employee { Id = 3, Name = "Michael Lee" });
            modelBuilder.Entity<Employee>().HasData(new Employee { Id = 4, Name = "Person A" });
            modelBuilder.Entity<Employee>().HasData(new Employee { Id = 5, Name = "Person B" });
            modelBuilder.Entity<Employee>().HasData(new Employee { Id = 6, Name = "Person C" });

            modelBuilder.Entity<Location>().HasData(new Location { Id = 1, Name = "Site A" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 2, Name = "Site B" });
            modelBuilder.Entity<Location>().HasData(new Location { Id = 3, Name = "Site C" });

        }
    }
}
