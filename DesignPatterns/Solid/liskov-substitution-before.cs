using static System.Console;

namespace DesignPatterns.Solid;

public class LiskovSubstitutionBefore
{
    public static void Main()
    {
        var order = new Order();
        order.AddOrderLine(new Order.ItemQuantityPrice("apple", 2, 1));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 3, 2.5m));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 2, 2.5m));
        WriteLine(order.TotalPrice());
        PaymentProcessor paypalPaymentProcessor = new PaypalPaymentProcessor();
        paypalPaymentProcessor.Pay(order, "test@test.com"); //strange an email address is used here

        PaymentProcessor creditPaymentProcessor = new CreditPaymentProcessor();
        creditPaymentProcessor.Pay(order, "1234"); //strange an security code is used here
        
        PaymentProcessor afterpayPaymentProcessor = new AfterpayPaymentProcessor();
        // this is a violation of the Liskov Substitution Principle, because VerifyAge is not applicable to all PaymentProcessor implementations!
        bool verifyAge = ((AfterpayPaymentProcessor)afterpayPaymentProcessor).VerifyAge(20);
        if (verifyAge)
        {
            afterpayPaymentProcessor.Pay(order, "1234"); //we should use an email address here
        }
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

    private abstract class PaymentProcessor
    {
        public abstract void Pay(Order order, string email);
    }
    
    private class DebitPaymentProcessor : PaymentProcessor
    {
        public override void Pay(Order order, string email)
        {
            WriteLine("Processing debit payment type");
            WriteLine($"Verifying security code {email}");
            order.Status = "paid";
        }
    }
    
    private class CreditPaymentProcessor : PaymentProcessor
    {
        public override void Pay(Order order, string email)
        {
            WriteLine("Processing credit payment type");
            WriteLine($"Verifying security code {email}");
            order.Status = "paid";
        }
    }
    
    private class PaypalPaymentProcessor : PaymentProcessor
    {
        public override void Pay(Order order, string email)
        {
            WriteLine("Processing paypal payment type");
            // changed this to email!!
            WriteLine($"Verifying email address {email}");
            order.Status = "paid";
        }
    }
    
    private class AfterpayPaymentProcessor : PaymentProcessor
    {
        public override void Pay(Order order, string email)
        {
            WriteLine("Processing afterpay payment type");
            WriteLine($"Verifying email address {email}");
            order.Status = "paid";
        }
        
        // this is a violation of the Liskov Substitution Principle, because VerifyAge is not applicable
        // to all PaymentProcessor implementations!
        public bool VerifyAge(int age)
        {
            return age >= 18;
        }
    }
}