using Contract_Monthly_Claim_System_POE.Controllers;  // Import the Controllers namespace
using Contract_Monthly_Claim_System_POE.Models;       // Import the Models namespace
using Microsoft.AspNetCore.Mvc;                       // Import ASP.NET Core MVC for ActionResult types
using Xunit;                                          // Import Xunit for unit testing
using System.Linq;                                    // Import Linq for working with collections
using Microsoft.AspNetCore.Http;                      // For IFormFile

namespace Contract_Monthly_Claim_System_POE.Tests     // Adjust the namespace to reflect the Test Project
{
    public class ClaimsControllerTests
    {
        // Test case: Valid claim submission should redirect to "ClaimSubmitted"
        [Fact]
        public void SubmitClaim_ValidClaim_RedirectsToClaimSubmitted()
        {
            // Arrange
            var controller = new ClaimsController();   // Initialize the ClaimsController
            var lecturerID = "L001";                  // Sample Lecturer ID
            var lecturerName = "John Doe";            // Sample Lecturer Name
            var subject = "Mathematics";              // Sample Subject
            var hoursWorked = 10;                     // Hours worked
            var hourlyRate = 50m;                     // Hourly rate
            var notes = "Sample claim";               // Notes for the claim
            IFormFile file = null;                    // File (optional, can be null)

            // Act
            var result = controller.SubmitClaim(lecturerID, lecturerName, subject, hoursWorked, hourlyRate, notes, file) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ClaimSubmitted", result.ActionName);
        }

        // Test case: Approving a valid claim should change its status to "Approved"
        [Fact]
        public void ApproveClaim_ValidClaim_ChangesStatusToApproved()
        {
            // Arrange
            var controller = new ClaimsController();
            var lecturerID = "L001";
            var lecturerName = "John Doe";
            var subject = "Mathematics";
            var hoursWorked = 10;
            var hourlyRate = 50m;
            var notes = "Sample claim";
            IFormFile file = null;

            // Simulate submitting the claim (set it as pending)
            controller.SubmitClaim(lecturerID, lecturerName, subject, hoursWorked, hourlyRate, notes, file);

            // Act
            controller.VerifyClaim(1, "Approve", null);  // Pass null as rejectionReason for approval

            // Assert
            var claimsList = controller.ListClaims() as ViewResult;
            var claimsModel = claimsList.Model as List<Claim>;
            var approvedClaim = claimsModel.FirstOrDefault(c => c.ClaimID == 1);

            Assert.NotNull(approvedClaim);
            Assert.Equal("Approved", approvedClaim.Status);
        }

        // Test case: Rejecting a valid claim should change its status to "Rejected"
        [Fact]
        public void RejectClaim_ValidClaim_ChangesStatusToRejected()
        {
            // Arrange
            var controller = new ClaimsController();
            var lecturerID = "L002";
            var lecturerName = "Jane Doe";
            var subject = "Physics";
            var hoursWorked = 8;
            var hourlyRate = 40m;
            var notes = "Another sample claim";
            IFormFile file = null;

            // Simulate submitting the claim (set it as pending)
            controller.SubmitClaim(lecturerID, lecturerName, subject, hoursWorked, hourlyRate, notes, file);

            // Act
            var rejectionReason = "Missing supporting documents";
            controller.VerifyClaim(2, "Reject", rejectionReason);

            // Assert
            var claimsList = controller.ListClaims() as ViewResult;
            var claimsModel = claimsList.Model as List<Claim>;
            var rejectedClaim = claimsModel.FirstOrDefault(c => c.ClaimID == 2);

            Assert.NotNull(rejectedClaim);
            Assert.Equal("Rejected", rejectedClaim.Status);
            Assert.Equal(rejectionReason, rejectedClaim.RejectionReason);
        }
    }
}
