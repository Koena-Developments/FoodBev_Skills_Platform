using FoodBev.Application.DTOs.TripartiteAgreement;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Linq;

namespace FoodBev.API.Services
{
    /// <summary>
    /// Service for generating PDF documents from form data.
    /// </summary>
    public class PdfGenerationService
    {
        public byte[] GenerateFormPdf(CompleteFormDetailsDto formDetails)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(20);

                            // Header
                            column.Item().AlignCenter().Text("Tripartite Agreement Form")
                                .FontSize(20)
                                .Bold()
                                .FontColor(Colors.Blue.Darken2);

                            column.Item().PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                            // Agreement Information Section
                            column.Item().Text("Agreement Information").FontSize(14).Bold();
                            column.Item().PaddingLeft(10).Column(col =>
                            {
                                col.Item().Text($"Agreement ID: {formDetails.AgreementID}");
                                col.Item().Text($"Application ID: {formDetails.ApplicationID}");
                                col.Item().Text($"Status: {formDetails.Status}");
                                col.Item().Text($"Created Date: {formDetails.CreatedDate:yyyy-MM-dd HH:mm}");
                                if (formDetails.SubmittedToAdminDate.HasValue)
                                    col.Item().Text($"Submitted to Admin: {formDetails.SubmittedToAdminDate.Value:yyyy-MM-dd HH:mm}");
                            });

                            column.Item().PaddingVertical(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                            // Candidate Details Section
                            column.Item().Text("Candidate Details").FontSize(14).Bold();
                            column.Item().PaddingLeft(10).Column(col =>
                            {
                                col.Item().Text($"Name: {formDetails.CandidateFirstName} {formDetails.CandidateLastName}");
                                col.Item().Text($"ID Number: {formDetails.CandidateIDNumber ?? "N/A"}");
                                if (formDetails.CandidateDateOfBirth.HasValue)
                                    col.Item().Text($"Date of Birth: {formDetails.CandidateDateOfBirth.Value:yyyy-MM-dd}");
                                col.Item().Text($"Email: {formDetails.CandidateEmail ?? "N/A"}");
                                col.Item().Text($"Contact Number: {formDetails.CandidateContactNumber ?? "N/A"}");
                                col.Item().Text($"Physical Address: {formDetails.CandidatePhysicalAddress ?? "N/A"}");
                                col.Item().Text($"Postal Code: {formDetails.CandidatePostalCode ?? "N/A"}");
                                col.Item().Text($"Province: {formDetails.CandidateProvince ?? "N/A"}");
                                col.Item().Text($"Race: {formDetails.CandidateRace ?? "N/A"}");
                                col.Item().Text($"Gender: {formDetails.CandidateGender ?? "N/A"}");
                                col.Item().Text($"Nationality: {formDetails.CandidateNationality ?? "N/A"}");
                                col.Item().Text($"Employment Status: {formDetails.CandidateEmploymentStatus ?? "N/A"}");
                                col.Item().Text($"OFO Code: {formDetails.CandidateOFO_Code ?? "N/A"}");
                                col.Item().Text($"Highest Qualification: {formDetails.CandidateHighestQualification ?? "N/A"}");
                                if (!string.IsNullOrEmpty(formDetails.CandidateInstitutionName))
                                    col.Item().Text($"Institution: {formDetails.CandidateInstitutionName}");
                                if (formDetails.CandidateQualificationYear.HasValue)
                                    col.Item().Text($"Qualification Year: {formDetails.CandidateQualificationYear}");
                                if (formDetails.CandidateIsDisabled)
                                    col.Item().Text($"Disability: Yes - {formDetails.CandidateDisabilityDetails ?? "Details not provided"}");
                            });

                            column.Item().PaddingVertical(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                            // Employer Details Section
                            column.Item().Text("Employer Details").FontSize(14).Bold();
                            column.Item().PaddingLeft(10).Column(col =>
                            {
                                col.Item().Text($"Company Name: {formDetails.EmployerCompanyName}");
                                if (!string.IsNullOrEmpty(formDetails.EmployerLevyNumber))
                                    col.Item().Text($"Levy Number: {formDetails.EmployerLevyNumber}");
                                if (!string.IsNullOrEmpty(formDetails.EmployerLNumber))
                                    col.Item().Text($"L Number: {formDetails.EmployerLNumber}");
                                if (!string.IsNullOrEmpty(formDetails.EmployerTNumber))
                                    col.Item().Text($"T Number: {formDetails.EmployerTNumber}");
                                if (!string.IsNullOrEmpty(formDetails.EmployerSDFName))
                                {
                                    col.Item().Text($"SDF Name: {formDetails.EmployerSDFName}");
                                    col.Item().Text($"SDF Email: {formDetails.EmployerSDFEmail ?? "N/A"}");
                                    col.Item().Text($"SDF Contact: {formDetails.EmployerSDFContactNumber ?? "N/A"}");
                                }
                            });

                            column.Item().PaddingVertical(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                            // Job Information Section
                            column.Item().Text("Job Information").FontSize(14).Bold();
                            column.Item().PaddingLeft(10).Column(col =>
                            {
                                col.Item().Text($"Job Title: {formDetails.JobTitle}");
                                if (!string.IsNullOrEmpty(formDetails.JobDescription))
                                    col.Item().Text($"Description: {formDetails.JobDescription}");
                                if (!string.IsNullOrEmpty(formDetails.OFO_Code_Required))
                                    col.Item().Text($"Required OFO Code: {formDetails.OFO_Code_Required}");
                            });

                            column.Item().PaddingVertical(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                            // Application Details Section
                            column.Item().Text("Application Details").FontSize(14).Bold();
                            column.Item().PaddingLeft(10).Column(col =>
                            {
                                col.Item().Text($"Date Applied: {formDetails.ApplicationDateApplied:yyyy-MM-dd HH:mm}");
                                col.Item().Text($"Status: {formDetails.ApplicationStatus}");
                                if (formDetails.ApplicationInterviewDate.HasValue)
                                {
                                    col.Item().Text($"Interview Date: {formDetails.ApplicationInterviewDate.Value:yyyy-MM-dd HH:mm}");
                                    col.Item().Text($"Interview Venue: {formDetails.ApplicationInterviewVenue ?? "N/A"}");
                                }
                            });

                            column.Item().PaddingVertical(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);

                            // Signatures Section
                            column.Item().Text("Signatures").FontSize(14).Bold();
                            column.Item().PaddingLeft(10).Column(col =>
                            {
                                // Candidate Signature
                                if (!string.IsNullOrEmpty(formDetails.CandidateSignature))
                                {
                                    col.Item().Text("Candidate Signature:").Bold();
                                    try
                                    {
                                        var candidateSigBytes = ConvertBase64ToBytes(formDetails.CandidateSignature);
                                        col.Item().PaddingTop(5)
                                            .Width(200)
                                            .Height(80)
                                            .Image(candidateSigBytes);
                                    }
                                    catch
                                    {
                                        col.Item().PaddingTop(5).Text("Signature image could not be displayed");
                                    }
                                    if (formDetails.CandidateSignedDate.HasValue)
                                        col.Item().Text($"Signed: {formDetails.CandidateSignedDate.Value:yyyy-MM-dd HH:mm}");
                                }
                                else
                                {
                                    col.Item().Text("Candidate Signature: Not Signed");
                                }

                                col.Item().PaddingTop(10);

                                // Employer Signature
                                if (!string.IsNullOrEmpty(formDetails.EmployerSignature))
                                {
                                    col.Item().Text("Employer Signature:").Bold();
                                    try
                                    {
                                        var employerSigBytes = ConvertBase64ToBytes(formDetails.EmployerSignature);
                                        col.Item().PaddingTop(5)
                                            .Width(200)
                                            .Height(80)
                                            .Image(employerSigBytes);
                                    }
                                    catch
                                    {
                                        col.Item().PaddingTop(5).Text("Signature image could not be displayed");
                                    }
                                    if (formDetails.EmployerSignedDate.HasValue)
                                        col.Item().Text($"Signed: {formDetails.EmployerSignedDate.Value:yyyy-MM-dd HH:mm}");
                                }
                                else
                                {
                                    col.Item().Text("Employer Signature: Not Signed");
                                }

                                col.Item().PaddingTop(10);

                                // Training Provider Signature
                                if (!string.IsNullOrEmpty(formDetails.TrainingProviderSignatureFileRef))
                                {
                                    col.Item().Text("Training Provider Signature: Uploaded").Bold();
                                    col.Item().Text($"File Reference: {formDetails.TrainingProviderSignatureFileRef}");
                                    if (formDetails.TrainingProviderSignatureUploadDate.HasValue)
                                        col.Item().Text($"Uploaded: {formDetails.TrainingProviderSignatureUploadDate.Value:yyyy-MM-dd HH:mm}");
                                }
                                else
                                {
                                    col.Item().Text("Training Provider Signature: Not Uploaded");
                                }
                            });

                            // Admin Notes if available
                            if (!string.IsNullOrEmpty(formDetails.AdminNotes))
                            {
                                column.Item().PaddingVertical(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
                                column.Item().Text("Admin Notes").FontSize(14).Bold();
                                column.Item().PaddingLeft(10).Text(formDetails.AdminNotes);
                            }
                        });
                });
            })
            .GeneratePdf();
        }

        private byte[] ConvertBase64ToBytes(string base64String)
        {
            // Remove data URL prefix if present
            var base64 = base64String;
            if (base64String.Contains(","))
            {
                base64 = base64String.Split(',')[1];
            }

            return Convert.FromBase64String(base64);
        }
    }
}

