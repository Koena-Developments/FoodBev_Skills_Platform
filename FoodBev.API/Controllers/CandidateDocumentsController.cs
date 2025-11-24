using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodBev.API.Controllers
{
    [Route("api/v1/candidate/documents")]
    [ApiController]
    [Authorize(Roles = "Candidate")]
    public class CandidateDocumentsController : ControllerBase
    {
        // TODO: Implement IDocumentService when needed
        // private readonly IDocumentService _documentService;

        public CandidateDocumentsController()
        {
            // _documentService = documentService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            // TODO: Implement document service
            return StatusCode(501, "Document service not yet implemented");
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            // TODO: Implement document service
            return StatusCode(501, "Document service not yet implemented");
        }

        [HttpDelete("{docId}")]
        public async Task<IActionResult> DeleteDocument(string docId)
        {
            // TODO: Implement document service
            return StatusCode(501, "Document service not yet implemented");
        }
    }
}