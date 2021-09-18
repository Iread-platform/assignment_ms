using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class MultiChoice : Question
    {
        public List<Choice> Choices { get; set; }
        public Nullable<int> RightChoiceId { get; set; }
        public Choice RightChoice { get; set; }
        [Required]
        public Nullable<int> AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

    }
}