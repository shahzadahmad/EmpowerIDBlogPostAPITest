using EmpowerIDBlogPost.Application.DTOs;

namespace EmpowerIDBlogPost.Application.Commands
{
    public class DeleteBlogPostCommand : ICommand<bool>
    {
        public int Id { get; set; }

        public DeleteBlogPostCommand(int id)
        {
            Id = id;
        }
    }
}
