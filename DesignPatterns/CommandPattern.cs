namespace DesignPatterns;

// public class CommandClient
// {
//     public static void Main()
//     {
//         //Client: main is the client
//
// //Receiver: stock class
//         Stock abcStock = new Stock();
//
// //Creates two concrete commands (BuyStock and SellStock)
//         BuyStock buyStockOrder = new BuyStock(abcStock);
//         SellStock sellStockOrder = new SellStock(abcStock);
//
// //Creates an invoker object that will take the order and place the order
//         Broker broker = new Broker();
//         broker.TakeOrder(buyStockOrder);
//         broker.TakeOrder(sellStockOrder);
//         broker.PlaceOrders();
//
//     }
// }


public class Stock
{
    private string _name = "ABC";
    private int _quantity = 10;

    public void Buy()
    {
        Console.WriteLine("Stock [ Name: " + _name + ", Quantity: " + _quantity + " ] bought");
    }

    public void Sell()
    {
        Console.WriteLine("Stock [ Name: " + _name + ", Quantity: " + _quantity + " ] sold");
    }
}

public interface Order
{
    void Execute();
}

public class BuyStock : Order
{
    private Stock _abcStock;

    public BuyStock(Stock abcStock)
    {
        _abcStock = abcStock;
    }

    public void Execute()
    {
        _abcStock.Buy();
    }
}

public class SellStock : Order
{
    private Stock _abcStock;

    public SellStock(Stock abcStock)
    {
        _abcStock = abcStock;
    }

    public void Execute()
    {
        _abcStock.Sell();
    }
}

public class Broker
{
    private List<Order> _orderList = new List<Order>();

    public void TakeOrder(Order order)
    {
        _orderList.Add(order);
    }

    public void PlaceOrders()
    {
        foreach (Order order in _orderList)
        {
            order.Execute();
        }
        _orderList.Clear();
    }
}



