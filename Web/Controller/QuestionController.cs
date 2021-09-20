using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using iread_assignment_ms.Web.Service;
using Microsoft.AspNetCore.Http;
using iread_assignment_ms.DataAccess.Data.Entity;
using System.Linq;
using iread_assignment_ms.Web.Util;
using Microsoft.AspNetCore.Authorization;
using iread_assignment_ms.DataAccess.Data.Type;
using iread_assignment_ms.Web.Dto.EssayQuestion;

namespace iread_assignment_ms.Web.Controller
{
    [ApiController]
    [Route("api/Assignment/[controller]/")]
    public class QuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AssignmentService _assignmentService;
        private readonly EssayAnswerService _essayAnswerService;
        private readonly MultiChoiceService _multiChoiceService;
        private readonly EssayQuestionService _essayQuestionService;
        private readonly InteractionQuestionService _interactionQuestionService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public QuestionController(AssignmentService assignmentService,
        MultiChoiceService multiChoiceService,
        EssayQuestionService essayQuestionService,
        EssayAnswerService essayAnswerService,
        InteractionQuestionService interactionQuestionService,
         IMapper mapper, IConsulHttpClientService consulHttpClient)
        {
            _assignmentService = assignmentService;
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
            _multiChoiceService = multiChoiceService;
            _essayQuestionService = essayQuestionService;
            _interactionQuestionService = interactionQuestionService;
            _essayAnswerService = essayAnswerService;
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

            CheckEssayQuestion(essayQuestionEntity);
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

        private void CheckEssayQuestion(EssayQuestion essayQuestionEntity)
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




        // //POST: api/Assignment/Question/3/Answer/Add
        // [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        // [HttpPost("{id}/Answer/Add")]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // public IActionResult InsertMultiChoiceQuestion([FromRoute] int id,
        // [FromBody] MultiChoiceCreateDto multiChoice)
        // {
        //     // check if the assignment exist
        //     Assignment assignmentEntity = _assignmentService.GetById(id).GetAwaiter().GetResult();
        //     if (assignmentEntity == null)
        //     {
        //         ModelState.AddModelError("Id", "Assignment not found");
        //         return BadRequest(ErrorMessage.ModelStateParser(ModelState));
        //     }

        //     CheckAddMultiChoicesValiadtion(multiChoice, assignmentEntity);
        //     if (ModelState.ErrorCount > 0)
        //     {
        //         return BadRequest(ErrorMessage.ModelStateParser(ModelState));
        //     }

        //     MultiChoice multiChoiceEntity = _mapper.Map<MultiChoice>(multiChoice);
        //     multiChoiceEntity.AssignmentId = id;
        //     multiChoiceEntity.Type = QuestionType.MultiChoice.ToString();
        //     _multiChoiceService.Insert(multiChoiceEntity, multiChoice.RightChoiceIndex);

        //     return Ok(multiChoiceEntity);
        // }




        // [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        // [HttpPost("{id}/add-interaction-question")]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // public IActionResult InsertInteractionQuestion([FromRoute] int id,
        // [FromBody] InteractionQuestionCreateDto interactionQuestion)
        // {
        //     // check if the assignment exist
        //     Assignment assignmentEntity = _assignmentService.GetById(id).GetAwaiter().GetResult();
        //     if (assignmentEntity == null)
        //     {
        //         ModelState.AddModelError("Id", "Assignment not found");
        //         return BadRequest(ErrorMessage.ModelStateParser(ModelState));
        //     }

        //     // check if the teacher the owner of this assignment
        //     CheckAssignment(assignmentEntity);

        //     if (ModelState.ErrorCount > 0)
        //     {
        //         return BadRequest(ErrorMessage.ModelStateParser(ModelState));
        //     }

        //     InteractionQuestion interactionQuestionEntity = _mapper.Map<InteractionQuestion>(interactionQuestion);
        //     interactionQuestionEntity.AssignmentId = id;
        //     interactionQuestionEntity.Type = QuestionType.Interaction.ToString();
        //     _interactionQuestionService.Insert(interactionQuestionEntity);

        //     return Ok(_mapper.Map<InteractionQuestionDto>(interactionQuestionEntity));
        // }



        //     private void CheckAddValiadtion(AssignmentCreateDto assignment, Assignment assignmentEntity)
        //     {
        //         //check class id if exists
        //         ClassDto classDto = _consulHttpClient.GetAsync<ClassDto>("school_ms", $"/api/School/Class/get/{assignment.ClassId}").Result;

        //         if (classDto == null || classDto.ClassId == 0 || classDto.Archived)
        //         {
        //             ModelState.AddModelError("ClassId", "Class not found");
        //         }
        //         else
        //         {
        //             assignmentEntity.AssignmentStatuses = new List<AssignmentStatus>();

        //             // get student of class to create assignment foreach one
        //             List<ViewStoryDto> student = new List<ViewStoryDto>();
        //             if (classDto.Members != null || classDto.Members.Count > 0)
        //             {
        //                 classDto.Members.ForEach(m =>

        //                 {
        //                     if (m.ClassMembershipType.Equals(ClassMembershipType.Student.ToString()))
        //                         assignmentEntity.AssignmentStatuses.Add(
        //                             new AssignmentStatus()
        //                             {
        //                                 Value = AssignmentStatusTypes.WaitingForSubmit.ToString(),
        //                                 StudentFirstName = m.FirstName,
        //                                 StudentLastName = m.LastName,
        //                                 StudentId = m.MemberId
        //                             }
        //                         );
        //                 }
        //                 );
        //             }

        //         }

        //         //check stories' ids if exist
        //         string storyIds = "";
        //         if (assignment.Stories == null || assignment.Stories.Count == 0)
        //         {
        //             ModelState.AddModelError("Stories", "stories empty");
        //             return;
        //         }

        //         assignment.Stories.ForEach(r =>
        //         {
        //             storyIds += r.StoryId + ",";
        //         });

        //         storyIds = storyIds.Remove(storyIds.Length - 1);
        //         Dictionary<string, string> formData = new Dictionary<string, string>();
        //         formData.Add("ids", storyIds);
        //         List<ViewStoryDto> res = new List<ViewStoryDto>();
        //         res = _consulHttpClient.PostFormAsync<List<ViewStoryDto>>("story_ms", $"/api/Story/get-by-ids", formData, res).Result;

        //         if (res == null || res.Count == 0)
        //         {
        //             ModelState.AddModelError("Stories", "Stories not found");
        //             return;
        //         }
        //         assignmentEntity.Stories = new List<AssignmentStory>();
        //         for (int index = 0; index < assignment.Stories.Count; index++)
        //         {

        //             if (res.ElementAt(index) == null)
        //             {
        //                 ModelState.AddModelError("Stories", $"Story with id = {assignment.Stories.ElementAt(index).StoryId} not found");
        //             }
        //             else
        //             {
        //                 assignmentEntity.Stories.Add(
        //                     new AssignmentStory()
        //                     {
        //                         StoryId = res.ElementAt(index).StoryId,
        //                         StoryTitle = res.ElementAt(index).Title
        //                     });
        //             }
        //         }

        //     }

        //     private void CheckAddMultiChoicesValiadtion(MultiChoiceCreateDto multiChoice, Assignment assignmentEntity)
        //     {
        //         // check if the teacher the owner of this assignment
        //         CheckQuestion(assignmentEntity);

        //         // check if the choices not empty and more than one
        //         if (multiChoice.Choices.Count < 2)
        //         {
        //             ModelState.AddModelError("Choices", "Choices should be at least 2 options");
        //         }

        //         // check if the index of right choice valid
        //         if (!(multiChoice.RightChoiceIndex > -1 && multiChoice.RightChoiceIndex < multiChoice.Choices.Count))
        //         {
        //             ModelState.AddModelError("RightChoiceIndex", $"RightChoiceIndex should be from [0] to [{multiChoice.Choices.Count - 1}]");
        //         }

        //     }

        //     private void CheckQuestion<T>(T question) where T : Question
        //     {
        //         // check if the student the owner of this question
        //         string teacherId = User.Claims.Where(c => c.Type == "sub")
        //             .Select(c => c.Value).SingleOrDefault();
        //         if (question.TeacherId != teacherId)
        //         {
        //             ModelState.AddModelError("TeacherId", "Assignment is not yours");
        //         }
        //     }

    }
}