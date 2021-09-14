using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iread_assignment_ms.DataAccess.Data.Entity.Type;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class AssignmentStatus
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentStatusId { get; set; }
        [Required]
        public Nullable<int> AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        [Required]
        public Nullable<int> StudentId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string StudentFirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string StudentLastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [EnumDataType(typeof(AssignmentStatusTypes))]
        public string Value { get; set; }



    }
}