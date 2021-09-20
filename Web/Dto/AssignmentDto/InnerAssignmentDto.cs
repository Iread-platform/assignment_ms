using System;
using System.Collections.Generic;
using iread_assignment_ms.Web.Dto.MultiChoice;

namespace iread_assignment_ms.Web.Dto.AssignmentDTO
{

    public class InnerAssignmentDto
    {
        public int AssignmentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Nullable<int> ClassId { get; set; }



    }
}