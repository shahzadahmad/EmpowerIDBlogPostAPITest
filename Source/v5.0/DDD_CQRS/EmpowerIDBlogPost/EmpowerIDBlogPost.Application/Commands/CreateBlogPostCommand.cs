using EmpowerIDBlogPost.Application.DTOs;

namespace EmpowerIDBlogPost.Application.Commands
{
    public class CreateBlogPostCommand : ICommand<BlogPostDto>
    {        
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }        
    }
}
