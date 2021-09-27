using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class FeedBack
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedBackId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TeacherId { get; set; }


    }
}