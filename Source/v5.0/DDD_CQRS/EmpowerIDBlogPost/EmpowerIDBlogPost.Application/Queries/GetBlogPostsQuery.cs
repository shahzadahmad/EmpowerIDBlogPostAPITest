using EmpowerIDBlogPost.Application.DTOs;
using System.Collections.Generic;

namespace EmpowerIDBlogPost.Application.Queries
{
    public class GetBlogPostsQuery : IQuery<IEnumerable<BlogPostDto>>
    {
        // You can include properties to specify query parameters if needed
        // (e.g., filtering criteria, sorting options).
    }
}
