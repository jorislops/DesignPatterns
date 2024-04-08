using static System.Console;

namespace DesignPatterns.Solid;

public class SingleResponsibilityAfter
{
 public static void Main()
    {
        var order = new Order();
        order.AddOrderLine(new Order.ItemQuantityPrice("apple", 2, 1));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 3, 2.5m));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 2, 2.5m));
        WriteLine(order.TotalPrice());
        var paymentProcessor = new PaymentProcessor();
        paymentProcessor.PayDebit(order, "1234");
    }

    private class Order
    {
        public record ItemQuantityPrice(string Product, decimal Quantity, decimal UnitPrice)
        {
        }
        
        private List<ItemQuantityPrice> _orderlines = new List<ItemQuantityPrice>();
        public string Status { get; set; } = "Open";

        public void AddOrderLine(ItemQuantityPrice itemQuantityPrice)
        {
            int index = _orderlines.FindIndex(x => 
                x.Product.Equals(itemQuantityPrice.Product) &&
                x.UnitPrice.Equals(itemQuantityPrice.UnitPrice));
            if(index != -1)
            {
                var quantity = _orderlines[index].Quantity;
                _orderlines[index] = _orderlines[index] 
                    with { Quantity = itemQuantityPrice.Quantity + quantity };
            } else
            {
                _orderlines.Add(itemQuantityPrice);    
            }
        }
        
        public decimal TotalPrice()
        {
            return _orderlines.Sum(x => x.Quantity * x.UnitPrice);
        }
    }

    private class PaymentProcessor
    {
        public void PayDebit(Order order, string securityCode)
        {
            WriteLine("Processing debit payment type");
            WriteLine($"Verifying security code {securityCode}");
            order.Status = "paid";
        }
        
        public void PayCredit(Order order, string securityCode)
        {
            WriteLine("Processing credit payment type");
            WriteLine($"Verifying security code {securityCode}");
            order.Status = "paid";
        }
        
    }
}