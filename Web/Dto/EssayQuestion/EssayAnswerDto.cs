
namespace iread_assignment_ms.Web.Dto.EssayQuestion
{
    public class EssayAnswerDto
    {
        public int AnswerId { get; set; }
        public string Text { get; set; }
        public bool IsAnswered { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentId { get; set; }
        public string StudentLastName { get; set; }
        public string Type { get; set; }


    }
}