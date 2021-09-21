
using System.Collections.Generic;

namespace iread_assignment_ms.Web.Dto.Interaction
{
    public class InteractionAnswerDto
    {
        public int AnswerId { get; set; }
        public bool IsAnswered { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentId { get; set; }
        public string StudentLastName { get; set; }
        public string Type { get; set; }
        public List<AnswerInteractionDto> Interactions { get; set; }


    }
}