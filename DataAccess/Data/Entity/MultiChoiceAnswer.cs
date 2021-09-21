using System;

namespace iread_assignment_ms.DataAccess.Data.Entity
{
    public class MultiChoiceAnswer : Answer
    {

        public Nullable<int> ChosenChoiceId { get; set; }
        public Choice ChosenChoice { get; set; }

    }
}