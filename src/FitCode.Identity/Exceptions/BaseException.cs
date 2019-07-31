using FitCode.Identity.Extensions;
using System;

namespace FitCode.Identity.Exceptions
{
    public abstract class BaseException : Exception
    {
        public ExceptionErrorCode Code { get; }
        public string CorrelationId() => this.GetCorrelationId();

        public BaseException(ExceptionErrorCode code) : base()
        {
            this.AppendCorrelationId();
            Code = code;
        }

        public BaseException(ExceptionErrorCode code, string message) : base(message)
        {
            this.AppendCorrelationId();
            Code = code;
        }

        public BaseException(ExceptionErrorCode code, string message, Exception innerException) : base(message, innerException)
        {
            this.AppendCorrelationId();
            Code = code;
        }
    }
}
