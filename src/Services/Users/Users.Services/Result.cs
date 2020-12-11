using System;

namespace Lounge.Services.Users.Services
{
    public class Result
    {
        private readonly string[] _errors;

        internal Result(bool succeeded, string[] errors)
        {
            Succeeded = succeeded;

            if (!succeeded && errors.Length == 0)
            {
                _errors = new[] {"Unsuccessful operation."};
            }
            else
            {
                _errors = errors;
            }
        }

        public bool Succeeded { get; }

        public string[] Errors
            => Succeeded
                ? Array.Empty<string>()
                : _errors;

        public static Result Success
            => new Result(true, Array.Empty<string>());

        public static Result Failure(params string[] errors)
            => new Result(false, errors);
    }

    public class Result<TData> : Result
    {
        private readonly TData _data;

        private Result(bool succeeded, TData data, string[] errors)
            : base(succeeded, errors)
            => _data = data;

        public TData Data
            => Succeeded
                ? _data
                : throw new InvalidOperationException(
                    $"{nameof(Data)} is not available with a failed result. Use {this.Errors} instead.");

        public static Result<TData> SuccessWith(TData data)
            => new Result<TData>(true, data, Array.Empty<string>());

        public new static Result<TData> Failure(params string[] errors)
            => new Result<TData>(false, default, errors);
    }
}
