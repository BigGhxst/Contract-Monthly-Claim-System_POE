using Microsoft.EntityFrameworkCore;
using Contract_Monthly_Claim_System_POE.Models;

namespace Contract_Monthly_Claim_System_POE.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        /// Represents the Claims table in the database. <summary>
        /// Represents the Claims table in the database.
    
        public DbSet<Claim> Claims { get; set; }

        /// Represents the Lecturers table in the database.
        /// Each Lecturer entity corresponds to a record in this table
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Approval> Approvals { get; set; } // Include your Approval entity
    }
}
