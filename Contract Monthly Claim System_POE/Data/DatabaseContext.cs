using Microsoft.EntityFrameworkCore;
using Contract_Monthly_Claim_System_POE.Models;

namespace Contract_Monthly_Claim_System_POE.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        /// Represents the Claims table in the database.
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Approval> Approvals { get; set; } // Include your Approval entity
    }
}
