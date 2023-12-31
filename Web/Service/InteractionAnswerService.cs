﻿using System;
using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;

namespace iread_assignment_ms.Web.Service
{
    public class InteractionAnswerService
    {
        private readonly IPublicRepository _publicRepository;

        public InteractionAnswerService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<InteractionAnswer> GetById(int id, bool withQuestion)
        {
            return await _publicRepository.GetInteractionAnswerRepository.GetById(id, withQuestion);
        }

        public void Insert(InteractionAnswer interactionAnswer)
        {
            _publicRepository.GetInteractionAnswerRepository.Insert(interactionAnswer);
        }

        public bool Exists(int id)
        {
            return _publicRepository.GetInteractionAnswerRepository.Exists(id);
        }

        internal void Update(InteractionAnswer interactionAnswer, InteractionAnswer oldInteractionAnswer)
        {
            _publicRepository.GetInteractionAnswerRepository.Update(interactionAnswer, oldInteractionAnswer);
        }

        internal void Update(InteractionAnswer interactionAnswer)
        {
            _publicRepository.GetInteractionAnswerRepository.Update(interactionAnswer);
        }

        internal void AddInteractionToAnswer(AnswerInteraction answerInteraction)
        {
            _publicRepository.GetInteractionAnswerRepository.AddInteractionToAnswer(answerInteraction);
        }

        internal void Delete(InteractionAnswer interactionAnswer)
        {
            _publicRepository.GetInteractionAnswerRepository.Delete(interactionAnswer);
        }

        internal void RemoveInteractionFromAnswer(int AnswerId, int interactionId)
        {
            _publicRepository.GetInteractionAnswerRepository
            .RemoveByInteractionAndAnswer(AnswerId, interactionId);
        }
    }
}