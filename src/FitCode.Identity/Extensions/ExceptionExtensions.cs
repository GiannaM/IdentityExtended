using FitCode.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCode.Identity.Extensions
{
    public static class ExceptionExtensions
    {
        private static readonly string ExceptionStateKey = typeof(LoggerState).FullName;
        private const string ExceptionDoNotLog = "Keona.DoNotLog";
        public const string CorrelationId = "Keona.CorrelationId";

        public const string AppName = "OnlineTriage";

        static ExceptionExtensions() { }

        public static void DoNotLog(this Exception ex)
        {
            if (ex.Data.Contains(ExceptionDoNotLog))
            {
                ex.Data[ExceptionDoNotLog] = true;
            }
            else
            {
                ex.Data.Add(ExceptionDoNotLog, true);
            }
        }

        public static bool CanLog(this Exception ex)
        {
            if (ex.Data.Contains(ExceptionDoNotLog) && bool.TryParse(ex.Data[ExceptionDoNotLog].ToString(), out bool result))
                return !result;

            return true;
        }

        public static Exception AppendCorrelationId(this Exception ex, string defaultCorrelationId = null)
        {
            string correlationId = string.Empty;

            if (string.IsNullOrWhiteSpace(defaultCorrelationId))
            {
                correlationId = Guid.NewGuid().ToString().Replace("-", "");
            }
            else
            {
                correlationId = defaultCorrelationId;
            }

            if (ex.Data.Contains(CorrelationId))
            {
                ex.Data[CorrelationId] = correlationId;
            }
            else
            {
                ex.Data.Add(CorrelationId, correlationId);
            }

            return ex;
        }

        public static string GetCorrelationId(this Exception ex)
        {
            if (ex.Data.Contains(CorrelationId))
            {
                return ex.Data[CorrelationId].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static LoggerState GetStateOrNull(this Exception ex)
        {
            if (ex.Data.Contains(ExceptionStateKey))
                return ex.Data[ExceptionStateKey] as LoggerState;

            return null;
        }

        public static void AddState<TException>(this TException ex, LoggerState state) where TException : Exception
        {
            if (ex != null)
            {
                var mergeState = state;

                if (ex.Data.Contains(ExceptionStateKey))
                {
                    var prevState = ex.Data[ExceptionStateKey] as LoggerState;
                    if (prevState != null) { mergeState = prevState.MergedWith(state); }
                }

                if (mergeState != null)
                {
                    ex.Data[ExceptionStateKey] = mergeState;
                }
            }
        }

        public static Exception WithState<TException>(this TException ex, LoggerState state) where TException : Exception
        {
            if (state != null)
            {
                var correlationId = ex.GetCorrelationId();
                if (correlationId != Guid.Empty.ToString())
                    state.CorrelationId = correlationId;

                ex.AddState(state);
            }

            return ex;
        }
    }
}
