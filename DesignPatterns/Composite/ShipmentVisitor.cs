namespace DesignPatterns.Composite;

//visitor pattern violates the open closed principle
//because you have to change the visitor when you add a new class
//you have to add a new visit method
//you have to add a new accept method in the Composite when a new class is added to the Composite

//the concrete visitors don't violate the open closed principle
//because you can add a new visitor without changing the existing code
//for example the CountProductsVisitor and DisplayProductsVisitor

//lot of unused visit methods :-(
//maybe make them virtual and override only the ones you need, but I don't like this solution
//you forget to override a method and it will not work

public abstract class ShipmentVisitor
{
    //double dispatch
    //https://refactoring.guru/design-patterns/visitor-double-dispatch
    public abstract void Visit(ProductWithSerialNumber product);
    public abstract void Visit(ProductWithoutSerialNumber product);
    public abstract void Visit(Box box);
    public abstract void Visit(Pallet pallet);
    public abstract void Visit(Shipment shipment);
    public abstract void VisitPre(ShipmentItemGroup shipmentItemGroup);

    public abstract void VisitPost(ShipmentItemGroup shipmentItemGroup);
}

//if the visitor pattern is generated by AI there is a change that the iterations is incorrect!
//Done in the visitor instead of the Composite 
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

    public override void VisitPre(ShipmentItemGroup shipmentItemGroup)
    {
    }

    public override void VisitPost(ShipmentItemGroup shipmentItemGroup)
    {
        
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
    //maybe context object in the Visitor methods is also a solution
    //but then you have to pass the context around (one more parameter in each visit method)
    //this actually violates the single responsibility principle
    //because the visitor has to know about depth of the hierarchy :-(
    private int _level; 
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

    public override void VisitPre(ShipmentItemGroup shipmentItemGroup)
    {
        _level++;
    }

    public override void VisitPost(ShipmentItemGroup shipmentItemGroup)
    {
        _level--;
    }

    // public override void Visit(ShipmentItemGroup group)
    // {
    //     _level++;
    // }
    
    private string GetIndent()
    {
        return new string(' ', _level * 2);
    }
}