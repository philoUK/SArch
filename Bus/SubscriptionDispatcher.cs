using System;
using System.Linq;
using System.Reflection;
using Domain;

namespace Bus
{
    public class SubscriptionDispatcher
    {
        private readonly ISubscriberActivator activator;

        public SubscriptionDispatcher(ISubscriberActivator activator)
        {
            this.activator = activator;
        }

        public object Dispatch(string eventTypeName, IAggregateEvent @event)
        {
            var type = Type.GetType(eventTypeName);
            if (IsClassicInterface(type))
            {
                return this.DispatchViaClassicInterface(type, @event);
            }
            return null;
        }

        private object DispatchViaClassicInterface(Type type, IAggregateEvent @event)
        {
            var results = this.activator.CreateInstance(type);
            var interfaceType = type.GetInterfaces()
                .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandleEventsOf<>) &&
                             x.GetGenericArguments()[0] == @event.GetType());
            interfaceType.InvokeMember("HandleEvent",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod ,null,
                results, new object[] {@event});
            return results;
        }

        private bool IsClassicInterface(Type type)
        {
            return type.GetInterfaces().Any(x => x.IsGenericType &&
                                                 x.GetGenericTypeDefinition() == typeof(IHandleEventsOf<>));
        }
    }

}
