namespace FoodBev.Application.DTOs.TripartiteAgreement
{
    /// <summary>
    /// DTO for employer to submit their signature and upload Training Provider signature file.
    /// Note: File handling is done in the API layer, this DTO receives the file path after upload.
    /// </summary>
    public class SubmitEmployerSignatureDto
    {
        public int AgreementID { get; set; }
        public string EmployerSignatureBase64 { get; set; } = string.Empty; // Employer's digital signature
        public string? TrainingProviderSignatureFileRef { get; set; } // Path to uploaded Training Provider signature file
    }
}

