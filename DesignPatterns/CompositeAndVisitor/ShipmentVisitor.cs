namespace DesignPatterns.CompositeAndVisitor;

//visitor pattern violates the open closed principle
//because you have to change the visitor when you add a new class
//you have to add a new visit method

//the concrete visitors don't violate the open closed principle
//because you can add a new visitor without changing the existing code
//for example the CountProductsVisitor and DisplayProductsVisitor

//lot of unused visit methods :-(
//maybe make them virtual and override only the ones that you need
//we need a base class for the visitors that implements the traversal
//and the concrete visitors should only implement the visit methods that they need
//the traversal should be done in the base class, not in the concrete visitors
//so we should not forget to call the base class accept method in the accept method!

public class VisitorClient
{
    public static void Main()
    {
        var shipment = SeedShipment.CreateDummyShipment();

        var countProductsVisitor = new CountProductsVisitor();
        shipment.Accept(countProductsVisitor);
        Console.WriteLine(countProductsVisitor.Count);

        var printProductsVisitor = new DisplayProductsVisitor();
        shipment.Accept(printProductsVisitor);
        Console.WriteLine(printProductsVisitor.ToString());
    }
}

public abstract class ShipmentVisitor
{
    //double dispatch
    //https://refactoring.guru/design-patterns/visitor-double-dispatch
    public abstract void Visit(ProductWithSerialNumber product);
    public abstract void Visit(ProductWithoutSerialNumber product);
    public abstract void Visit(Box box);
    public abstract void Visit(Pallet pallet);
    public abstract void Visit(Shipment shipment);
    public abstract void Visit(ShipmentItemGroup shipment);
}

public class CountProductsVisitor : ShipmentVisitor
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

    public override void Visit(Box box)
    {
    }

    public override void Visit(Pallet pallet)
    {
    }

    public override void Visit(Shipment shipment)
    {
        
    }

    public override void Visit(ShipmentItemGroup shipment)
    {
        foreach (var shipmentItem in shipment.Items)
        {
            shipmentItem.Accept(this);
        }
    }
}

// This visitor violates the Dependency Inversion Principle :-(
// why because the DisplayProductsVisitor depends on the Console class
// and the Console class is a concrete class
// The idea is that the DisplayProductsVisitor should depend on an abstraction
// interface IWriter { void Write(string value); }
// Or even better a Writer class that has a Write method for each class in the Composite
// Or even better a Writer class that has a Write method that know by reflection how to write the class to the destination
// JsonDeserializer.Serialize(object obj)
// so we can make concrete classes that implement the abstraction
// for example a JsonWriter, XmlWriter, etc.
public class DisplayProductsVisitor : ShipmentVisitor
{
    private int _level; 
    
    public override void Visit(ShipmentItemGroup shipment)
    {
        _level++;
        foreach (var shipmentItem in shipment.Items)
        {
            shipmentItem.Accept(this);
        }
        _level--;
    }
    
    public override void Visit(ProductWithSerialNumber product)
    {
        Console.WriteLine($"{GetIndent()}Product: {product.Name}, Serial Number: {product.SerialNumber}, Price: {product.Price}, Weight: {product.Weight}");
    }

    public override void Visit(ProductWithoutSerialNumber product)
    {
        Console.WriteLine($"{GetIndent()}Product: {product.Name}, Price: {product.Price}, Weight: {product.Weight}");
    }

    public override void Visit(Box box)
    {
        Console.WriteLine($"{GetIndent()}Box Contains: ");
    }

    public override void Visit(Pallet pallet)
    {
        Console.WriteLine($"{GetIndent()}Pallet Contains: ");
    }

    public override void Visit(Shipment shipment)
    {
        Console.WriteLine($"{GetIndent()}Shipment Contains: ");
    }
    
    private string GetIndent()
    {
        return new string(' ', _level * 2);
    }
}