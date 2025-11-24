using FoodBev.Application.DTOs.ProfileManagement;
using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    public interface ICandidateService
    {
        Task<CandidateProfileDto> GetCandidateProfileByIdAsync(int candidateId);
        Task<CandidateProfileDto> GetCandidateProfileByUserIdAsync(string userId);
        Task<CandidateProfileDto> UpdateCandidateProfileAsync(int candidateId, UpdateCandidateProfileDto dto);
        Task<int?> GetCandidateIdByUserIdAsync(string userId);
    }
}