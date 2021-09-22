using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto.EssayQuestion;
using iread_assignment_ms.Web.Dto.Interaction;
using iread_assignment_ms.Web.Dto.MultiChoice;
using iread_assignment_ms.Web.Dto.StoryDto;

namespace iread_assignment_ms.Web.Dto.AssignmentDto
{
    public class AssignmentWithStoryDto
    {
        public int AssignmentId { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Nullable<int> ClassId { get; set; }
        public string TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public List<FullStoryDto> Stories { get; set; }
        public List<AttachmentDto.AttachmentDto> Attachments { get; set; }


        [Setting, DefaultValue(default(List<MultiChoiceDto>))]
        public List<MultiChoiceDto> MultiChoices { get; set; }


        [Setting, DefaultValue(default(List<EssayQuestionDto>))]
        public List<EssayQuestionDto> EssayQuestions { get; set; }


        [Setting, DefaultValue(default(List<InteractionQuestionDto>))]
        public List<InteractionQuestionDto> InteractionQuestions { get; set; }
    }
}