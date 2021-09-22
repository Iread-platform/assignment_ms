using System;
using System.Collections.Generic;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class InteractionAnswer : Answer
    {
        public List<AnswerInteraction> Interactions { get; set; }

    }
}