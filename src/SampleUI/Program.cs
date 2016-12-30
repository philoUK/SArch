using System;
using Autofac;
using Persistence;
using SampleDomain.Aggregates;
using SampleDomain.ValueTypes;

namespace SampleUI
{
    public class Program
    {
        private static IContainer Container;
        private static IRepository<Order> orderRepository;

        //private static Repository<Order> orderRepository;

        public static void Main(string[] args)
        {
            orderRepository = Container.Resolve<IRepository<Order>>();
            AcceptOrders();
        }

        private static void AcceptOrders()
        {
            Console.WriteLine("Welcome to Order Entry [stupid version]");
            var stillOrderingProducts = true;
            do
            {
                Console.WriteLine("Enter [p] to place order, or [x] to quit");
                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case 'p':
                    case 'P':
                        PlaceOrder();
                        break;
                    case 'x':
                    case 'X':
                        stillOrderingProducts = false;
                        break;
                }
            } while (stillOrderingProducts);
        }

        private static void PlaceOrder()
        {
            var order = new Order();
            var sku = "test-sku";
            var qty = 10;
            var unitPrice = 99.95M;
            var customerId = Guid.NewGuid();
            var item = new OrderItem(sku, qty, unitPrice);
            order.Place(customerId, item);
            orderRepository.Save(order);
            Console.WriteLine("Order placed");
        }
    }
}
