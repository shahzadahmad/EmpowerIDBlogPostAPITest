using EmpowerIDBlogPost.Application.Caching;
using EmpowerIDBlogPost.Domain.Entities;
using EmpowerIDBlogPost.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IRepository<BlogPost> _repository;
        private readonly ICacheService _cacheService;

        public BlogPostService(IRepository<BlogPost> repository, ICacheService cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<BlogPost>> GetBlogPostsAsync()
        {
            // Check if the data is cached
            var cachedBlogPosts = await _cacheService.GetAsync<IEnumerable<BlogPost>>("AllBlogPosts");

            if (cachedBlogPosts != null)
            {
                return cachedBlogPosts;
            }

            // If not cached, retrieve data from the repository
            var blogPosts = await _repository.GetAllAsync();

            // Cache the data for future requests
            await _cacheService.SetAsync("AllBlogPosts", blogPosts);

            return blogPosts;
        }

        public async Task<BlogPost> GetBlogPostAsync(int id)
        {
            // Check if the data is cached
            var cacheKey = $"BlogPost_{id}";
            var cachedBlogPost = await _cacheService.GetAsync<BlogPost>(cacheKey);

            if (cachedBlogPost != null)
            {
                return cachedBlogPost;
            }

            // If not cached, retrieve data from the repository
            var blogPost = await _repository.GetByIdAsync(id);

            if (blogPost != null)
            {
                // Cache the data for future requests
                await _cacheService.SetAsync(cacheKey, blogPost);
            }

            return blogPost;
        }

        public async Task<int> CreateBlogPostAsync(BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            await _repository.AddAsync(blogPost);
            await _repository.SaveChangesAsync();

            // Invalidate the cache for the list of all blog posts
            await _cacheService.RemoveAsync("AllBlogPosts");

            return blogPost.PostId;
        }

        public async Task UpdateBlogPostAsync(int id, BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            var existingPost = await _repository.GetByIdAsync(id);

            if (existingPost == null)
                throw new Exception("Blog post not found.");

            existingPost.Title = blogPost.Title;
            existingPost.Content = blogPost.Content;
            existingPost.ImagePath = blogPost.ImagePath;
            existingPost.UpdatedAt = DateTime.Now;

            _repository.Update(existingPost);
            await _repository.SaveChangesAsync();

            // Invalidate the cache for the updated blog post
            var cacheKey = $"BlogPost_{id}";
            await _cacheService.RemoveAsync(cacheKey);

            // Also invalidate the cache for the list of all blog posts
            await _cacheService.RemoveAsync("AllBlogPosts");
        }

        public async Task DeleteBlogPostAsync(int id)
        {
            var blogPost = await _repository.GetByIdAsync(id);

            if (blogPost == null)
                throw new Exception("Blog post not found.");

            _repository.Delete(blogPost);
            await _repository.SaveChangesAsync();

            // Invalidate the cache for the deleted blog post
            var cacheKey = $"BlogPost_{id}";
            await _cacheService.RemoveAsync(cacheKey);

            // Also invalidate the cache for the list of all blog posts
            await _cacheService.RemoveAsync("AllBlogPosts");
        }
    }
}
