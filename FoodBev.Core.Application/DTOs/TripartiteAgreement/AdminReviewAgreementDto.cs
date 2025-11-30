namespace FoodBev.Application.DTOs.TripartiteAgreement
{
    /// <summary>
    /// DTO for admin to review and approve/reject an agreement.
    /// </summary>
    public class AdminReviewAgreementDto
    {
        public int AgreementID { get; set; }
        public bool Approved { get; set; }
        public string? Notes { get; set; }
    }
}

