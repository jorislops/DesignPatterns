using static System.Console;

namespace DesignPatterns.Solid;

public class LiskovSubstitutionAfter
{
    public static void Main()
    {
        var order = new Order();
        order.AddOrderLine(new Order.ItemQuantityPrice("apple", 2, 1));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 3, 2.5m));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 2, 2.5m));
        WriteLine(order.TotalPrice());
        PaymentProcessor paypalPaymentProcessor = new PaypalPaymentProcessor("test@test.com");
        paypalPaymentProcessor.Pay(order); //strange an email address is used here

        //we can still supply a email address here, it's simply a string and not a security code
        //so what we also need to do is create a value type for the security code, but this is out of scope for this example
        PaymentProcessor creditPaymentProcessor = new CreditPaymentProcessor("1234");
        creditPaymentProcessor.Pay(order); //strange an security code is used here
        
        PaymentProcessor afterpayPaymentProcessor = new AfterpayPaymentProcessor("test@test.com", 12);
        
        // This is a violation of the Liskov Substitution Principle, because VerifyAge is not applicable to all PaymentProcessor implementations!
        // bool verifyAge = ((AfterpayPaymentProcessor)afterpayPaymentProcessor).VerifyAge(20);
        // if (verifyAge)
        // {
        //     afterpayPaymentProcessor.Pay(order, "1234"); //we should use an email address here
        // }
        //The verify age method is now encapsulated in the AfterpayPaymentProcessor
        //this solves the Liskov Substitution Principle violation
        afterpayPaymentProcessor.Pay(order); 
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
        public abstract void Pay(Order order);
    }
    
    private class DebitPaymentProcessor : PaymentProcessor
    {
        private string _securityCode;

        public DebitPaymentProcessor(string securityCode)
        {
            _securityCode = securityCode;
        }

        public override void Pay(Order order)
        {
            WriteLine("Processing debit payment type");
            WriteLine($"Verifying security code {_securityCode}");
            order.Status = "paid";
        }
    }
    
    private class CreditPaymentProcessor : PaymentProcessor
    {
        private string _securityCode;

        public CreditPaymentProcessor(string securityCode)
        {
            _securityCode = securityCode;
        }

        public override void Pay(Order order)
        {
            WriteLine("Processing credit payment type");
            WriteLine($"Verifying security code {_securityCode}");
            order.Status = "paid";
        }
    }
    
    private class PaypalPaymentProcessor : PaymentProcessor
    {
        private string _email;

        public PaypalPaymentProcessor(string email)
        {
            _email = email;
        }

        public override void Pay(Order order)
        {
            WriteLine("Processing paypal payment type");
            // changed this to email!!
            WriteLine($"Verifying email address {_email}");
            order.Status = "paid";
        }
    }
    
    private class AfterpayPaymentProcessor : PaymentProcessor
    {
        private readonly string _email;
        private readonly int _age;

        public AfterpayPaymentProcessor(string email, int age)
        {
            _email = email;
            _age = age;
        }
        
        public override void Pay(Order order)
        {
            if(VerifyAge(_age)) {
                WriteLine("Processing afterpay payment type");
                WriteLine($"Verifying email address {_email}");
                order.Status = "paid";
            }
            else
            {
                WriteLine("You are not old enough to use Afterpay!");
                order.Status = "not paid";
            }
        }
        
        // this is a violation of the Liskov Substitution Principle, because VerifyAge is not applicable
        // to all PaymentProcessor implementations!
        // public bool VerifyAge(int age)
        // {
        //     return age >= 18;
        // }
        
        private bool VerifyAge(int age)
        {
            return age >= 18;
        }
    }
}