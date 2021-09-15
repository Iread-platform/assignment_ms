using System.ComponentModel.DataAnnotations;
using iread_assignment_ms.DataAccess.Data.Entity.Type;

namespace iread_assignment_ms.Web.Dto.AssignmentDTO
{

    public class AssignmentStatusDto
    {

        [EnumDataType(typeof(AssignmentStatusTypes))]
        public string Value { get; set; }
        public int AssignmentStatusId { get; set; }



    }
}