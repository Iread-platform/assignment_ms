using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using iread_assignment_ms.Web.Service;
using Microsoft.AspNetCore.Http;
using iread_assignment_ms.DataAccess.Data.Entity;
using System.Linq;
using iread_assignment_ms.Web.Util;
using Microsoft.AspNetCore.Authorization;
using iread_assignment_ms.Web.Dto.FeedBack;

namespace iread_assignment_ms.Web.Controller
{
    [ApiController]
    [Route("api/Assignment/")]
    public class FeedBackController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly EssayAnswerService _essayAnswerService;
        private readonly MultiChoiceAnswerService _multiChoiceAnswerService;
        private readonly InteractionAnswerService _interactionAnswerService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public FeedBackController(
        MultiChoiceAnswerService multiChoiceAnswerService,
        EssayAnswerService essayAnswerService,
        InteractionAnswerService interactionAnswerService,
         IMapper mapper, IConsulHttpClientService consulHttpClient)
        {
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
            _multiChoiceAnswerService = multiChoiceAnswerService;
            _essayAnswerService = essayAnswerService;
            _interactionAnswerService = interactionAnswerService;
        }


        //POST: api/Assignment/essay-answer/3/feed-back/add
        [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        [HttpPost("essay-answer/{id}/feed-back/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertEssayFeedBack([FromRoute] int id,
        [FromBody] FeedBackCreateDto feedBack)
        {
            // check if the essay answer exist
            EssayAnswer essayAnswerEntity = _essayAnswerService.GetById(id, true).GetAwaiter().GetResult();
            if (essayAnswerEntity == null)
            {
                ModelState.AddModelError("Id", "Essay answer not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            CheckAnswerFeedBack(essayAnswerEntity);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }
            essayAnswerEntity.FeedBackFromTeacher = feedBack.Text;
            _essayAnswerService.Update(essayAnswerEntity);

            return NoContent();
        }

        //POST: api/Assignment/multi-choice-answer/3/feed-back/add
        [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        [HttpPost("multi-choice-answer/{id}/feed-back/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertMultiChoiceFeedBack([FromRoute] int id,
        [FromBody] FeedBackCreateDto feedBack)
        {
            // check if the essay answer exist
            MultiChoiceAnswer multiChoiceAnswerEntity = _multiChoiceAnswerService.GetById(id, true).GetAwaiter().GetResult();
            if (multiChoiceAnswerEntity == null)
            {
                ModelState.AddModelError("Id", "Essay answer not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            CheckAnswerFeedBack(multiChoiceAnswerEntity);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }
            multiChoiceAnswerEntity.FeedBackFromTeacher = feedBack.Text;
            _multiChoiceAnswerService.Update(multiChoiceAnswerEntity);

            return NoContent();
        }


        //POST: api/Assignment/interaction-answer/3/feed-back/add
        [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        [HttpPost("interaction-answer/{id}/feed-back/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertInteractionFeedBack([FromRoute] int id,
        [FromBody] FeedBackCreateDto feedBack)
        {
            // check if the essay answer exist
            InteractionAnswer interactionAnswerEntity = _interactionAnswerService.GetById(id, true).GetAwaiter().GetResult();
            if (interactionAnswerEntity == null)
            {
                ModelState.AddModelError("Id", "Essay answer not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            CheckAnswerFeedBack(interactionAnswerEntity);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }
            interactionAnswerEntity.FeedBackFromTeacher = feedBack.Text;
            _interactionAnswerService.Update(interactionAnswerEntity);

            return NoContent();
        }

        private void CheckAnswerFeedBack<T>(T answer) where T : Answer
        {

            //check if the teacher is the owner of the answer's question
            string myId = User.Claims.Where(c => c.Type == "sub")
                     .Select(c => c.Value).SingleOrDefault();
            if (answer.Question.Assignment.TeacherId != myId)
            {
                ModelState.AddModelError("Question", "Question not yours");
            }

            //check if the answer not have previouce feedback
            if (answer.FeedBackFromTeacher != null)
            {
                ModelState.AddModelError("FeedBacks", "Essay answer has already feedback");
            }
        }
    }
}