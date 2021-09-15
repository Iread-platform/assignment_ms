using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto;
using iread_assignment_ms.Web.Dto.AssignmentDTO;

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

        }
    }
}