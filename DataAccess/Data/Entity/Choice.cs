using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class Choice
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChoiceId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public Question MultiChoice { get; set; }

    }
}