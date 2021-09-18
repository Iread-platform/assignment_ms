using System;
using System.ComponentModel.DataAnnotations;
using iread_assignment_ms.Web.Util;

namespace iread_assignment_ms.Web.Dto.TagDto
{
    public class TagWithIdDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required(ErrorMessage = ErrorMessage.TAG_TITLE_REQUIRED)] 
        public String Title { get; set; }
    }
}