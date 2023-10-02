using EmpowerIDBlogPost.Application.Caching;
using EmpowerIDBlogPost.Application.DTOs;
using EmpowerIDBlogPost.Application.Exceptions;
using EmpowerIDBlogPost.Application.Queries;
using EmpowerIDBlogPost.Domain.Entities;
using EmpowerIDBlogPost.Infrastructure.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.QueryHandlers
{
    public class GetBlogPostQueryHandler : IQueryHandler<GetBlogPostQuery, BlogPostDto>
    {
        private readonly IRepository<BlogPost> _blogPostRepository; // Inject your repository here
        private readonly ICacheService _cacheService;

        public GetBlogPostQueryHandler(IRepository<BlogPost> blogPostRepository, ICacheService cacheService)
        {
            _blogPostRepository = blogPostRepository;
            _cacheService = cacheService;
        }

        public async Task<BlogPostDto> HandleAsync(GetBlogPostQuery query)
        {
            // Check if the data is cached
            var cacheKey = $"BlogPost_{query.Id}";
            var cachedBlogPost = await _cacheService.GetAsync<BlogPostDto>(cacheKey);

            if (cachedBlogPost != null)
            {
                return cachedBlogPost;
            }

            // Use your repository to fetch a specific blog post based on the query parameters
            var blogPost = await _blogPostRepository.GetByIdAsync(query.Id);

            // Check if the blog post exists
            if (blogPost == null)
            {
                // You can throw an exception or return null based on your error handling strategy
                throw new NotFoundException($"Blog post with ID {query.Id} not found.");
            }
            else
            {
                // Map the domain entity to a DTO
                var blogPostDto = MapToDto(blogPost);

                await _cacheService.SetAsync(cacheKey, blogPostDto, TimeSpan.FromMinutes(10)); // Cache for 10 minutes

                return blogPostDto;
            }            
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
