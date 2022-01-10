using Microsoft.EntityFrameworkCore;
using SuperPanel.API.Enums;
using SuperPanel.API.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperPanel.API.Infraestructure.Repositories
{
    public class DeletionRequestRepository : GenericRepository<DeletionRequest>, IDeletionRequestRepository
    {
        public DeletionRequestRepository(SuperPanelContext context) : base(context)
        {
        }
    }
}
