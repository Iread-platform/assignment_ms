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
using iread_assignment_ms.Web.Dto.StoryDto;

namespace iread_assignment_ms.Web.Controller
{
    [ApiController]
    [Route("api/[controller]/")]
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
            
            foreach (var assignment in assignments)
            {
                foreach (var story in assignment.Stories)
                {
                    FullStoryDto fullStoryDto = _consulHttpClient.GetAsync<FullStoryDto>("story_ms", $"/api/story/get/{story.StorytId}").GetAwaiter().GetResult();
                    
                }
            }
            
            
            return Ok(_mapper.Map<List<AssignmentWithStoryIdDto>>(assignments));
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

            CheckAddValiadtion(assignment, assignmentEntity);
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

        private void CheckAddValiadtion(AssignmentCreateDto assignment, Assignment assignmentEntity)
        {
            //check class id if exists
            ClassDto classDto = _consulHttpClient.GetAsync<ClassDto>("school_ms", $"/api/School/Class/get/{assignment.ClassId}").Result;

            if (classDto == null || classDto.ClassId == 0 || classDto.Archived)
            {
                ModelState.AddModelError("ClassId", "Class not found");
            }
            else
            {
                assignmentEntity.AssignmentStudents = new List<AssignmentStatus>();

                // get student of class to create assignment foreach one
                List<ViewStoryDto> student = new List<ViewStoryDto>();
                if (classDto.Members != null || classDto.Members.Count > 0)
                {
                    classDto.Members.ForEach(m =>

                    {
                        if (m.ClassMembershipType.Equals(ClassMembershipType.Student.ToString()))
                            assignmentEntity.AssignmentStudents.Add(
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
            res = _consulHttpClient.PostFormAsync<List<ViewStoryDto>>("story_ms", $"/api/Story/get-by-ids", formData, res).Result;

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
                            StorytId = res.ElementAt(index).StoryId,
                            StoryTitle = res.ElementAt(index).Title
                        });
                }
            }

        }
    }
}