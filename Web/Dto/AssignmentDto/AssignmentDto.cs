using System;
using System.Collections.Generic;
using iread_assignment_ms.Web.Dto.EssayQuestion;
using iread_assignment_ms.Web.Dto.Interaction;
using iread_assignment_ms.Web.Dto.MultiChoice;

namespace iread_assignment_ms.Web.Dto.AssignmentDTO
{

    public class AssignmentDto
    {
        public int AssignmentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Nullable<int> ClassId { get; set; }
        public string TeacherId { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public List<MultiChoiceDto> MultiChoices { get; set; }
        public List<EssayQuestionDto> EssayQuestions { get; set; }
        public List<InteractionQuestionDto> InteractionQuestions { get; set; }



    }
}