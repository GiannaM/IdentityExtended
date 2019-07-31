using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace FitCode.Identity.Models
{
    [Serializable]
    public class LoggerState : ISerializable
    {
        private Dictionary<string, object> StateValues = new Dictionary<string, object>();

        public string UserId { get { return GetValueOrDefault<string>(nameof(UserId)); } set { SetValue(nameof(UserId), value); } }
        public string Username { get { return GetValueOrDefault<string>(nameof(Username)); } set { SetValue(nameof(Username), value); } }
        public string CorrelationId { get { return GetValueOrDefault<string>(nameof(CorrelationId)); } set { SetValue(nameof(CorrelationId), value); } }
        public string ObjectJson { get { return GetValueOrDefault<string>(nameof(ObjectJson)); } set { SetValue(nameof(ObjectJson), value); } }

        public LoggerState() { }

        private LoggerState(IDictionary<string, object> stateValues)
        {
            StateValues = new Dictionary<string, object>(stateValues);
        }

        private T GetValueOrDefault<T>(string key)
        {
            if (StateValues.TryGetValue(key, out object value))
            {
                try { return (T)value; }
                catch { }
            }

            return default(T);
        }

        private void SetValue<T>(string key, T value)
        {
            StateValues[key] = value;
        }

        public IReadOnlyDictionary<string, object> GetStateValues()
        {
            return new ReadOnlyDictionary<string, object>(StateValues);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(StateValues), StateValues);
        }

        public LoggerState MergedWith(LoggerState newState)
        {
            if (newState == null || newState.StateValues == null || newState.StateValues.Count == 0) { return this; }

            return new LoggerState((from key in StateValues.Keys.Concat(newState.GetStateValues().Keys).Distinct(StringComparer.OrdinalIgnoreCase)
                                    select new KeyValuePair<string, object>(key, newState.StateValues.ContainsKey(key) ? newState.StateValues[key] : StateValues[key]))
                                    .ToDictionary(p => p.Key, p => p.Value));
        }
    }
}
