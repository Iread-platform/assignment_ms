
using System.Collections.Generic;

namespace iread_assignment_ms.Web.Dto.School
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public string Title { get; set; }
        public bool Archived { get; set; }
        public List<InnerClassMemberDto> Members { get; set; }


    }
}