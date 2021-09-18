using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace iread_assignment_ms.Web.Dto.MultiChoice
{
    public class ChoiceCreateDto
    {

        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
    }
}