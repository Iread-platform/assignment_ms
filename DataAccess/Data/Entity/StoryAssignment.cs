using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class AssignmentStory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentStoryId { get; set; }
        [Required]
        public Nullable<int> AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        [Required]
        public Nullable<int> StorytId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string StoryTitle { get; set; }

    }
}