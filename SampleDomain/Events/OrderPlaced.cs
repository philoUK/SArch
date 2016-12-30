using System;
using Domain;
using SampleDomain.ValueTypes;

namespace SampleDomain.Events
{
    public class OrderPlaced : AggregateEventBase
    {
        public Guid CustomerId { get; set; }
        public OrderItem[] Items { get; set; }
    }
}
