using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using iread_assignment_ms.Web.Service;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto;
using iread_assignment_ms.Web.Dto.AssignmentDTO;
using System.Linq;
using iread_assignment_ms.Web.Util;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Text.Json;
using iread_assignment_ms.Web.DTO.Story;
using iread_assignment_ms.Web.Dto.School;
using iread_assignment_ms.DataAccess.Data.Entity.Type;
using iread_assignment_ms.DataAccess.Data.Type;
using iread_assignment_ms.Web.Dto.MultiChoice;
using iread_assignment_ms.Web.Dto.AssignmentDto;
using iread_assignment_ms.Web.Dto.AttachmentDto;
using iread_assignment_ms.Web.Dto.StoryDto;
using iread_assignment_ms.Web.Dto.Notification;

namespace iread_assignment_ms.Web.Controller
{
    [ApiController]
    [Route("api/[controller]/")]
    public class AssignmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AssignmentService _assignmentService;
        private readonly IConsulHttpClientService _consulHttpClient;

        public AssignmentController(AssignmentService assignmentService,
         IMapper mapper, IConsulHttpClientService consulHttpClient)
        {
            _assignmentService = assignmentService;
            _mapper = mapper;
            _consulHttpClient = consulHttpClient;
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

            List<FullStoryDto> fullStories = new List<FullStoryDto>();

            AssignmentWithStoryDto assignmentWithStoryDto = _mapper.Map<AssignmentWithStoryDto>(assignment);

            if (assignment.Attachments != null && assignment.Attachments.Count > 0)
            {
                string attachmentIds = "";

                assignmentWithStoryDto.Attachments.ForEach(r =>
                {
                    attachmentIds += r.Id + ",";
                });

                attachmentIds = attachmentIds.Remove(attachmentIds.Length - 1);
                Dictionary<string, string> formData = new Dictionary<string, string>();
                formData.Add("ids", attachmentIds);
                List<AttachmentDto> res = new List<AttachmentDto>();
                res = _consulHttpClient.PostFormAsync<List<AttachmentDto>>("attachment_ms", $"/api/Attachment/get-by-ids", formData, res).GetAwaiter().GetResult();

                assignmentWithStoryDto.Attachments = new List<AttachmentDto>();
                assignmentWithStoryDto.Attachments.AddRange(res);
            }

            foreach (var story in assignmentWithStoryDto.Stories)
            {
                FullStoryDto fullStoryDto = _consulHttpClient.GetAsync<FullStoryDto>("story_ms", $"/api/story/get/{story.StoryId}").GetAwaiter().GetResult();
                fullStories.Add(fullStoryDto);
            }

            assignmentWithStoryDto.Stories = new List<FullStoryDto>();
            assignmentWithStoryDto.Stories.AddRange(fullStories);
            fullStories = new List<FullStoryDto>();

            return Ok(assignmentWithStoryDto);
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

            List<Assignment> assignments = await _assignmentService.GetByStudent(myId);

            List<AssignmentWithStoryDto> assignmentsWithStoryDto =
                _mapper.Map<List<AssignmentWithStoryDto>>(assignments);

            // for each assginment
            foreach (var assignment in assignmentsWithStoryDto)
            {
                // get attachments' details
                if (assignment.Attachments != null && assignment.Attachments.Count > 0)
                {
                    string attachmentIds = "";

                    assignment.Attachments.ForEach(r =>
                    {
                        attachmentIds += r.Id + ",";
                    });

                    attachmentIds = attachmentIds.Remove(attachmentIds.Length - 1);
                    Dictionary<string, string> formData = new Dictionary<string, string>();
                    formData.Add("ids", attachmentIds);
                    List<AttachmentDto> res = new List<AttachmentDto>();
                    res = _consulHttpClient.PostFormAsync<List<AttachmentDto>>("attachment_ms", $"/api/Attachment/get-by-ids", formData, res).GetAwaiter().GetResult();

                    assignment.Attachments = new List<AttachmentDto>();
                    assignment.Attachments.AddRange(res);
                }

                // get stories' details
                List<FullStoryDto> fullStories = new List<FullStoryDto>();
                foreach (var story in assignment.Stories)
                {
                    FullStoryDto res = _consulHttpClient.GetAsync<FullStoryDto>("story_ms", $"/api/story/get/{story.StoryId}").GetAwaiter().GetResult();
                    fullStories.Add(res);
                }
                assignment.Stories = new List<FullStoryDto>(fullStories);
            }

            return Ok(assignmentsWithStoryDto);
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

            return CreatedAtAction("GetById", new
            {
                id = assignmentEntity.AssignmentId
            }, _mapper.Map<AssignmentDto>(assignmentEntity));

        }
        private async Task<SingletNotificationDto> SendSingleNotification(string title, string body, int userId, string message, string route)
        {
            SingletNotificationDto response = new SingletNotificationDto() { Body = body, UserId = 1, Title = title, ExtraData = new ExtraDataDto() { GoTo = route, Messsage = message } };
            response = await _consulHttpClient.PostBodyAsync<SingletNotificationDto>("notifications_ms", $"/api/Notification/Send",
             response);

            return response;
        }

        private async Task<TopicNotificationAddDto> SendTopicNotification(string title, string body, string topicName, string message, string route)
        {
            TopicNotificationAddDto response = new TopicNotificationAddDto() { Body = body, TopicName = topicName, Title = title, ExtraData = new ExtraDataDto() { GoTo = route, Messsage = message } };
            response = await _consulHttpClient.PostBodyAsync<TopicNotificationAddDto>("notifications_ms", $"/api/Notification/broadcast-by-topic-title",
             response);

            return response;
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
            if (assignment.Attachments != null && assignment.Attachments.Count > 0)
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