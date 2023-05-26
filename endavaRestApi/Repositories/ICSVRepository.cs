using endavaRestApi.Data;

namespace endavaRestApi.Repositories
{
    public interface ICSVRepository
    {
        Task ImportCsv(IFormFile file);
        Task<object> GetMatchingPaymentDetailsAsync(int userId, string productName);
    }

}
