# Contract Monthly Claim System

###  A web based system which allows the Human Resource to deal with the lecturer claims, issue the invoice and produce the report of the approved claims.
This system, developed with the use of ASP.NET Core MVC and MySQL, enables HR to create, modify, and delete lecturers, as well as funkce theoretical claims. 
prepare bank statements in PDF format and also issue the invoices in the same format.

## Features
 Lecturer Management: Add, edit, and delete lecturers.
 Claims Submission: Submit, approve, and reject claims.
Reports: Generate PDF reports for approved claims.
 Invoices: Generate PDF invoices for approved claims.
Dashboard for HR: View all approved claims and manage lecturer information.

##Technologies Used
ASP.NET Core MVC for building the web application
Entity Framework Core for data access
MySQL as the database
iTextSharp for generating PDF reports and invoices

## Usage
### HR Dashboard
loggs in/logout
Lecturer Management: Navigate to /HR/Lecturers to add, edit, or delete lecturers.
Claims Management: Submit new claims, approve, or reject claims. Approved claims are displayed on the HR dashboard.
Generate Reports: Navigate to /HR/Invoices and select Generate Report to download a PDF report of approved claims.
Generate Invoices: Navigate to /HR/Invoices and select Generate Invoices to download a PDF invoice for each approved claim. 

## Controllers:
User login/register: data is kept in the database 
HRController: Handles HR-specific actions like lecturer management, claim approval, report generation, and invoice generation.
Models:
Lecturer: Represents a lecturer in the system.
Claim: Represents a claim submitted by a lecturer.
Views:
HR: Contains views for managing lecturers, generating invoices, and displaying the HR dashboard.

# GitHuub Link
