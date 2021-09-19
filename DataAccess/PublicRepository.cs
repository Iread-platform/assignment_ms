
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Repo;

namespace iread_assignment_ms.DataAccess
{
    public class PublicRepository : IPublicRepository
    {
        private readonly AppDbContext _context;
        private IAssignmentRepository _assignmentRepository;
        private IMultiChoiceRepository _multiChoiceRepository;
        private IEssayQuestionRepository _essayQuestionRepository;
        private IInteractionQuestionRepository _interactionQuestionRepository;



        private readonly IMapper _mapper;



        public PublicRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IAssignmentRepository GetAssignmentRepository
        {
            get
            {
                return _assignmentRepository ??= new AssignmentRepository(_context, _mapper);
            }
        }

        public IMultiChoiceRepository GetMultiChoiceRepository
        {
            get
            {
                return _multiChoiceRepository ??= new MultiChoiceRepository(_context, _mapper);
            }

        }
        public IEssayQuestionRepository GetEssayQuestionRepository
        {
            get
            {
                return _essayQuestionRepository ??= new EssayQuestionRepository(_context, _mapper);
            }

        }

        public IInteractionQuestionRepository GetInteractionQuestionRepository
        {
            get
            {
                return _interactionQuestionRepository ??= new InteractionQuestionRepository(_context, _mapper);
            }

        }


    }
}