using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto;
using iread_assignment_ms.Web.Dto.AssignmentDTO;
using iread_assignment_ms.Web.Dto.MultiChoice;
using iread_assignment_ms.Web.DTO.StoryDto;

namespace iread_assignment_ms.Web.Profile
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Assignment, AssignmentDto>().ReverseMap();
            CreateMap<Assignment, AssignmenWithStorytDto>().ReverseMap();

            CreateMap<AssignmentCreateDto, Assignment>();

            CreateMap<AssignmentStatus, AssignmentStatusDto>().ReverseMap();
            CreateMap<AssignmentStory, AssignmentStoryDto>().ReverseMap();


            CreateMap<MultiChoiceCreateDto, MultiChoice>();
            CreateMap<ChoiceCreateDto, Choice>();
            CreateMap<Choice, ChoiceDto>();
            CreateMap<MultiChoice, MultiChoiceDto>();

            CreateMap<StoryDto, AssignmentStory>();


        }
    }
}