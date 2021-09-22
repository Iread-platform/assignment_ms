
namespace iread_assignment_ms.Web.Dto.Interaction
{
    public class GeneralInteractionDto
    {
        public int InteractionId { get; set; }
        public int StoryId { get; set; }
        public string StudentId { get; set; }
        public int PageId { get; set; }
        public InnerDrawingDto Drawing { get; set; }
        public InnerCommentDto Comment { get; set; }
        public InnerHighLightDto HighLight { get; set; }
        public InnerAudioDto Audio { get; set; }
    }
}