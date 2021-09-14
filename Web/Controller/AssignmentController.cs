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
using iread_assignment_ms.Web.Dto.Class;
using iread_assignment_ms.Web.Util;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using iread_assignment_ms.Web.DTO.Story;

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
            //check class id
            InnerClassDto classDto = _consulHttpClient.GetAsync<InnerClassDto>("school_ms", $"/api/School/Class/get/{assignment.ClassId}").Result;

            if (classDto == null || classDto.ClassId == 0)
            {
                ModelState.AddModelError("ClassId", "Class not found");
            }

            //check stories' ids
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
            assignmentEntity.AssignmentStories = new List<AssignmentStory>();
            for (int index = 0; index < assignment.Stories.Count; index++)
            {

                if (res.ElementAt(index) == null)
                {
                    ModelState.AddModelError("Stories", $"Story with id = {assignment.Stories.ElementAt(index).StoryId} not found");
                }
                else
                {
                    assignmentEntity.AssignmentStories.Add(
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