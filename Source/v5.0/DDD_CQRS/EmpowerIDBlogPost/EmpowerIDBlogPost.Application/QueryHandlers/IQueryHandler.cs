using EmpowerIDBlogPost.Application.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.QueryHandlers
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);        
    }
}