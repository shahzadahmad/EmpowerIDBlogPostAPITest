using EmpowerIDBlogPost.Application.Caching;
using EmpowerIDBlogPost.Application.DTOs;
using EmpowerIDBlogPost.Application.Queries;
using EmpowerIDBlogPost.Domain.Entities;
using EmpowerIDBlogPost.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.QueryHandlers
{
    public class GetBlogPostsQueryHandler : IQueryHandler<GetBlogPostsQuery, IEnumerable<BlogPostDto>>
    {
        private readonly IRepository<BlogPost> _blogPostRepository; // Inject your repository here
        private readonly ICacheService _cacheService;

        public GetBlogPostsQueryHandler(IRepository<BlogPost> blogPostRepository, ICacheService cacheService)
        {
            _blogPostRepository = blogPostRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<BlogPostDto>> HandleAsync(GetBlogPostsQuery query)
        {
            // Check if the data is cached
            var cachedBlogPosts = await _cacheService.GetAsync<IEnumerable<BlogPostDto>>("AllBlogPosts");

            if (cachedBlogPosts != null)
            {
                return cachedBlogPosts;
            }

            // Use your repository to fetch blog posts based on the query parameters
            var blogPosts = await _blogPostRepository.GetAllAsync();

            // You may map the domain entities to DTOs here if needed
            var blogPostDtos = MapToDto(blogPosts);

            // Cache the data for future requests
            await _cacheService.SetAsync("AllBlogPosts", blogPostDtos, TimeSpan.FromMinutes(10)); // Cache for 10 minutes

            return blogPostDtos;
        }

        // Helper method to map domain entities to DTOs
        private List<BlogPostDto> MapToDto(IEnumerable<BlogPost> blogPosts)
        {
            // Implement your mapping logic here
            // You can use libraries like AutoMapper for mapping

            // Example mapping using manual mapping (replace with your logic):
            var blogPostDtos = new List<BlogPostDto>();
            foreach (var blogPost in blogPosts)
            {
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
                blogPostDtos.Add(dto);
            }

            return blogPostDtos;
        }
    }
}