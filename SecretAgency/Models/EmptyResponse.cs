using System.Collections.Generic;

namespace SecretAgency.Models
{
    public record EmptyResponse
    {
        public List<string> Errors { get; internal init; }
        public List<string> Warnings { get; internal init; }
        public bool IsSuccessful { get; internal set; }

        public EmptyResponse()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
            IsSuccessful = false;
        }

        public EmptyResponse AsSuccess() => this with { IsSuccessful = true };

        public EmptyResponse WithWarnings(params string[] warnings)
        {
            Warnings.AddRange(warnings);

            return this;
        }

        public EmptyResponse AsError(params string[] errors)
        {
            Errors.AddRange(errors);
            IsSuccessful = false;

            return this;
        }
    }
}