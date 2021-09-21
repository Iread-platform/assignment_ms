using System.Collections.Generic;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class InteractionQuestion : Question
    {
        public List<InteractionAnswer> InteractionAnswers { get; set; }

    }
}