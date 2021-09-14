using System;
using System.ComponentModel.DataAnnotations;

namespace iread_assignment_ms.Web.Dto
{
    public class AssignmentCreateDto
    {
        public int AssignmentId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public Nullable<int> ClassId { get; set; }

    }
}