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
        private readonly FeedBackService _feedBackService;
        private readonly EssayAnswerService _essayAnswerService;
        private readonly MultiChoiceAnswerService _multiChoiceAnswerService;
        private readonly InteractionAnswerService _interactionAnswerService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public FeedBackController(FeedBackService feedBackService,
        MultiChoiceAnswerService multiChoiceAnswerService,
        EssayAnswerService essayAnswerService,
        InteractionAnswerService interactionAnswerService,
         IMapper mapper, IConsulHttpClientService consulHttpClient)
        {
            _feedBackService = feedBackService;
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
        public IActionResult InsertFeedBack([FromRoute] int id,
        [FromBody] FeedBackCreateDto feedBack)
        {
            // check if the essay answer exist
            EssayAnswer essayAnswerEntity = _essayAnswerService.GetById(id, true).GetAwaiter().GetResult();
            if (essayAnswerEntity == null)
            {
                ModelState.AddModelError("Id", "Essay answer not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            CheckEssayAnswerFeedBack(essayAnswerEntity);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            EssayFeedBack feedbackEntity = _mapper.Map<EssayFeedBack>(feedBack);
            feedbackEntity.EssayAnswerId = essayAnswerEntity.AnswerId;
            feedbackEntity.TeacherId = User.Claims.Where(c => c.Type == "sub")
                     .Select(c => c.Value).SingleOrDefault();

            _feedBackService.Insert(feedbackEntity);

            return Ok(_mapper.Map<FeedBackDto>(feedbackEntity));
        }

        private void CheckEssayAnswerFeedBack(EssayAnswer essayAnswer)
        {

            //check if the teacher is the owner of the answer's question
            string myId = User.Claims.Where(c => c.Type == "sub")
                     .Select(c => c.Value).SingleOrDefault();
            if (essayAnswer.Question.Assignment.TeacherId != myId)
            {
                ModelState.AddModelError("Question", "Question not yours");
            }

            //check if the answer not have previouce feedback
            if (essayAnswer.FeedBacks != null && essayAnswer.FeedBacks.Count > 0)
            {
                ModelState.AddModelError("FeedBacks", "Essay answer has already feedback");
            }
        }
    }
}