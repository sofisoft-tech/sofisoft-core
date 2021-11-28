using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Sofisoft.Abstractions.Exceptions
{
    public abstract class BadRequestException : ApplicationException
    {
        protected BadRequestException(string message)
            : base("Bad Request", message)
        { }

        [ExcludeFromCodeCoverage]
        protected BadRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}