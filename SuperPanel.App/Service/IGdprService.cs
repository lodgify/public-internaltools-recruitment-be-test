using SuperPanel.App.Models;
using SuperPanel.App.Models.Base.Output;
using System.Threading.Tasks;

namespace SuperPanel.App.Service
{
    public interface IGdprService
    {
        Task<string> GdprOrchestrator(int id);
    }
}
