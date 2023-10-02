using EmpowerIDBlogPost.Application.Commands;
using EmpowerIDBlogPost.Application.DTOs;
using EmpowerIDBlogPost.Application.Exceptions;
using EmpowerIDBlogPost.Domain.Entities;
using EmpowerIDBlogPost.Infrastructure.Persistence;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.CommandHandlers
{
    public class DeleteBlogPostCommandHandler : ICommandHandler<DeleteBlogPostCommand, bool>
    {
        private readonly IRepository<BlogPost> _repository;

        public DeleteBlogPostCommandHandler(IRepository<BlogPost> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> HandleAsync(DeleteBlogPostCommand command)
        {
            var blogPost = await _repository.GetByIdAsync(command.Id);

            if (blogPost == null)
            {
                throw new NotFoundException(nameof(BlogPost), command.Id);
            }

            await _repository.DeleteAsync(blogPost);            

            return true;
        }       
    }
}
