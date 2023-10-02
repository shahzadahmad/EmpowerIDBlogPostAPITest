using EmpowerIDBlogPost.Application.Services;
using EmpowerIDBlogPost.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts()
        {
            var blogPosts = await _blogPostService.GetBlogPostsAsync();
            return Ok(blogPosts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetBlogPost(int id)
        {
            var blogPost = await _blogPostService.GetBlogPostAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(blogPost);
        }

        [HttpPost]        
        public async Task<ActionResult<int>> CreateBlogPost(BlogPost blogPost)
        {
            var postId = await _blogPostService.CreateBlogPostAsync(blogPost);
            return CreatedAtAction(nameof(GetBlogPost), new { id = postId }, postId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(int id, BlogPost blogPost)
        {
            try
            {
                await _blogPostService.UpdateBlogPostAsync(id, blogPost);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            try
            {
                await _blogPostService.DeleteBlogPostAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
