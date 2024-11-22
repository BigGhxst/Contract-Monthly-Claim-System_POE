using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Contract_Monthly_Claim_System_POE.Models;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Contract_Monthly_Claim_System_POE.Controllers
{
    public class ClaimsController : Controller
    {
        private string connectionString = "server=localhost;database=claimsystem;uid=root;password=FORTUNE@123;";

        // Static list of lecturers and claims (in-memory storage)
        private static List<Lecturer> lecturers = new List<Lecturer>
        {
            new Lecturer { LecturerID = 1, FirstName = "Fortune", LastName = "Mlilo", Email = "mlilofortune21@gmail.com", HourlyRate = 250 }
        };

        private static List<Claim> claims = new List<Claim>();

        // Predefined criteria for automated verification
        private const int MaxHoursWorked = 160; // Maximum allowable hours per claim
        private const decimal MaxHourlyRate = 350; // Maximum hourly rate allowed

        // GET: Show claim submission form
        public IActionResult SubmitClaim()
        {
            return View();
        }

        // POST: Handle claim submission, including file upload
        [HttpPost]
        public IActionResult SubmitClaim(string firstName, string lastName, string email, int hoursWorked, decimal hourlyRate, string notes, IFormFile supportingDoc)
        {
            // Server-side validation for negative or zero values
            if (hoursWorked <= 0 || hourlyRate <= 0)
            {
                ModelState.AddModelError(string.Empty, "Hours worked and hourly rate must be greater than zero.");
                return View();
            }

            // Calculate total amount
            decimal totalAmount = hoursWorked * hourlyRate;

            // Create a new lecturer if one does not already exist
            var lecturer = lecturers.FirstOrDefault(l => l.Email == email);
            if (lecturer == null)
            {
                lecturer = new Lecturer
                {
                    LecturerID = lecturers.Count + 1,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    HourlyRate = hourlyRate
                };
                lecturers.Add(lecturer);
            }

            // Handle file upload
            string uploadedFileName = null;
            if (supportingDoc != null && supportingDoc.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                uploadedFileName = Path.GetFileName(supportingDoc.FileName);
                var filePath = Path.Combine(uploads, uploadedFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    supportingDoc.CopyTo(stream);
                }
            }

            // Create a new claim object
            var claim = new Claim
            {
                ClaimID = claims.Count + 1,
                LecturerID = lecturer.LecturerID,
                Date = DateTime.Now,
                HoursWorked = hoursWorked,
                TotalAmount = totalAmount,
                Status = "Pending",
                Notes = notes,
                SupportingDocumentPath = uploadedFileName,
                Lecturer = lecturer
            };

            // Add claim to the in-memory list
            claims.Add(claim);

            return RedirectToAction("ClaimSubmitted");
        }

        // GET: Show confirmation page after claim submission
        public IActionResult ClaimSubmitted()
        {
            return View();
        }

        // GET: List all claims
        public IActionResult ListClaims()
        {
            return View(claims);
        }

        // GET: View claims to verify (approve/reject)
        public IActionResult VerifyClaims()
        {
            // Run automated verification checks for all pending claims
            foreach (var claim in claims.Where(c => c.Status == "Pending"))
            {
                claim.VerificationStatus = CheckClaimValidity(claim);
            }

            return View(claims);
        }

        // Automated verification logic
        private string CheckClaimValidity(Claim claim)
        {
            if (claim.HoursWorked > MaxHoursWorked)
                return "Invalid: Exceeds maximum allowed hours.";
            if (claim.Lecturer.HourlyRate > MaxHourlyRate)
                return "Invalid: Exceeds maximum hourly rate.";
            if (string.IsNullOrEmpty(claim.SupportingDocumentPath))
                return "Invalid: Missing supporting documentation.";
            return "Valid";
        }

        // POST: Handle claim verification
        [HttpPost]
        public IActionResult VerifyClaim(int claimID, string action, string rejectionReason)
        {
            var claim = claims.FirstOrDefault(c => c.ClaimID == claimID);
            if (claim == null) return NotFound();

            if (action == "Approve")
            {
                if (claim.VerificationStatus == "Valid")
                {
                    claim.Status = "Approved";
                    claim.RejectionReason = null; // Clear rejection reason if approved
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Cannot approve an invalid claim.");
                }
            }
            else if (action == "Reject")
            {
                claim.Status = "Rejected";
                claim.RejectionReason = rejectionReason;
            }

            return RedirectToAction("VerifyClaims");
        }
    }
}
