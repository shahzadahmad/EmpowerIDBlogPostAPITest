using EmpowerIDBlogPost.Application.Commands;
using EmpowerIDBlogPost.Application.CommandHandlers;
using EmpowerIDBlogPost.Application.Queries;
using EmpowerIDBlogPost.Application.QueryHandlers;
using System;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.Dispatcher
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // This method should match the constraints of IDispatcher.DispatchAsync
        public async Task<TResult> DispatchAsyncQry<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for {handlerType}");
            }

            return await handler.HandleAsync((dynamic)query);
        }

        // This method should match the constraints of IDispatcher.DispatchAsync
        public async Task<TResult> DispatchAsyncCmd<TCommand, TResult>(TCommand command)
            where TCommand : ICommand<TResult>
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for {handlerType}");
            }

            return await handler.HandleAsync((dynamic)command);
        }
    }
}
