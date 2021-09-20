using System.Collections.Generic;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class EssayQuestion : Question
    {
        public List<EssayAnswer> EssayAnswers { get; set; }

    }
}