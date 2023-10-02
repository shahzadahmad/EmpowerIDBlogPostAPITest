using EmpowerIDBlogPost.Application.DTOs;

namespace EmpowerIDBlogPost.Application.Queries
{
    public class GetBlogPostQuery : IQuery<BlogPostDto> // Assuming you have an IQuery interface
    {
        public int Id { get; set; }
    }
}
