using endavaRestApi.Data;

namespace endavaRestApi.Repositories
{
    public interface ICSVRepository
    {
        Task ImportCsv(IFormFile file);
    }

}
