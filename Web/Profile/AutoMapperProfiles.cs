using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto;
using iread_assignment_ms.Web.Dto.AssignmentDto;
using iread_assignment_ms.Web.Dto.AssignmentDTO;
using iread_assignment_ms.Web.Dto.StoryDto;
using iread_assignment_ms.Web.DTO.StoryDto;

namespace iread_assignment_ms.Web.Profile
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Assignment, AssignmentDto>().ReverseMap();
            CreateMap<Assignment, AssignmentWithStoryIdDto>().ReverseMap();

            CreateMap<AssignmentCreateDto, Assignment>().ReverseMap();

            CreateMap<AssignmentStatus, AssignmentStatusDto>().ReverseMap();
            CreateMap<AssignmentStory, AssignmentStoryIdDto>().ReverseMap();
            CreateMap<AssignmentStory, StoryDto>().ReverseMap();
            CreateMap<AssignmentWithStoryIdDto, AssignmentWithStoryDto>().ReverseMap();
            CreateMap<AssignmentStoryIdDto, FullStoryDto>().ReverseMap();

        }
    }
}