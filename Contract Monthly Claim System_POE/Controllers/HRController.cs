using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contract_Monthly_Claim_System_POE.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Contract_Monthly_Claim_System_POE.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public class HRController : Controller
{
    private string connectionString = "server=localhost;database=claimsystem;uid=root;password=FORTUNE@123;";
    private readonly DatabaseContext _context;

    public HRController(DatabaseContext context)
    {
        _context = context;
    }

    // Display all lecturers
    public async Task<IActionResult> Lecturers()
    {
        var lecturers = await _context.Lecturers.ToListAsync();
        return View(lecturers);
    }

    // Edit Lecturer
    public async Task<IActionResult> EditLecturer(int id)
    {
        var lecturer = await _context.Lecturers.FindAsync(id);
        if (lecturer == null)
            return NotFound();

        return View(lecturer);
    }

    [HttpPost]
    public async Task<IActionResult> EditLecturer(Lecturer lecturer)
    {
        if (ModelState.IsValid)
        {
            _context.Lecturers.Update(lecturer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Lecturers");
        }
        return View(lecturer);
    }

    // Delete Lecturer
    [HttpPost]
    public async Task<IActionResult> DeleteLecturer(int id)
    {
        var lecturer = await _context.Lecturers.FindAsync(id);
        if (lecturer != null)
        {
            _context.Lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Lecturers");
    }

    // List approved claims on the HR Dashboard
    public IActionResult Index()
    {
        var approvedClaims = _context.Claims
            .Include(c => c.Lecturer)
            .Where(c => c.Status == "Approved")
            .ToList();
        return View(approvedClaims);
    }

    // Submit a new claim and save it automatically
    [HttpPost]
    public async Task<IActionResult> SubmitClaim(int lecturerId, int hoursWorked, decimal hourlyRate, string notes)
    {
        var claim = new Claim
        {
            LecturerID = lecturerId,
            Date = DateTime.Now,
            HoursWorked = hoursWorked,
            TotalAmount = hoursWorked * hourlyRate,
            Status = "Pending",
            Notes = notes
        };

        _context.Claims.Add(claim);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // Add or update lecturer details
    public IActionResult AddLecturer()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddLecturer(Lecturer lecturer)
    {
        if (ModelState.IsValid)
        {
            if (lecturer.LecturerID == 0)
            {
                _context.Lecturers.Add(lecturer);
            }
            else
            {
                _context.Lecturers.Update(lecturer);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(lecturer);
    }

    // Display the Invoice page
    public IActionResult Invoices()
    {
        return View();
    }

    // Generate report of approved claims
    [HttpPost]
    public async Task<IActionResult> GenerateReport()
    {
        var approvedClaims = await _context.Claims
            .Include(c => c.Lecturer)
            .Where(c => c.Status == "Approved")
            .ToListAsync();

        if (!approvedClaims.Any())
        {
            TempData["ErrorMessage"] = "No approved claims found to generate a report.";
            return RedirectToAction("Invoices");
        }

        using var stream = new MemoryStream();
        var document = new Document(PageSize.A4, 25, 25, 30, 30);
        PdfWriter.GetInstance(document, stream).CloseStream = false;

        document.Open();
        document.Add(new Paragraph("Approved Claims Invoices"));
        document.Add(new Paragraph(" "));

        foreach (var claim in approvedClaims)
        {
            document.Add(new Paragraph($"Lecturer: {claim.Lecturer.FirstName} {claim.Lecturer.LastName}"));
            document.Add(new Paragraph($"Amount: {claim.TotalAmount:C}"));
            document.Add(new Paragraph($"Date: {claim.Date.ToShortDateString()}"));
            document.Add(new Paragraph(" "));
        }

        document.Close();
        stream.Position = 0;

        return File(stream, "application/pdf", "ApprovedClaimsReport.pdf");
    }

    // Generate invoices for approved claims
    [HttpPost]
    public async Task<IActionResult> GenerateInvoices()
    {
        var approvedClaims = await _context.Claims
            .Include(c => c.Lecturer)
            .Where(c => c.Status == "Approved")
            .ToListAsync();

        if (!approvedClaims.Any())
        {
            TempData["ErrorMessage"] = "No approved claims found to generate invoices.";
            return RedirectToAction("Invoices");
        }

        using var stream = new MemoryStream();
        var document = new Document(PageSize.A4, 25, 25, 30, 30);
        PdfWriter.GetInstance(document, stream).CloseStream = false;

        document.Open();
        document.Add(new Paragraph("Invoices for Approved Claims"));
        document.Add(new Paragraph($"Generated on: {DateTime.Now}"));
        document.Add(new Paragraph(" "));

        foreach (var claim in approvedClaims)
        {
            document.Add(new Paragraph($"Invoice for Claim ID: {claim.ClaimID}"));
            document.Add(new Paragraph($"Lecturer: {claim.Lecturer.FirstName} {claim.Lecturer.LastName}"));
            document.Add(new Paragraph($"Total Amount: {claim.TotalAmount:C}"));
            document.Add(new Paragraph($"Date Approved: {claim.Date.ToShortDateString()}"));
            document.Add(new Paragraph(" "));
        }

        document.Close();
        stream.Position = 0;

        return File(stream, "application/pdf", "ApprovedClaimsInvoices.pdf");
    }
}
