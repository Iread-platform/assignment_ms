﻿
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
        private IAttachmentRepository _attachmentRepository;
        private IEssayQuestionRepository _essayQuestionRepository;
        private IInteractionQuestionRepository _interactionQuestionRepository;
        private IEssayAnswerRepository _essayAnswerRepository;
        private IMultiChoiceAnswerRepository _multiChoiceAnswerRepository;
        private IInteractionAnswerRepository _interactionAnswerRepository;


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

        public IAttachmentRepository GetAttachmentRepository
        {
            get
            {
                return _attachmentRepository ??= new AttachmentRepository(_context);
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

        public IEssayAnswerRepository GetEssayAnswerRepository
        {
            get
            {
                return _essayAnswerRepository ??= new EssayAnswerRepository(_context, _mapper);
            }

        }

        public IMultiChoiceAnswerRepository GetMultiChoiceAnswerRepository
        {
            get
            {
                return _multiChoiceAnswerRepository ??= new MultiChoiceAnswerRepository(_context, _mapper);
            }

        }

        public IInteractionAnswerRepository GetInteractionAnswerRepository
        {
            get
            {
                return _interactionAnswerRepository ??= new InteractionAnswerRepository(_context, _mapper);
            }
        }


    }
}