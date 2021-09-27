using System.ComponentModel.DataAnnotations;

namespace iread_assignment_ms.Web.Dto.FeedBack
{
    public class FeedBackCreateDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }

    }
}