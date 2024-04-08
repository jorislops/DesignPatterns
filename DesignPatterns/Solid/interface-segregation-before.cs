using static System.Console;

namespace DesignPatterns.Solid;

public class InterfaceSegregationBefore
{
    public static void Main()
    {
        var order = new Order();
        order.AddOrderLine(new Order.ItemQuantityPrice("apple", 2, 1));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 3, 2.5m));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 2, 2.5m));
        WriteLine(order.TotalPrice());
        
        // Only paypal requires AuthorizeBySms, so this is a violation of the Interface Segregation Principle
        // The other payment providers do not require this method, but they still have to implement it
        // so they do this by throwing a NotImplementedException.
        // This is a violation of the Interface Segregation Principle!
        // The Interface Segregation Principle states that a class should not be forced to implement an interface
        // that it does not use.
        // This is fixed by splitting up the IPaymentProcessor interface into two separate interfaces
        // var is actually needed because we are using the interface IAuthorizeBySms and IPaymentProcessor 
        var paypalPaymentProcessor = new PaypalPaymentProcessor("test@test.com");
        paypalPaymentProcessor.Pay(order);
        paypalPaymentProcessor.AuthorizeBySms(order, "987652");
        
        PaymentProcessor creditPaymentProcessor = new CreditPaymentProcessor("1234");
        creditPaymentProcessor.Pay(order); 
        
        var afterpayPaymentProcessor = new AfterpayPaymentProcessor("test@test.com", 12);
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

        public abstract void AuthorizeBySms(Order order, string code);
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

        public override void AuthorizeBySms(Order order, string code)
        {
            throw new NotImplementedException("Debit payment does not require sms verification");
        }
    }
    
    private class CreditPaymentProcessor : PaymentProcessor
    {
        private string _securityCode;

        public CreditPaymentProcessor(string securityCode)
        {
            _securityCode = securityCode;
        }
        
        public override void AuthorizeBySms(Order order, string code)
        {
            throw new NotImplementedException("Credit payment does not require sms verification");
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
        public bool _verified = false;
        
        public PaypalPaymentProcessor(string email)
        {
            _email = email;
        }

        public override void Pay(Order order)
        {
            if (!_verified)
            {
                throw new NotImplementedException("Paypal payment requires sms verification");
            }
            
            WriteLine("Processing paypal payment type");
            // changed this to email!!
            WriteLine($"Verifying email address {_email}");
            order.Status = "paid";
        }

        public override void AuthorizeBySms(Order order, string code)
        {
            //code to check send sms and verify
            if ("987652".Equals(code))
            {
                _verified = true;    
            }
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
                WriteLine("Processing Afterpay payment type");
                WriteLine($"Verifying email address {_email}");
                order.Status = "paid";
            }
            else
            {
                WriteLine("You are not old enough to use Afterpay!");
                order.Status = "not paid";
            }
        }

        public override void AuthorizeBySms(Order order, string code)
        {
            throw new NotImplementedException("Afterpay payment does not require sms verification");
        }

        private bool VerifyAge(int age)
        {
            return age >= 18;
        }
    }
}