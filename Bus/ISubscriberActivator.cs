using System;

namespace Bus
{
    public interface ISubscriberActivator
    {
        object CreateInstance(Type instanceType);
    }
}