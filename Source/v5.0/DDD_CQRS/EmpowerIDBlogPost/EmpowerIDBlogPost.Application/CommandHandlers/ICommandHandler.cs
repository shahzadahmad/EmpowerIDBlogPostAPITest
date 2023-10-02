using EmpowerIDBlogPost.Application.Commands;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.CommandHandlers
{        
    // Marker interface for command handlers
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }

    // Command handler with a result (generally used for commands that return data)
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
