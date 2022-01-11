using API_Proyecto2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Proyecto2.Data
{
   public class DbContextChild : DbContext
   {
      public DbContextChild(DbContextOptions<DbContextChild> options) : base(options)
      {

      }
      public DbSet<Client> Clients { get; set; }
      public DbSet<Project> Projects { get; set; }

      public DbSet<ProjectImage> Images { get; set; }
   }
}
