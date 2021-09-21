using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using iread_assignment_ms.Web.Dto.AttachmentDto;
using iread_assignment_ms.Web.DTO.StoryDto;
using iread_assignment_ms.Web.Util;

namespace iread_assignment_ms.Web.Dto
{
    public class AssignmentCreateDto
    {
        [Required]
        [DateValidation.NotDefaultValueAttribute(ErrorMessage = ErrorMessage.START_DATE_REQUIRED)]
        [DateValidation.AfterNowAttribute(ErrorMessage = ErrorMessage.START_DATE_AFTER_NOW)]
        public DateTime StartDate { get; set; }
        
        [Required]
        [DateValidation.NotDefaultValueAttribute(ErrorMessage = ErrorMessage.END_DATE_REQUIRED)]
        [DateValidation.AfterNowAttribute(ErrorMessage = ErrorMessage.END_DATE_AFTER_NOW)]
        [DateValidation.AfterAttribute(StartDateProperty = "StartDate", ErrorMessage = ErrorMessage.END_DATE_AFTER_START)]
        public DateTime EndDate { get; set; }
        
        [Required]
        public Nullable<int> ClassId { get; set; }

        public List<DTO.StoryDto.StoryDto> Stories { get; set; }
        public List<AttachmentIdDto>? Attachments { get; set; }

    }
}