using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using iread_assignment_ms.Web.DTO.StoryDto;

namespace iread_assignment_ms.Web.Dto
{
    public class AssignmentCreateDto
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public Nullable<int> ClassId { get; set; }

        public List<DTO.StoryDto.StoryDto> Stories { get; set; }

    }
}