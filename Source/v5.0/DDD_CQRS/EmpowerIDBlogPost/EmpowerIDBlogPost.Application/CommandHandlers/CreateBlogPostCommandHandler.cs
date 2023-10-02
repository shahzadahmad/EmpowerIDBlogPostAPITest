using EmpowerIDBlogPost.Application.Commands;
using EmpowerIDBlogPost.Application.DTOs;
using EmpowerIDBlogPost.Domain.Entities;
using EmpowerIDBlogPost.Infrastructure.Persistence;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.CommandHandlers
{
    public class CreateBlogPostCommandHandler : ICommandHandler<CreateBlogPostCommand, BlogPostDto>
    {
        private readonly IRepository<BlogPost> _repository;

        public CreateBlogPostCommandHandler(IRepository<BlogPost> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<BlogPostDto> HandleAsync(CreateBlogPostCommand command)
        {
            // Validate or sanitize input data as needed
            if (string.IsNullOrWhiteSpace(command.Title) || string.IsNullOrWhiteSpace(command.Content))
            {
                throw new ValidationException("Title and Content are required.");
            }

            // Create a new BlogPost entity
            var blogPost = new BlogPost
            {
                Title = command.Title,
                Content = command.Content,
                ImagePath = command.ImagePath,
                CreatedAt = DateTime.UtcNow
                // You can set other properties as needed
            };

            // Add the new blog post into database
            await _repository.AddAsync(blogPost);

            // Map the domain entity to a DTO
            var blogPostDto = MapToDto(blogPost);

            return blogPostDto;
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
