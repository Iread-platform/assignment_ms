using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class AssignmentAttachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentAttachmentId { get; set; }

        [Required]
        public int AttachmentId { get; set; }
        
        [Required]
        public int AssignmentId { get; set; }

        public Assignment Assignment { get; set; }
    }
}