using System.Threading.Tasks;

namespace FoodBev.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for file storage operations.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Uploads a file to storage and returns the URL.
        /// </summary>
        Task<string> UploadFileAsync(string folder, byte[] fileBytes, string fileName);

        /// <summary>
        /// Deletes a file from storage.
        /// </summary>
        Task<bool> DeleteFileAsync(string fileUrl);
    }
}

