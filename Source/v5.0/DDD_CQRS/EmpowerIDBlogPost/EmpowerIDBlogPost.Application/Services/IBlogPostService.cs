using EmpowerIDBlogPost.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.Services
{
    public interface IBlogPostService
    {        
        Task<IEnumerable<BlogPost>> GetBlogPostsAsync();
        Task<BlogPost> GetBlogPostAsync(int id);
        Task<int> CreateBlogPostAsync(BlogPost blogPost);
        Task UpdateBlogPostAsync(int id, BlogPost blogPost);
        Task DeleteBlogPostAsync(int id);
    }
}
