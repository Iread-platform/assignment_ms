using iread_assignment_ms.DataAccess.Repo;

namespace iread_assignment_ms.DataAccess
{
    public interface IPublicRepository
    {
        IAssignmentRepository GetAssignmentRepository { get; }

        IMultiChoiceRepository GetMultiChoiceRepository { get; }

        IAttachmentRepository GetAttachmentRepository { get; }

        IEssayQuestionRepository GetEssayQuestionRepository { get; }

        IInteractionQuestionRepository GetInteractionQuestionRepository { get; }
        
    }
}