using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class AnswerInteraction
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerInteractionId { get; set; }
        public Nullable<int> InteractionId { get; set; }
        public Nullable<int> InteractionAnswerId { get; set; }
        public InteractionAnswer InteractionAnswer { get; set; }

    }
}