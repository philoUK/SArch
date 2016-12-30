using System;
using Domain;
using Newtonsoft.Json;

namespace Shared
{
    public static class AggregateEventExtensions
    {
        public static string ToJsonString(this IAggregateEvent @event)
        {
            return JsonConvert.SerializeObject(@event);
        }

        public static IAggregateEvent DeSerialise(string eventData, string eventType)
        {
            var typeToDeserialise = Type.GetType(eventType);
            var rawResults = JsonConvert.DeserializeObject(eventData, typeToDeserialise);
            return (IAggregateEvent) rawResults;
        }
    }
}
