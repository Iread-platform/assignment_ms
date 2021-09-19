using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public Nullable<int> ClassId { get; set; }
        [Required]
        public string TeacherId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string TeacherFirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string TeacherLastName { get; set; }
        public List<AssignmentStory> Stories { get; set; }
        public List<AssignmentStatus> AssignmentStatuses { get; set; }
        public List<MultiChoice> MultiChoices { get; set; }


    }
}