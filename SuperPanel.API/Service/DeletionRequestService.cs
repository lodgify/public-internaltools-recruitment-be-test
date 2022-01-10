using Mapster;
using SuperPanel.API.Dto;
using SuperPanel.API.Infraestructure.Repositories;
using SuperPanel.API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperPanel.API.Service
{
    public class DeletionRequestService : GenericService<DeletionRequest, DeletionRequestDto>, IDeletionRequestService
    {
        private readonly IDeletionRequestRepository _deletionRequestRepository;

        public DeletionRequestService(IDeletionRequestRepository deletionRequestRepository) : base(deletionRequestRepository)
        {
            _deletionRequestRepository = deletionRequestRepository;
        }
    }
}
