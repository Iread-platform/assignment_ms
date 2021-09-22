using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using iread_assignment_ms.Web.Service;
using Microsoft.AspNetCore.Http;
using iread_assignment_ms.DataAccess.Data.Entity;
using System.Linq;
using iread_assignment_ms.Web.Util;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using iread_assignment_ms.DataAccess.Data.Type;
using iread_assignment_ms.Web.Dto.MultiChoice;
using iread_assignment_ms.Web.Dto.EssayQuestion;
using iread_assignment_ms.Web.Dto.Interaction;

namespace iread_assignment_ms.Web.Controller
{
    [ApiController]
    [Route("api/Assignment/")]
    public class QuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AssignmentService _assignmentService;
        private readonly MultiChoiceService _multiChoiceService;
        private readonly EssayQuestionService _essayQuestionService;
        private readonly InteractionQuestionService _interactionQuestionService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public QuestionController(AssignmentService assignmentService,
        MultiChoiceService multiChoiceService,
        EssayQuestionService essayQuestionService,
        InteractionQuestionService interactionQuestionService,
         IMapper mapper, IConsulHttpClientService consulHttpClient)
        {
            _assignmentService = assignmentService;
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
            _multiChoiceService = multiChoiceService;
            _essayQuestionService = essayQuestionService;
            _interactionQuestionService = interactionQuestionService;
        }


        [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        [HttpPost("{id}/add-multi-choice-question")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertMultiChoiceQuestion([FromRoute] int id,
        [FromBody] MultiChoiceCreateDto multiChoice)
        {
            // check if the assignment exist
            Assignment assignmentEntity = _assignmentService.GetById(id).GetAwaiter().GetResult();
            if (assignmentEntity == null)
            {
                ModelState.AddModelError("Id", "Assignment not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }


            MultiChoice multiChoiceEntity = _mapper.Map<MultiChoice>(multiChoice);
            CheckAddMultiChoicesValidation(multiChoice, multiChoiceEntity, assignmentEntity);

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }


            multiChoiceEntity.AssignmentId = id;
            multiChoiceEntity.Type = QuestionType.MultiChoice.ToString();
            _multiChoiceService.Insert(multiChoiceEntity, multiChoice.RightChoiceIndex);

            return Ok(multiChoiceEntity);
        }

        [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        [HttpPost("{id}/add-essay-question")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertEssayQuestion([FromRoute] int id,
        [FromBody] EssayQuestionCreateDto essayQuestion)
        {
            // check if the assignment exist
            Assignment assignmentEntity = _assignmentService.GetById(id).GetAwaiter().GetResult();
            if (assignmentEntity == null)
            {
                ModelState.AddModelError("Id", "Assignment not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            EssayQuestion essayQuestionEntity = _mapper.Map<EssayQuestion>(essayQuestion);

            // check if the teacher the owner of this assignment
            CheckAddEssayQuestionValiadtion(assignmentEntity, essayQuestionEntity);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            essayQuestionEntity.AssignmentId = id;
            essayQuestionEntity.Type = QuestionType.EssayQuestion.ToString();
            _essayQuestionService.Insert(essayQuestionEntity);

            return Ok(_mapper.Map<EssayQuestionDto>(essayQuestionEntity));
        }


        [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        [HttpPost("{id}/add-interaction-question")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertInteractionQuestion([FromRoute] int id,
        [FromBody] InteractionQuestionCreateDto interactionQuestion)
        {
            // check if the assignment exist
            Assignment assignmentEntity = _assignmentService.GetById(id).GetAwaiter().GetResult();
            if (assignmentEntity == null)
            {
                ModelState.AddModelError("Id", "Assignment not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }
            InteractionQuestion interactionQuestionEntity = _mapper.Map<InteractionQuestion>(interactionQuestion);


            // check if the teacher the owner of this assignment
            CheckAddInteractionQuestionValidation(assignmentEntity, interactionQuestionEntity);

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            interactionQuestionEntity.AssignmentId = id;
            interactionQuestionEntity.Type = QuestionType.Interaction.ToString();
            _interactionQuestionService.Insert(interactionQuestionEntity);

            return Ok(_mapper.Map<InteractionQuestionDto>(interactionQuestionEntity));
        }

        private void CheckAddMultiChoicesValidation(MultiChoiceCreateDto multiChoiceDto, MultiChoice multiChoiceEntity, Assignment assignmentEntity)
        {
            // check if the teacher the owner of this assignment
            CheckAssignment(assignmentEntity);

            // check if the choices not empty and more than one
            if (multiChoiceDto.Choices.Count < 2)
            {
                ModelState.AddModelError("Choices", "Choices should be at least 2 options");
            }

            // check if the index of right choice valid
            if (!(multiChoiceDto.RightChoiceIndex > -1 && multiChoiceDto.RightChoiceIndex < multiChoiceDto.Choices.Count))
            {
                ModelState.AddModelError("RightChoiceIndex", $"RightChoiceIndex should be from [0] to [{multiChoiceDto.Choices.Count - 1}]");
            }

            // create empty answers for each student
            multiChoiceEntity.MultiChoiceAnswers = new List<MultiChoiceAnswer>();

            // get student of class to create empty answer foreach one
            assignmentEntity.AssignmentStatuses.ForEach(m =>
                {
                    multiChoiceEntity.MultiChoiceAnswers.Add(
                          new MultiChoiceAnswer()
                          {
                              Question = multiChoiceEntity,
                              Type = QuestionType.MultiChoice.ToString(),
                              StudentId = m.StudentId,
                              StudentFirstName = m.StudentFirstName,
                              StudentLastName = m.StudentLastName
                          }
                      );
                }
                );

        }

        private void CheckAddInteractionQuestionValidation(Assignment assignmentEntity, InteractionQuestion interactionQuestion)

        {
            // check if the teacher the owner of this assignment
            CheckAssignment(assignmentEntity);

            // create empty answers for each student
            interactionQuestion.InteractionAnswers = new List<InteractionAnswer>();

            // get student of class to create empty answer foreach one
            assignmentEntity.AssignmentStatuses.ForEach(m =>
                {
                    interactionQuestion.InteractionAnswers.Add(
                          new InteractionAnswer()
                          {
                              Question = interactionQuestion,
                              Type = QuestionType.Interaction.ToString(),
                              StudentId = m.StudentId,
                              StudentFirstName = m.StudentFirstName,
                              StudentLastName = m.StudentLastName
                          }
                      );
                }
                );

        }

        private void CheckAssignment(Assignment assignment)
        {
            // check if the teacher the owner of this assignment
            string teacherId = User.Claims.Where(c => c.Type == "sub")
                .Select(c => c.Value).SingleOrDefault();
            if (assignment.TeacherId != teacherId)
            {
                ModelState.AddModelError("TeacherId", "Assignment is not yours");
            }
        }

        private void CheckAddEssayQuestionValiadtion(Assignment assignmentEntity, EssayQuestion essayQuestion)
        {
            // check if the teacher the owner of this assignment
            CheckAssignment(assignmentEntity);

            // create empty answers for each student
            essayQuestion.EssayAnswers = new List<EssayAnswer>();

            // get student of class to create empty answer foreach one
            assignmentEntity.AssignmentStatuses.ForEach(m =>
                {
                    essayQuestion.EssayAnswers.Add(
                          new EssayAnswer()
                          {
                              Question = essayQuestion,
                              Type = QuestionType.EssayQuestion.ToString(),
                              StudentId = m.StudentId,
                              StudentFirstName = m.StudentFirstName,
                              StudentLastName = m.StudentLastName
                          }
                      );
                }
                );
        }

    }
}