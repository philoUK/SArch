using System;
using System.Collections.Generic;
using Domain;
using SampleDomain.Events;
using SampleDomain.ValueTypes;

namespace SampleDomain.Aggregates
{
    public class Order : AggregateRoot
    {
        private bool _placed;
        private Guid _customerId;
        private List<OrderItem> _items;
        private bool _cancelled;
        private bool _verified;

        public void Place(Guid customerId, params OrderItem[] items)
        {
            ValidatePlaceOrder(customerId, items);
            this.Id = Guid.NewGuid();
            Apply(new OrderPlaced {CustomerId = customerId, Items = items});
        }

        private void ValidatePlaceOrder(Guid customerId, OrderItem[] items)
        {
            if (_cancelled)
            {
                throw new InvalidOperationException();
            }
            if (_placed)
            {
                throw new InvalidOperationException();
            }
            if (_verified)
            {
                throw new InvalidOperationException();
            }
            if (customerId == Guid.Empty)
            {
                throw new InvalidOperationException();
            }
            if ((items?.Length ?? 0) == 0)
            {
                throw new InvalidOperationException();
            }
        }

        private void OnOrderPlaced(OrderPlaced @event)
        {
            _placed = true;
            _customerId = @event.CustomerId;
            _items = new List<OrderItem>(@event.Items);
        }

        public void Cancel()
        {
            ValidateCancelOrder();
            Apply(new OrderCancelled());
        }

        private void ValidateCancelOrder()
        {
            if (!_placed && !_cancelled)
            {
                throw new InvalidOperationException();
            }
        }

        private void OnOrderCancelled(OrderCancelled @event)
        {
            _cancelled = true;
        }

        public void Verify()
        {
            ValidateVerifyOrder();
            Apply(new OrderVerified());
        }

        private void ValidateVerifyOrder()
        {
            if (!_placed || _cancelled)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
