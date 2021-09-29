using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto;
using iread_assignment_ms.Web.Dto.AssignmentDto;
using iread_assignment_ms.Web.Dto.AssignmentDTO;
using iread_assignment_ms.Web.Dto.AttachmentDto;
using iread_assignment_ms.Web.Dto.EssayQuestion;
using iread_assignment_ms.Web.Dto.FeedBack;
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

            CreateMap<AssignmentCreateDto, Assignment>().ReverseMap();

            CreateMap<AssignmentStory, StoryDto>().ReverseMap();
            CreateMap<Assignment, AssignmentWithStoryDto>()
             .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.AssignmentStatuses != null && src.AssignmentStatuses.Count > 0 ? src.AssignmentStatuses[0].Value : null));

            CreateMap<AssignmentStory, FullStoryDto>().ReverseMap();

            //Attachment
            CreateMap<AttachmentIdDto, AssignmentAttachment>()
                .ForMember(dest => dest.AttachmentId,
                    opt => opt.MapFrom(src => src.Id));

            CreateMap<AssignmentAttachment, AttachmentIdDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.AttachmentId));
            CreateMap<AssignmentAttachment, AttachmentDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.AttachmentId));

            CreateMap<AttachmentDto, AssignmentAttachment>()
                .ForMember(dest => dest.AttachmentId,
                    opt => opt.MapFrom(src => src.Id));

            CreateMap<AttachmentIdDto, AttachmentDto>();

            CreateMap<AssignmentAttachment, AttachmentWithoutAssignmentDto>();


            CreateMap<MultiChoiceCreateDto, MultiChoice>();
            CreateMap<ChoiceCreateDto, Choice>();
            CreateMap<Choice, ChoiceDto>();
            CreateMap<MultiChoice, MultiChoiceDto>();
            CreateMap<MultiChoiceAnswer, MultiChoiceAnswerDto>();


            CreateMap<EssayQuestionCreateDto, EssayQuestion>();
            CreateMap<EssayQuestion, EssayQuestionDto>();
            CreateMap<EssayAnswer, EssayAnswerDto>();


            CreateMap<InteractionQuestionCreateDto, InteractionQuestion>();
            CreateMap<InteractionQuestion, InteractionQuestionDto>();
            CreateMap<InteractionAnswer, InteractionAnswerDto>();
            CreateMap<AnswerInteraction, AnswerInteractionDto>();

            CreateMap<StoryDto, AssignmentStory>();


        }
    }
}