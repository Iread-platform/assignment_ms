using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iread_assignment_ms.DataAccess.Data.Type;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class Answer
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }

        [Required(AllowEmptyStrings = false)]

        [EnumDataType(typeof(QuestionType))]
        public string Type { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string StudentId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string StudentFirstName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string StudentLastName { get; set; }
        public bool IsAnswered { get; set; } // answered means submitted also
        public List<FeedBack> FeedBacks { get; set; }
        public string FeedBackFromTeacher { get; set; }

    }
}