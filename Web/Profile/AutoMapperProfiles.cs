using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto;
using iread_assignment_ms.Web.Dto.AssignmentDto;
using iread_assignment_ms.Web.Dto.AssignmentDTO;
using iread_assignment_ms.Web.Dto.AttachmentDto;
using iread_assignment_ms.Web.Dto.EssayQuestion;
using iread_assignment_ms.Web.Dto.Interaction;
using iread_assignment_ms.Web.Dto.MultiChoice;
using iread_assignment_ms.Web.Dto.StoryDto;
using iread_assignment_ms.Web.DTO.StoryDto;

namespace iread_assignment_ms.Web.Profile
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Assignment, AssignmentDto>().ReverseMap();
            CreateMap<Assignment, InnerAssignmentDto>().ReverseMap();
            CreateMap<Assignment, AssignmentWithStoryIdDto>().ReverseMap();

            CreateMap<AssignmentCreateDto, Assignment>().ReverseMap();

            CreateMap<AssignmentStatus, AssignmentStatusDto>().ReverseMap();
            CreateMap<AssignmentStory, AssignmentStoryIdDto>().ReverseMap();
            CreateMap<AssignmentStory, StoryDto>().ReverseMap();
            CreateMap<AssignmentWithStoryIdDto, AssignmentWithStoryDto>().ReverseMap();
            CreateMap<AssignmentStoryIdDto, FullStoryDto>().ReverseMap();
            CreateMap<Assignment, AssignmentWithStoryDto>().ReverseMap();
            CreateMap<AssignmentStory ,  FullStoryDto>().ReverseMap();
            
            //Attachment
            CreateMap<AttachmentIdDto , AssignmentAttachment>()
                .ForMember(dest => dest.AttachmentId,
                    opt => opt.MapFrom(src => src.Id));
            
            CreateMap<AssignmentAttachment , AttachmentIdDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.AttachmentId));
            CreateMap<AssignmentAttachment ,  AttachmentDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.AttachmentId));

            CreateMap<AttachmentDto ,  AssignmentAttachment>()
                .ForMember(dest => dest.AttachmentId,
                    opt => opt.MapFrom(src => src.Id));

            CreateMap<AttachmentIdDto  , AttachmentDto>();
            
            CreateMap<AssignmentAttachment , AttachmentWithoutAssignmentDto>();
            

            CreateMap<MultiChoiceCreateDto, MultiChoice>();
            CreateMap<ChoiceCreateDto, Choice>();
            CreateMap<Choice, ChoiceDto>();
            CreateMap<MultiChoice, MultiChoiceDto>();

            CreateMap<EssayQuestionCreateDto, EssayQuestion>();
            CreateMap<EssayQuestion, EssayQuestionDto>();

            CreateMap<InteractionQuestionCreateDto, InteractionQuestion>();
            CreateMap<InteractionQuestion, InteractionQuestionDto>();

            CreateMap<StoryDto, AssignmentStory>();


        }
    }
}