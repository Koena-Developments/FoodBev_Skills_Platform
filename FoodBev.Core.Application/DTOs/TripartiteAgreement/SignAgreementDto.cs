namespace FoodBev.Application.DTOs.TripartiteAgreement
{
    /// <summary>
    /// DTO for submitting a digital signature on a Tripartite Agreement.
    /// </summary>
    public class SignAgreementDto
    {
        public int AgreementID { get; set; }
        public string SignatureBase64 { get; set; } = string.Empty; // Base64 encoded PNG image
    }
}

