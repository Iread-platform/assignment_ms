using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using iread_assignment_ms.Web.Service;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto;
using System;
using iread_assignment_ms.Web.Dto.AssignmentDTO;
using iread_assignment_ms.Web.Dto.UserDto;
using System.Linq;
using iread_assignment_ms.Web.Util;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using iread_assignment_ms.Web.DTO.Story;
using iread_assignment_ms.Web.Dto.School;
using iread_assignment_ms.DataAccess.Data.Entity.Type;
using iread_assignment_ms.DataAccess.Data.Type;
using iread_assignment_ms.Web.Dto.MultiChoice;
using iread_assignment_ms.Web.Dto.AssignmentDto;
using iread_assignment_ms.Web.Dto.AttachmentDto;
using iread_assignment_ms.Web.Dto.StoryDto;


namespace iread_assignment_ms.Web.Controller
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AssignmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AssignmentService _assignmentService;
        private readonly MultiChoiceService _multiChoiceService;

        private readonly IConsulHttpClientService _consulHttpClient;

        public AssignmentController(AssignmentService assignmentService,
        MultiChoiceService multiChoiceService,
         IMapper mapper, IConsulHttpClientService consulHttpClient)
        {
            _assignmentService = assignmentService;
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
            _multiChoiceService = multiChoiceService;
        }

        // GET: api/Assignment/get/1
        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Assignment assignment = await _assignmentService.GetById(id);

            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AssignmentDto>(assignment));
        }


        // GET: api/Assignment/get/of-mine
        [HttpGet("get/of-mine")]
        [Authorize(Roles = Policies.Student, AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOfMine()
        {
            string myId = User.Claims.Where(c => c.Type == "sub")
              .Select(c => c.Value).SingleOrDefault(); ;

            List<AssignmentWithStoryIdDto> assignments = await _assignmentService.GetByStudent(myId);
            List<AssignmentWithStoryDto> assignmentWithStoryDto = new List<AssignmentWithStoryDto>();


            List<FullStoryDto> fullStories = new List<FullStoryDto>();
            foreach (var assignment in assignments)
            {
                foreach (var story in assignment.Stories)
                {
                    FullStoryDto fullStoryDto = _consulHttpClient.GetAsync<FullStoryDto>("story_ms", $"/api/story/get/{story.StoryId}").GetAwaiter().GetResult();
                    fullStories.Add(fullStoryDto);
                }
                AssignmentWithStoryDto assignmentWithStoryDtoSingle = _mapper.Map<AssignmentWithStoryDto>(assignment);
                assignmentWithStoryDtoSingle.Stories = fullStories;

                assignmentWithStoryDto.Add(assignmentWithStoryDtoSingle);
                fullStories = new List<FullStoryDto>();
            }

            return Ok(assignmentWithStoryDto);
        }




        //POST: api/Assignment/3/add-multi-choice-question
        [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        [HttpPost("{id}/add-multi-choice-question")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InsertQuestion([FromRoute] int id,
        [FromBody] MultiChoiceCreateDto multiChoice)
        {
            // check if the assignment exist
            Assignment assignmentEntity = _assignmentService.GetById(id).GetAwaiter().GetResult();
            if (assignmentEntity == null)
            {
                ModelState.AddModelError("Id", "Assignment not found");
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            CheckAddMultiChoicesValidation(multiChoice, assignmentEntity);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }

            MultiChoice multiChoiceEntity = _mapper.Map<MultiChoice>(multiChoice);
            multiChoiceEntity.AssignmentId = id;
            multiChoiceEntity.Type = QuestionType.MultiChoice.ToString();
            _multiChoiceService.Insert(multiChoiceEntity, multiChoice.RightChoiceIndex);

            return Ok(multiChoiceEntity);
        }


        //POST: api/Assignment/add
        [Authorize(Roles = Policies.Teacher, AuthenticationSchemes = "Bearer")]
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Insert([FromBody] AssignmentCreateDto assignment)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Assignment assignmentEntity = _mapper.Map<Assignment>(assignment);

            CheckAddValidation(assignment, assignmentEntity);
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(ErrorMessage.ModelStateParser(ModelState));
            }


            assignmentEntity.TeacherId = User.Claims.Where(c => c.Type == "sub")
                .Select(c => c.Value).SingleOrDefault(); ;
            assignmentEntity.TeacherFirstName = User.Claims.Where(c => c.Type == "FirstName")
            .Select(c => c.Value).SingleOrDefault();
            assignmentEntity.TeacherLastName = User.Claims.Where(c => c.Type == "LastName")
            .Select(c => c.Value).SingleOrDefault();

            _assignmentService.Insert(assignmentEntity);

            return CreatedAtAction("GetById", new { id = assignmentEntity.AssignmentId }, _mapper.Map<AssignmentDto>(assignmentEntity));

        }

        private void CheckAddValidation(AssignmentCreateDto assignment, Assignment assignmentEntity)
        {
            //check class id if exists
            ClassDto classDto = _consulHttpClient.GetAsync<ClassDto>("school_ms", $"/api/School/Class/get/{assignment.ClassId}").Result;

            if (classDto == null || classDto.ClassId == 0 || classDto.Archived)
            {
                ModelState.AddModelError("ClassId", "Class not found");
            }
            else
            {
                assignmentEntity.AssignmentStatuses = new List<AssignmentStatus>();

                // get student of class to create assignment foreach one
                List<ViewStoryDto> student = new List<ViewStoryDto>();
                if (classDto.Members != null || classDto.Members.Count > 0)
                {
                    classDto.Members.ForEach(m =>

                    {
                        if (m.ClassMembershipType.Equals(ClassMembershipType.Student.ToString()))
                            assignmentEntity.AssignmentStatuses.Add(
                                new AssignmentStatus()
                                {
                                    Value = AssignmentStatusTypes.WaitingForSubmit.ToString(),
                                    StudentFirstName = m.FirstName,
                                    StudentLastName = m.LastName,
                                    StudentId = m.MemberId
                                }
                            );
                    }
                    );
                }

            }

            //check stories' ids if exist
            string storyIds = "";
            if (assignment.Stories == null || assignment.Stories.Count == 0)
            {
                ModelState.AddModelError("Stories", "stories empty");
                return;
            }

            assignment.Stories.ForEach(r =>
            {
                storyIds += r.StoryId + ",";
            });

            storyIds = storyIds.Remove(storyIds.Length - 1);
            Dictionary<string, string> formData = new Dictionary<string, string>();
            formData.Add("ids", storyIds);
            List<ViewStoryDto> res = new List<ViewStoryDto>();
            res = _consulHttpClient.PostFormAsync<List<ViewStoryDto>>("story_ms", $"/api/Story/get-by-ids", formData, res).GetAwaiter().GetResult();

            if (res == null || res.Count == 0)
            {
                ModelState.AddModelError("Stories", "Stories not found");
                return;
            }
            assignmentEntity.Stories = new List<AssignmentStory>();
            for (int index = 0; index < assignment.Stories.Count; index++)
            {

                if (res.ElementAt(index) == null)
                {
                    ModelState.AddModelError("Stories", $"Story with id = {assignment.Stories.ElementAt(index).StoryId} not found");
                }
                else
                {
                    assignmentEntity.Stories.Add(
                        new AssignmentStory()
                        {
                            StoryId = res.ElementAt(index).StoryId,
                            StoryTitle = res.ElementAt(index).Title
                        });
                }
            }
            
            //check attachment's ids if exist
            string attachmentIds = "";
            if (assignment.Attachments != null || assignment.Attachments.Count > 0)
            {
                assignment.Attachments.ForEach(a =>
                {
                    attachmentIds += a.Id + ",";
                });

                attachmentIds = attachmentIds.Remove(attachmentIds.Length - 1);
                Dictionary<string, string> attachmentFormData = new Dictionary<string, string>();
                attachmentFormData.Add("ids", attachmentIds);
                List<AttachmentIdDto> attachment = new List<AttachmentIdDto>();
                attachment = _consulHttpClient.PostFormAsync<List<AttachmentIdDto>>("attachment_ms", $"/api/Attachment/get-by-ids", attachmentFormData, attachment).GetAwaiter().GetResult();

                if (attachment == null || attachment.Count == 0)
                {
                    ModelState.AddModelError("Attachment", "Attachments not found");
                    return;
                }
                assignmentEntity.Attachments = new List<AssignmentAttachment>();
                for (int index = 0; index < assignment.Attachments.Count; index++)
                {

                    if (attachment.ElementAt(index) == null)
                    {
                        ModelState.AddModelError("Attachment", $"Attachment with id = {assignment.Attachments.ElementAt(index).Id} not found");
                    }
                    else
                    {
                        assignmentEntity.Attachments.Add(
                            new AssignmentAttachment()
                            {
                                AttachmentId = attachment.ElementAt(index).Id,
                                AssignmentId = assignmentEntity.AssignmentId,
                            });
                    }
                }
            }
        }

        private void CheckAddMultiChoicesValidation(MultiChoiceCreateDto multiChoice, Assignment assignmentEntity)
        {
            // check if the teacher the owner of this assignment
            string teacherId = User.Claims.Where(c => c.Type == "sub")
                .Select(c => c.Value).SingleOrDefault();
            if (assignmentEntity.TeacherId != teacherId)
            {
                ModelState.AddModelError("TeacherId", "Assignment is not yours");
            }

            // check if the choices not empty and more than one
            if (multiChoice.Choices.Count < 2)
            {
                ModelState.AddModelError("Choices", "Choices should be at least 2 options");
            }

            // check if the index of right choice valid
            if (!(multiChoice.RightChoiceIndex > -1 && multiChoice.RightChoiceIndex < multiChoice.Choices.Count))
            {
                ModelState.AddModelError("RightChoiceIndex", $"RightChoiceIndex should be from [0] to [{multiChoice.Choices.Count - 1}]");
            }

        }
    }
}