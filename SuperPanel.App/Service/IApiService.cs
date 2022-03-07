using SuperPanel.App.Models.DTO;
using SuperPanel.App.Models.DTO.Base;
using System.Threading.Tasks;

namespace SuperPanel.App.Service
{
    public interface IApiService
    {
        Task<SingleQueryResult<GdprCommandResult>> PutGdpr(int id);
    }
}
