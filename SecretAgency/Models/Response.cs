using System;
namespace SecretAgency.Models
{
    public record Response<T> : EmptyResponse
    {
        public T Result { get; internal init; }

        public Response(T result)
        {
            Result = result;
        }
    }
}
