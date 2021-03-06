﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News24.Data.Infrastructure;
using News24.Data.Repositories;
using News24.Model;

namespace News24.Service
{
    public interface ICommentService
    {
        Comment GetComment(int id);

        List<Comment> GetComments(int articleId);

        void CreateComment(Comment comment);

        void DeleteComment(Comment comment);

        void UpdateComment(Comment comment);
    }

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        private readonly IUnitOfWork _unitOfWork;

        public CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public Comment GetComment(int id)
        {
            var comment = _commentRepository.GetById(id);
            return comment;
        }

        public List<Comment> GetComments(int articleId)
        {
            var comments = _commentRepository.GetAll().Where(x => x.ArticleId == articleId).OrderByDescending(x => x.CommentDate).ToList();
            return comments;
        }

        public void CreateComment(Comment comment)
        {
            _commentRepository.Add(comment);
            _unitOfWork.Commit();
        }

        public void DeleteComment(Comment comment)
        {
            _commentRepository.Delete(comment);
            _unitOfWork.Commit();
        }

        public void UpdateComment(Comment comment)
        {
            _commentRepository.Update(comment);
            _unitOfWork.Commit();
        }
    }
}
