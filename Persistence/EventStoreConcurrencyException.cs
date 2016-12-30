using System;

namespace Persistence
{
    public class EventStoreConcurrencyException : Exception
    {
        public EventStoreConcurrencyException(string msg) : base(msg)
        {

        }
    }
}
