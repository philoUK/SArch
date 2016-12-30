namespace SampleDomain.ValueTypes
{
    public class OrderItem
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public OrderItem()
        {
            
        }

        public OrderItem(string sku, int qty, decimal price)
        {
            this.Sku = sku;
            this.Quantity = qty;
            this.UnitPrice = price;
        }
    }
}
