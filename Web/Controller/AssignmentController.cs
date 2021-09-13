using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using iread_assignment_ms.Web.Service;

namespace iread_assignment_ms.Web.Controller
{
    [ApiController]
    [Route("api/Interaction/[controller]/")]
    public class AssignmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AssignmentService _assignmentService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public AssignmentController(AssignmentService assignmentService, IMapper mapper, IConsulHttpClientService consulHttpClient)
        {
            _assignmentService = assignmentService;
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
        }


    }
}