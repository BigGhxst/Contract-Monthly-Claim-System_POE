using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//[Table("Claims")]
namespace Contract_Monthly_Claim_System_POE.Models
{
    public class Claim
    {
        [Key] // Specifies this as the primary key
        public int ClaimID { get; set; }

        [Required] // Ensures LecturerID is not null
        public int LecturerID { get; set; }  // Foreign Key

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, 168, ErrorMessage = "Hours worked must be between 0 and 168.")] // Weekly max hours
        public int HoursWorked { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total Amount must be a positive number.")]
        [Column(TypeName = "decimal(18,2)")] // Defines precision for the database
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)] // Restricts length of the status field
        public string Status { get; set; } // Approved, Pending, Rejected

        [StringLength(255)]
        public string UploadedFileName { get; set; } // Name of uploaded file, if any

        [Required]
        [StringLength(50)] // Verification status (e.g., Verified, Pending)
        public string VerificationStatus { get; set; }

        [StringLength(500)] // Rejection reasons have a limit
        public string RejectionReason { get; set; }

        [StringLength(1000)] // Additional notes about the claim
        public string Notes { get; set; }

        [StringLength(255)]
        public string SupportingDocumentPath { get; set; } // Path to the uploaded file

        // Tracks the user who submitted the claim
        [StringLength(100)]
        public string SubmittedBy { get; set; }

        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Defaults to current time
        public DateTime? LastUpdatedDate { get; set; } // Nullable for updates

        // Navigation property for the Lecturer
        [ForeignKey("LecturerID")]
        public Lecturer Lecturer { get; set; }

        // Optional navigation property for Approval information
        public Approval Approval { get; set; }

        // New calculated property: Total Pay
        [NotMapped] // This property is not stored in the database
        public decimal TotalPay => HoursWorked * (Lecturer?.HourlyRate ?? 0);
    }
}
