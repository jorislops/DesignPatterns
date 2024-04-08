namespace DesignPatterns.CompositeAndVisitor;

public class VisitorWithoutRecursionClient
{
    public static void Main()
    {
        var shipment = SeedShipment.CreateDummyShipment();
        
        var countProductsVisitorWithoutRecursion = new CountProductsVisitorWithoutRecursion();
        shipment.Accept(countProductsVisitorWithoutRecursion);
        Console.WriteLine("Number of Products: " +countProductsVisitorWithoutRecursion.Count);
        
        
        var printVisitorWithoutRecursion = new PrintVisitorWithoutRecursion();
        shipment.Accept(printVisitorWithoutRecursion);
        
        
    }
}

public abstract class ShipmentVisitorWithoutRecursion
{
    public abstract void Visit(ProductWithSerialNumber product);
    public abstract void Visit(ProductWithoutSerialNumber product);
    public abstract void PreVisit(Box box);
    
    public abstract void PostVisit(Box box);
    public abstract void PreVisit(Pallet pallet);
    public abstract void PostVisit(Pallet pallet);
    public abstract void Visit(Shipment shipment);
    
    //this method is not needed for this example
    public abstract void Visit(ShipmentItemGroup shipment);
}

public class CountProductsVisitorWithoutRecursion : ShipmentVisitorWithoutRecursion
{
    public int Count { get; private set; }

    public override void Visit(ProductWithSerialNumber product)
    {
        Count++;
    }

    public override void Visit(ProductWithoutSerialNumber product)
    {
        Count++;
    }

    public override void PreVisit(Box box)
    {
    }

    public override void PostVisit(Box box)
    {
        
    }

    public override void PreVisit(Pallet pallet)
    {
        
    }

    public override void PostVisit(Pallet pallet)
    {
        
    }

    public override void Visit(Shipment shipment)
    {
    }

    public override void Visit(ShipmentItemGroup shipment)
    {
        // the traversal is done by the Composite pattern
        // foreach (var shipmentItem in shipment.Items)
        // {
        //     shipmentItem.Accept(this);
        // }
    }
}

public class PrintVisitorWithoutRecursion : ShipmentVisitorWithoutRecursion
{
    private int _level = 0;

    private string GetIndent()
    {
        return new string('\t', _level);
    }
    
    public override void Visit(ProductWithSerialNumber product)
    {
        Console.WriteLine($"{GetIndent()}Product: {product.Name}, Serial Number: {product.SerialNumber}, Price: {product.Price}, Weight: {product.Weight}");
    }

    public override void Visit(ProductWithoutSerialNumber product)
    {
        Console.WriteLine($"{GetIndent()}Product: {product.Name}, Price: {product.Price}, Weight: {product.Weight}");
    }

    public override void PreVisit(Box box)
    {
        Console.WriteLine($"{GetIndent()}Box Contains: ");
        _level++;
    }

    public override void PostVisit(Box box)
    {
        _level--;
    }

    public override void PreVisit(Pallet pallet)
    {
        Console.WriteLine($"{GetIndent()}Pallet Contains: ");
        _level++;
    }

    public override void PostVisit(Pallet pallet)
    {
        _level--;
    }

    public override void Visit(Shipment shipment)
    {
        Console.WriteLine($"{GetIndent()}Shipment Contains: ");
        _level++;
    }

    //This method is never called, we could create a PreVisit() and PostVisit() method in the base class
    //but I duplicated the _level++ and _level-- code to make it easier to understand
    public override void Visit(ShipmentItemGroup shipment)
    {
        Console.WriteLine($"{GetIndent()} Shipment Item Group");
    }
}