using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System_POE.Models
{
    public class Lecturer
    {
        [Key] // Specifies this as the primary key
        public int LecturerID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress] // Validates email format
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Hourly Rate must be a positive number.")]
        [DataType(DataType.Currency)]
        public decimal HourlyRate { get; set; } // Lecturer's hourly pay rate

        [StringLength(15)]
        public string PhoneNumber { get; set; } // Optional for contact information

        [StringLength(100)]
        public string Department { get; set; } // Department where the lecturer works

        // New field to track whether the lecturer is active
        public bool IsActive { get; set; } = true;

        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedDate { get; set; }

        // Navigation property for claims
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();

        // New calculated property: Full Name
        [NotMapped] // This property is not stored in the database
        public string FullName => $"{FirstName} {LastName}";
    }
}
