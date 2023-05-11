using endavaRestApi.Data;

namespace endavaRestApi.Repositories
{
    public interface ICSVRepository
    {
        Task VnesiCSV(IFormFile file);
    }

}
