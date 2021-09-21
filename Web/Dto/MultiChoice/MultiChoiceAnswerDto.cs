
namespace iread_assignment_ms.Web.Dto.MultiChoice
{
    public class MultiChoiceAnswerDto
    {
        public int AnswerId { get; set; }
        public int ChosenChoiceId { get; set; }
        public ChoiceDto ChosenChoice { get; set; }
        public bool IsAnswered { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentId { get; set; }
        public string StudentLastName { get; set; }
        public string Type { get; set; }

    }
}