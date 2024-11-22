using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contract_Monthly_Claim_System_POE.Models
{
    public class Approval
    {
        [Key] // Specifies this as the primary key
        public int ApprovalID { get; set; }

        [Required] // Ensures that ClaimID is mandatory
        public int ClaimID { get; set; }  // Foreign Key to the Claim model

        [Required]
        [StringLength(20, ErrorMessage = "Coordinator approval status cannot exceed 20 characters.")]
        public string CoordinatorApprovalStatus { get; set; } // Approved, Pending, Rejected

        [Required]
        [StringLength(20, ErrorMessage = "Manager approval status cannot exceed 20 characters.")]
        public string ManagerApprovalStatus { get; set; } // Approved, Pending, Rejected

        [StringLength(500)] // Optional, reason for rejection
        public string RejectionReason { get; set; }

        [StringLength(1000)] // Optional, any comments or notes about the approval process
        public string Comments { get; set; }

        [Required]
        public DateTime SubmissionDate { get; set; } // When the claim was submitted for approval

        public DateTime? CoordinatorApprovalDate { get; set; } // When the coordinator approved/rejected
        public DateTime? ManagerApprovalDate { get; set; } // When the manager approved/rejected

        [StringLength(100)]
        public string ApprovedBy { get; set; } // Name of the person who approved the claim (manager)

        // Foreign Key reference to the Claim
        [ForeignKey("ClaimID")]
        public Claim Claim { get; set; }

        // New calculated property for overall approval status
        [NotMapped] // Not stored in the database
        public string OverallApprovalStatus
        {
            get
            {
                if (CoordinatorApprovalStatus == "Rejected" || ManagerApprovalStatus == "Rejected")
                    return "Rejected";
                if (CoordinatorApprovalStatus == "Pending" || ManagerApprovalStatus == "Pending")
                    return "Pending";
                return "Approved";
            }
        }
    }
}
