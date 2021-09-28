using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using iread_assignment_ms.Web.Service;
using Microsoft.AspNetCore.Http;
using iread_assignment_ms.DataAccess.Data.Entity;
using System.Linq;
using iread_assignment_ms.Web.Util;
using Microsoft.AspNetCore.Authorization;
using iread_assignment_ms.Web.Dto.EssayQuestion;
using iread_assignment_ms.Web.Dto.MultiChoice;
using System;
using iread_assignment_ms.Web.Dto.Interaction;

namespace iread_assignment_ms.Web.Controller
{
    [ApiController]
    [Route("api/Assignment/")]
    public class SubmitController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AssignmentService _assignmentService;
        private readonly EssayAnswerService _essayAnswerService;
        private readonly MultiChoiceAnswerService _multiChoiceAnswerService;
        private readonly InteractionAnswerService _interactionAnswerService;
        private readonly MultiChoiceService _multiChoiceService;
        private readonly EssayQuestionService _essayQuestionService;
        private readonly InteractionQuestionService _interactionQuestionService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public SubmitController(AssignmentService assignmentService,
        MultiChoiceAnswerService multiChoiceAnswerService,
        EssayQuestionService essayQuestionService,
        EssayAnswerService essayAnswerService,
        MultiChoiceService multiChoiceService,
        InteractionAnswerService interactionAnswerService,
        InteractionQuestionService interactionQuestionService,
         IMapper mapper, IConsulHttpClientService consulHttpClient)
        {
            _assignmentService = assignmentService;
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
            _multiChoiceAnswerService = multiChoiceAnswerService;
            _essayQuestionService = essayQuestionService;
            _multiChoiceService = multiChoiceService;
            _interactionQuestionService = interactionQuestionService;
            _essayAnswerService = essayAnswerService;
            _interactionAnswerService = interactionAnswerService;
        }





        //POST: api/Assignment/3/submit
        [HttpPut("{assignmentId}/submit")]
        [Authorize(Roles = Policies.Student, AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Submit([FromRoute] int assignmentId)
        {
            Assignment assignment = _assignmentService.GetById(assignmentId).GetAwaiter().GetResult();

            if (assignment == null)
            {
                return NotFound();
            }
            // check if mine
            string myId = User.Claims.Where(c => c.Type == "sub")
             .Select(c => c.Value).SingleOrDefault();


            if (!_assignmentService.IsMine(assignment, myId))
            {
                ModelState.AddModelError("Assignment", "Assignment is not yours");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            _assignmentService.SubmitAnswers(assignment, myId);

            return NoContent();
        }





    }
}