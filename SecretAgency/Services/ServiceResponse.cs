using System.Collections.Generic;
using System.Linq;

namespace SecretAgency.Services
{
    public interface IResult<T>
    {
        List<string> Errors { get; init; }
        T Value { get; init; }
        bool IsSuccessful { get; init; }

        public bool HasFailedElseReturn(out T value);
    }

    public record Result<T> : IResult<T>
    {
        public List<string> Errors { get; init; }
        public T Value { get; init; }
        public bool IsSuccessful { get; init; }

        public bool HasFailedElseReturn(out T value)
        {
            value = Value;
            return !IsSuccessful;
        }

        public Result()
        {
            Errors = new List<string>();
        }
    }

    public static class ServiceResult
    {
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>
            {
                Value = value,
                IsSuccessful = true,
                Errors = new List<string>()
            };
        }

        public static Result<T> Failure<T>(params string[] errors)
        {
            return new Result<T>
            {
                Value = default,
                IsSuccessful = false,
                Errors = errors.ToList()
            };
        }

        public static Result<T> Failure<T>(List<string> errors)
        {
            return new Result<T>
            {
                Value = default,
                IsSuccessful = false,
                Errors = errors
            };
        }
    }

    public static class RepositoryResult
    {
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>
            {
                Value = value,
                IsSuccessful = true,
                Errors = new List<string>()
            };
        }

        public static Result<T> Failure<T>()
        {
            return new Result<T>
            {
                Value = default,
                IsSuccessful = false,
                Errors = new List<string>()
            };
        }
    }

    public static class ControllerResult
    {
        public static Result<T> Success<T>(T value)
        {
            return new Result<T>
            {
                Value = value,
                IsSuccessful = true,
                Errors = new List<string>()
            };
        }

        public static Result<T> Failure<T>(params string[] errors)
        {
            return new Result<T>
            {
                Value = default,
                IsSuccessful = false,
                Errors = errors.ToList()
            };
        }

        public static Result<T> Failure<T>(List<string> errors)
        {
            return new Result<T>
            {
                Value = default,
                IsSuccessful = false,
                Errors = errors
            };
        }
    }
}