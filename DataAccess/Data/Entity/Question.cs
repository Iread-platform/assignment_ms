using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iread_assignment_ms.DataAccess.Data.Entity.Type;
using iread_assignment_ms.DataAccess.Data.Type;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class Question
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }
        [Required]

        public string Text { get; set; }

        [Required(AllowEmptyStrings = false)]

        [EnumDataType(typeof(QuestionType))]
        public string Type { get; set; }




    }
}