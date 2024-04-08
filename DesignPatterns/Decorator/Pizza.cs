using static System.Console;

namespace DesignPatterns.Decorator;

public class PizzaClient
{
    public static void Main()
    {
        var pizzaMargherita = new CheeseToppingDecorator(new PizzaBase());
        WriteLine(pizzaMargherita.GetDescription());
        WriteLine(pizzaMargherita.GetPrice());
        
        WriteLine("------------------");

        var pizzaHawaii =
            new PineappleToppingDecorator(
                new HamToppingDecorator(
                    new CheeseToppingDecorator(
                        new PizzaBase()
                        )
                    )
                );
        WriteLine(pizzaHawaii.GetDescription());
        WriteLine(pizzaHawaii.GetPrice());
    }
}

public interface IPizza
{
    public string GetDescription();
    public decimal GetPrice();
}

public class PizzaBase : IPizza
{
    public string GetDescription()
    {
        return "Pizza Base with tomato sauce";
    }

    public decimal GetPrice()
    {
        return 5;
    }
}

public abstract class PizzaToppingDecorator(IPizza pizza) : IPizza
{
    protected readonly IPizza Pizza = pizza;

    public abstract string GetDescription();
    public abstract decimal GetPrice();
} 

public class CheeseToppingDecorator : PizzaToppingDecorator
{
    public CheeseToppingDecorator(IPizza pizza) : base(pizza)
    {
    }

    public override string GetDescription() => $"{Pizza.GetDescription()} with cheese";
    public override decimal GetPrice() => Pizza.GetPrice() + 1.0m;
}

public class HamToppingDecorator : PizzaToppingDecorator
{
    public HamToppingDecorator(IPizza pizza) : base(pizza)
    {
    }

    public override string GetDescription() => $"{Pizza.GetDescription()} with ham";
    public override decimal GetPrice() => Pizza.GetPrice() + 2.0m;
}

public class PineappleToppingDecorator : PizzaToppingDecorator
{
    public PineappleToppingDecorator(IPizza pizza) : base(pizza)
    {
    }

    public override string GetDescription() => $"{Pizza.GetDescription()} with mushrooms";
    public override decimal GetPrice() => Pizza.GetPrice() + 3.0m;
}