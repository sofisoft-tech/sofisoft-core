using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Sofisoft.Abstractions.Exceptions
{
    [Serializable]
    public sealed class ValidationException : ApplicationException
    {
        public IReadOnlyDictionary<string, string[]>? Errors { get; }
        
        public ValidationException(IReadOnlyDictionary<string, string[]> errors)
            : base("Validation Failure", "One or more validation errors occurred.")
            => Errors = errors;

        [ExcludeFromCodeCoverage]
        private ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}