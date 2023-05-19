using endavaRestApi.Data;

namespace endavaRestApi.Repositories
{
    public interface ICSVRepository
    {
        Task ImportCsv(IFormFile file);

        Task<bool> CheckOrderAsync(string firstName, string lastName, decimal sum, string productName);
    }

}
