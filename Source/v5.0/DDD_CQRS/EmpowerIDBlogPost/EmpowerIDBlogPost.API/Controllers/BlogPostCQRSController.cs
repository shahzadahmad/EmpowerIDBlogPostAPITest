using EmpowerIDBlogPost.Application.Commands;
using EmpowerIDBlogPost.Application.Dispatcher;
using EmpowerIDBlogPost.Application.DTOs;
using EmpowerIDBlogPost.Application.Exceptions;
using EmpowerIDBlogPost.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.API.Controllers
{
    [ApiController]
    [Route("api/blogposts")]
    public class BlogPostCQRSController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public BlogPostCQRSController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogPost(int id)
        {
            var query = new GetBlogPostQuery() { Id = id};
            var blogPost = await _dispatcher.DispatchAsyncQry<GetBlogPostQuery, BlogPostDto>(query);

            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(blogPost);
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogPosts()
        {
            var query = new GetBlogPostsQuery();
            var blogPosts = await _dispatcher.DispatchAsyncQry<GetBlogPostsQuery, IEnumerable<BlogPostDto>>(query);

            return Ok(blogPosts);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostCommand command)
        {
            try
            {
                // Use the dispatcher to send the command to the appropriate handler.
                var result = await _dispatcher.DispatchAsyncCmd<CreateBlogPostCommand, BlogPostDto>(command);                             

                if (result != null)
                {
                    // Blog post created successfully.
                    //return Ok(result);

                    // Return a 201 Created response with the created blog post DTO.
                    return CreatedAtAction("GetBlogPost", new { id = result.Id }, result);
                }
                else
                {
                    // Handle the case where the blog post creation failed.
                    return BadRequest("Failed to create blog post.");
                }

            }
            catch (NotFoundException ex)
            {
                // Handle the case where the requested resource was not found.
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other exceptions and return a 500 Internal Server Error response.
                return StatusCode(500, "An error occurred while creating the blog post.");
            }
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(int id, [FromBody] UpdateBlogPostCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The provided ID does not match the command.");
            }

            // Execute the update command using the dispatcher
            BlogPostDto updatedBlogPost = await _dispatcher.DispatchAsyncCmd<UpdateBlogPostCommand, BlogPostDto>(command);

            if (updatedBlogPost == null)
            {
                return NotFound("Blog post not found.");
            }

            return Ok(updatedBlogPost);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            try
            {
                var command = new DeleteBlogPostCommand(id);
                var result = await _dispatcher.DispatchAsyncCmd<DeleteBlogPostCommand, bool>(command);

                if (result)
                {
                    return NoContent(); // 204 No Content
                }
                else
                {
                    return NotFound(); // 404 Not Found
                }
            }
            catch (NotFoundException)
            {
                return NotFound(); // 404 Not Found
            }
            catch (Exception)
            {
                // Handle other exceptions as needed
                return StatusCode(500); // 500 Internal Server Error
            }
        }
    }
}