using EmpowerIDBlogPost.Application.DTOs;

namespace EmpowerIDBlogPost.Application.Commands
{
    public class UpdateBlogPostCommand : ICommand<BlogPostDto>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }        
    }
}
