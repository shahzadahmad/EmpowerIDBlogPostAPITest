namespace EmpowerIDBlogPost.Application.Commands
{    
    // Marker interface for commands
    public interface ICommand
    {
    }

    // ICommand with a result (generally used for commands that return data)
    public interface ICommand<TResult> : ICommand
    {
    }
}
