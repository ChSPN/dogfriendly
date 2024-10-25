
namespace DogFriendly.Domain.Repositories
{
    /// <summary>
    /// Interface for file storage repository.
    /// </summary>
    public interface IFileStorageRepository
    {
        /// <summary>
        /// Uploads the file asynchronous.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileContent">Content of the file.</param>
        /// <returns></returns>
        Task<bool> UploadFileAsync(string fileName, byte[] fileContent);

        /// <summary>
        /// Removes the file asynchronous.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        Task<bool> RemoveFileAsync(string fileName);
    }
}