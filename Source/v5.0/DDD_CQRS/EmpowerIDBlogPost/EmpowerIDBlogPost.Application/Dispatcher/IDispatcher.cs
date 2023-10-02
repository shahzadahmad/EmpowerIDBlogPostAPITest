using EmpowerIDBlogPost.Application.Commands;
using EmpowerIDBlogPost.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.Dispatcher
{
    public interface IDispatcher
    {        
        Task<TResult> DispatchAsyncQry<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>;

        Task<TResult> DispatchAsyncCmd<TCommand, TResult>(TCommand command)
            where TCommand : ICommand<TResult>;
    }
}
