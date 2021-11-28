using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Sofisoft.Abstractions.Exceptions
{
    public abstract class NotFoundException : ApplicationException
    {
        protected NotFoundException(string message)
            : base("Not Found", message)
        { }

        [ExcludeFromCodeCoverage]
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}