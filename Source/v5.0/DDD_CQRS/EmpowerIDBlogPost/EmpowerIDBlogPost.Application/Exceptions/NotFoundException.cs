using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpowerIDBlogPost.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string entityName, object entityId)
           : base($"Entity of type {entityName} with ID {entityId} was not found.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }

        public string EntityName { get; }
        public object EntityId { get; }
    }
}
