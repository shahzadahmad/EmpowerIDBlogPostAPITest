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
    public class UpdateBlogPostCommandHandler : ICommandHandler<UpdateBlogPostCommand, BlogPostDto>
    {
        private readonly IRepository<BlogPost> _repository;

        public UpdateBlogPostCommandHandler(IRepository<BlogPost> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BlogPostDto> HandleAsync(UpdateBlogPostCommand command)
        {
            // Validate or sanitize input data as needed
            if (string.IsNullOrWhiteSpace(command.Title) || string.IsNullOrWhiteSpace(command.Content))
            {
                throw new ValidationException("Title and Content are required.");
            }

            var existingBlogPost = await _repository.GetByIdAsync(command.Id);

            if (existingBlogPost == null)
            {
                throw new NotFoundException(nameof(BlogPost), command.Id);
            }

            // Update the properties of the existing blog post
            existingBlogPost.Title = command.Title;
            existingBlogPost.Content = command.Content;
            existingBlogPost.ImagePath = command.ImagePath;
            existingBlogPost.UpdatedAt = DateTime.UtcNow;

            // Save changes to the database
            await _repository.UpdateAsync(existingBlogPost);            

            // You can use AutoMapper or manual mapping to convert BlogPost to BlogPostDto
            var updatedBlogPostDto = MapToDto(existingBlogPost);

            return updatedBlogPostDto;
        }

        // Helper method to map a domain entity to a DTO
        private BlogPostDto MapToDto(BlogPost blogPost)
        {
            // Implement your mapping logic here
            // You can use libraries like AutoMapper for mapping

            // Example mapping using manual mapping (replace with your logic):
            var dto = new BlogPostDto
            {
                Id = blogPost.PostId,
                Title = blogPost.Title,
                Content = blogPost.Content,
                ImagePath = blogPost.ImagePath,
                CreatedAt = blogPost.CreatedAt,
                UpdatedAt = blogPost.UpdatedAt,
                // Map other properties as needed
            };

            return dto;
        }
    }
}
