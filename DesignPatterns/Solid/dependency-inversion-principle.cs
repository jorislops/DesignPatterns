using static System.Console;

namespace DesignPatterns.Solid;

public class DependencyInversionPrinciple
{
        //
    
    public static void Main()
    {
        var order = new Order();
        order.AddOrderLine(new Order.ItemQuantityPrice("apple", 2, 1));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 3, 2.5m));
        order.AddOrderLine(new Order.ItemQuantityPrice("banana", 2, 2.5m));
        WriteLine(order.TotalPrice());
        
        // We are using composition instead of inheritance (interface)
        // Introducing SmsAuthorizationPaymentProcessor as a separate class
        
        //new SmsAuthorizationPaymentProcessor() is injected into the PaypalPaymentProcessor
        var smsAuthorizationPaymentProcessor = new SmsAuthorizationPaymentProcessor();
        var paypalPaymentProcessor = new PaypalPaymentProcessor("test@test.com", smsAuthorizationPaymentProcessor);
        paypalPaymentProcessor.Pay(order);
        // paypalPaymentProcessor.AuthorizeBySms(order, "987652");
        
        IPaymentProcessor creditPaymentProcessor = new CreditPaymentProcessor("1234");
        creditPaymentProcessor.Pay(order); 
        
        IPaymentProcessor afterpayPaymentProcessor = new AfterpayPaymentProcessor("test@test.com", 12);
        afterpayPaymentProcessor.Pay(order);
    }

    public interface ISmsAuthorizationPaymentProcessor
    {
        void AuthorizeBySms(string code);
        bool Authorize { get; }
    }

    public class SmsAuthorizationPaymentProcessor : ISmsAuthorizationPaymentProcessor
    {
        public bool Authorize { get; private set; } = false;
        
        public void AuthorizeBySms(string code)
        {
            //code to check send sms and verify
            if ("987652".Equals(code))
            {
                Authorize = true;    
            }
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

    private interface IPaymentProcessor
    {
        void Pay(Order order);
    }

    // This interface is not needed anymore, because we use the SmsAuthorizationPaymentProcessor class to authorize
    // private interface IAuthorizeBySms
    // {
    //     void AuthorizeBySms(Order order, string code);
    // }

    private class DebitPaymentProcessor : IPaymentProcessor
    {
        private string _securityCode;

        public DebitPaymentProcessor(string securityCode)
        {
            _securityCode = securityCode;
        }

        public void Pay(Order order)
        {
            WriteLine("Processing debit payment type");
            WriteLine($"Verifying security code {_securityCode}");
            order.Status = "paid";
        }
    }
    
    private class CreditPaymentProcessor : IPaymentProcessor
    {
        private string _securityCode;

        public CreditPaymentProcessor(string securityCode)
        {
            _securityCode = securityCode;
        }

        public void Pay(Order order)
        {
            WriteLine("Processing credit payment type");
            WriteLine($"Verifying security code {_securityCode}");
            order.Status = "paid";
        }
    }
    
    private class PaypalPaymentProcessor : IPaymentProcessor
    {
        private string _email;
        private readonly ISmsAuthorizationPaymentProcessor _paymentProcessor;
        
        public PaypalPaymentProcessor(string email, ISmsAuthorizationPaymentProcessor smsAuthorizationPaymentProcessor)
        {
            _email = email;
            _paymentProcessor = smsAuthorizationPaymentProcessor;
        }

        public void Pay(Order order)
        {
            if (!_paymentProcessor.Authorize)
            {
                throw new NotImplementedException("Paypal payment requires sms verification");
            }
            
            WriteLine("Processing paypal payment type");
            // changed this to email!!
            WriteLine($"Verifying email address {_email}");
            order.Status = "paid";
        }

        // we use the SmsAuthorizationPaymentProcessor class to authorize, so we don't need this method anymore
        // public void AuthorizeBySms(Order order, string code)
        // {
        //     //code to check send sms and verify
        //     if ("987652".Equals(code))
        //     {
        //         _verified = true;    
        //     }
        // }
    }
    
    private class AfterpayPaymentProcessor : IPaymentProcessor
    {
        private readonly string _email;
        private readonly int _age;

        public AfterpayPaymentProcessor(string email, int age)
        {
            _email = email;
            _age = age;
        }
        
        public void Pay(Order order)
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

        private bool VerifyAge(int age)
        {
            return age >= 18;
        }
    }

}