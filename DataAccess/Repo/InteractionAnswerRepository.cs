﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using iread_assignment_ms.DataAccess.Data;
using iread_assignment_ms.DataAccess.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace iread_assignment_ms.DataAccess.Repo
{
    public class InteractionAnswerRepository : IInteractionAnswerRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public InteractionAnswerRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InteractionAnswer> GetById(int id, bool withQuestion)
        {
            return withQuestion ?
            await _context.InteractionAnswer
            .Include(e => e.Question)
            .ThenInclude(q => ((InteractionQuestion)q).Assignment)
            .SingleOrDefaultAsync(m => m.AnswerId == id) :
            await _context.InteractionAnswer.SingleOrDefaultAsync(m => m.AnswerId == id);
        }

        public void Insert(InteractionAnswer interactionAnswer)
        {
            _context.InteractionAnswer.Add(interactionAnswer);
            _context.SaveChanges();
        }

        public void Delete(InteractionAnswer interactionAnswer)
        {
            _context.InteractionAnswer.Remove(interactionAnswer);
            _context.SaveChanges();
        }

        public void Update(InteractionAnswer interactionAnswer, InteractionAnswer oldInteractionAnswer)
        {
            _context.Entry(oldInteractionAnswer).State = EntityState.Deleted;
            _context.InteractionAnswer.Attach(interactionAnswer);
            _context.Entry(interactionAnswer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(InteractionAnswer interactionAnswer)
        {
            _context.Entry(interactionAnswer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool Exists(int id)
        {
            return _context.InteractionAnswer.Any(r => r.AnswerId == id);
        }

        public void AddInteractionToAnswer(AnswerInteraction answerInteraction)
        {
            _context.AnswerInteraction.Add(answerInteraction);
            _context.SaveChanges();
        }

        public void RemoveByInteractionAndAnswer(int AnswerId, int interactionId)
        {
            _context.AnswerInteraction.RemoveRange(
                _context.AnswerInteraction.Where(
                            ia => ia.InteractionId == interactionId
                               && ia.InteractionAnswerId == AnswerId).ToList());
            _context.SaveChanges();

        }

    }
}