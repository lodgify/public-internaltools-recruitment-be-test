using SuperPanel.App.Data;
using SuperPanel.App.Models;
using SuperPanel.App.Models.Base.Output;
using System.Threading.Tasks;

namespace SuperPanel.App.Service
{
    public class GdprService : IGdprService
    {
        private readonly IApiService _apiservice;
        private readonly IUserRepository _userRepository;
        public GdprService(IApiService apiservice, IUserRepository userRepository)
        {
            _apiservice = apiservice;
            _userRepository = userRepository;
        }

        public async Task<string> GdprOrchestrator(int id)
        {
            var gdprResult = await _apiservice.PutGdpr(id);

            if (gdprResult.Success)
            {
                _userRepository.DeleteUser(id);
            }
            else
            {
                return gdprResult.ErrorMessage;
            }

            return null;
        }
    }
}
