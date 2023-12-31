﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using iread_assignment_ms.DataAccess;
using iread_assignment_ms.DataAccess.Data.Entity;
using iread_assignment_ms.Web.Dto.AssignmentDTO;

namespace iread_assignment_ms.Web.Service
{
    public class AssignmentService
    {
        private readonly IPublicRepository _publicRepository;

        public AssignmentService(IPublicRepository publicRepository)
        {
            _publicRepository = publicRepository;
        }

        public async Task<Assignment> GetById(int id)
        {
            return await _publicRepository.GetAssignmentRepository.GetById(id);
        }

        public void Insert(Assignment assignment)
        {
            _publicRepository.GetAssignmentRepository.Insert(assignment);
        }

        public bool Exists(int id)
        {
            return _publicRepository.GetAssignmentRepository.Exists(id);
        }

        internal void Update(Assignment assignment, Assignment oldAssignment)
        {
            _publicRepository.GetAssignmentRepository.Update(assignment, oldAssignment);
        }

        internal void Delete(Assignment assignment)
        {
            _publicRepository.GetAssignmentRepository.Delete(assignment);
        }

        internal async Task<List<Assignment>> GetByStudent(string myId)
        {
            return await _publicRepository.GetAssignmentRepository.GetByStudent(myId);
        }

        internal void SubmitAnswers(Assignment assignment, string studentId)
        {
            _publicRepository.GetAssignmentRepository.SubmitAnswers(assignment.AssignmentId, studentId);
        }

        internal bool IsMine(Assignment assignment, string studentId)
        {
            return _publicRepository.GetAssignmentRepository.IsMine(assignment.AssignmentId, studentId);
        }

        internal AssignmentStatus GetStatusByAssignmentStudentId(int id, string studentId)
        {
            return _publicRepository.GetAssignmentRepository.GetStatusByAssignmentStudentId(id, studentId);
        }

        internal async Task<Question> GetQuestionById(int questionId)
        {
            return await _publicRepository.GetAssignmentRepository.GetQuestionById(questionId);
        }

        internal void Update(AssignmentStatus assignmentStatus)
        {
            _publicRepository.GetAssignmentRepository.Update(assignmentStatus);
        }
    }
}