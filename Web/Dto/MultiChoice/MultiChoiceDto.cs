using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace iread_assignment_ms.Web.Dto.MultiChoice
{
    public class MultiChoiceDto
    {

        public int QuestionId { get; set; }
        public List<ChoiceDto> Choices { get; set; }
        public int RightChoiceIndex { get; set; }
        public string Text { get; set; }
    }
}