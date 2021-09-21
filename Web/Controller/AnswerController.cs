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
    [Route("api/Assignment/Question/")]
    public class AnswerController : ControllerBase
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

        public AnswerController(AssignmentService assignmentService,
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


        //POST: api/Assignment/Question/3/essay-answer/Add
        [Authorize(Roles = Policies.Student, AuthenticationSchemes = "Bearer")]
        [HttpPost("{id}/essay-answer/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertEssayAnswer([FromRoute] int id,
        [FromBody] EssayAnswerCreateDto essayAnswer)
        {
            // check if the question exist
            EssayQuestion essayQuestionEntity = _essayQuestionService.GetById(id).GetAwaiter().GetResult();
            if (essayQuestionEntity == null)
            {
                ModelState.AddModelError("Id", "Question not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            CheckEssayAnswer(essayQuestionEntity);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            string myId = User.Claims.Where(c => c.Type == "sub")
                     .Select(c => c.Value).SingleOrDefault();
            EssayAnswer essayAnswerEntity = essayQuestionEntity.EssayAnswers.Single(ea => ea.StudentId == myId);
            essayAnswerEntity.Text = essayAnswer.Text;
            essayAnswerEntity.IsAnswered = true;
            _essayAnswerService.Update(essayAnswerEntity);

            return Ok(_mapper.Map<EssayAnswerDto>(essayAnswerEntity));
        }



        //POST: api/Assignment/Question/3/multi-choice-answer/add
        [Authorize(Roles = Policies.Student, AuthenticationSchemes = "Bearer")]
        [HttpPost("{id}/multi-choice-answer/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertMultiChoiceAnswer([FromRoute] int id,
        [FromBody] MultiChoiceAnswerCreateDto multiChoiceAnswer)
        {
            // check if the question exist
            MultiChoice multiChoiceEntity = _multiChoiceService.GetById(id).GetAwaiter().GetResult();
            if (multiChoiceEntity == null)
            {
                ModelState.AddModelError("Id", "Question not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            CheckMultiChoiceAnswer(multiChoiceEntity, multiChoiceAnswer);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            string myId = User.Claims.Where(c => c.Type == "sub")
                     .Select(c => c.Value).SingleOrDefault();
            MultiChoiceAnswer multiChoiceAnswerEntity = multiChoiceEntity.MultiChoiceAnswers.Single(ea => ea.StudentId == myId);
            multiChoiceAnswerEntity.ChosenChoiceId = multiChoiceAnswer.ChosenChoiceId;
            multiChoiceAnswerEntity.IsAnswered = true;
            _multiChoiceAnswerService.Update(multiChoiceAnswerEntity);

            return Ok(_mapper.Map<MultiChoiceAnswerDto>(multiChoiceAnswerEntity));
        }


        //POST: api/Assignment/Question/3/interaction-answer/add
        [Authorize(Roles = Policies.Student, AuthenticationSchemes = "Bearer")]
        [HttpPost("{id}/interaction-answer/add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertInteractionAnswer([FromRoute] int id,
        [FromBody] AnswerInteractionCreateDto interaction)
        {
            // check if the question exist
            InteractionQuestion interactionQuestionEntity = _interactionQuestionService.GetById(id).GetAwaiter().GetResult();
            if (interactionQuestionEntity == null)
            {
                ModelState.AddModelError("Id", "Question not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            CheckInteractionAnswer(interactionQuestionEntity, interaction);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            string myId = User.Claims.Where(c => c.Type == "sub")
                     .Select(c => c.Value).SingleOrDefault();
            InteractionAnswer interactionAnswerEntity = interactionQuestionEntity.InteractionAnswers.Single(ea => ea.StudentId == myId);
            interactionAnswerEntity.Interactions.Add(new AnswerInteraction() { AnswerInteractionId = interaction.InteractionId });
            _interactionAnswerService.Update(interactionAnswerEntity);

            return Ok(_mapper.Map<InteractionAnswerDto>(interactionAnswerEntity));
        }

        private void CheckInteractionAnswer(InteractionQuestion interactionQuestionEntity, AnswerInteractionCreateDto interaction)
        {

            // TODO:
            // check if the interactions related to any stories of this assignment
            // check if the student is the owner of this question
            // check if the student is the owner of this interaction

        }

        private void CheckMultiChoiceAnswer(MultiChoice multiChoiceEntity, MultiChoiceAnswerCreateDto multiChoiceAnswerCreateDto)
        {

            //check if chosen choice is related to this question
            if (!multiChoiceEntity.Choices.Exists(c => c.ChoiceId == multiChoiceAnswerCreateDto.ChosenChoiceId))
            {
                ModelState.AddModelError("Answered", "you chose a wrong choice (not related to this question)");
            }

            //check if the student is the owner of this question
            if (multiChoiceEntity.MultiChoiceAnswers == null || multiChoiceEntity.MultiChoiceAnswers.Count < 1)
            {
                ModelState.AddModelError("Question", "This multi choice question not assigned to any student");
                return;
            }

            string myId = User.Claims.Where(c => c.Type == "sub")
         .Select(c => c.Value).SingleOrDefault();

            foreach (MultiChoiceAnswer multiChoiceAnswer in multiChoiceEntity.MultiChoiceAnswers)
            {
                if (multiChoiceAnswer.StudentId == myId)
                {
                    // if student is the owner of this question 
                    // check if this question is not answered before
                    if (multiChoiceAnswer.IsAnswered)
                    {
                        ModelState.AddModelError("Question", "Question is answered before");
                    }

                    return;
                }

            }
            //check if the student isn't the owner of this question
            ModelState.AddModelError("Question", "Question not yours");
        }

        private void CheckEssayAnswer(EssayQuestion essayQuestionEntity)
        {

            //check if the student is the owner of this question
            if (essayQuestionEntity.EssayAnswers == null || essayQuestionEntity.EssayAnswers.Count < 1)
            {
                ModelState.AddModelError("Question", "This essay question not assigned to any student");
                return;
            }

            string myId = User.Claims.Where(c => c.Type == "sub")
         .Select(c => c.Value).SingleOrDefault();

            foreach (EssayAnswer essayAnswer in essayQuestionEntity.EssayAnswers)
            {
                if (essayAnswer.StudentId == myId)
                {
                    // if student is the owner of this question 
                    // check if this question is not answered before
                    if (essayAnswer.IsAnswered)
                    {
                        ModelState.AddModelError("Question", "Question is answered before");
                    }

                    return;
                }

            }
            //check if the student isn't the owner of this question
            ModelState.AddModelError("Question", "Question not yours");

        }





    }
}