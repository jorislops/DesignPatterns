namespace DesignPatterns.CompositeAndVisitor;

public abstract class ShipmentItem
{
    // public abstract int CountProducts();
    //
    // public abstract void DisplayProducts();

    public abstract void Accept(ShipmentVisitor visitor);
    
    public abstract void Accept(ShipmentVisitorWithoutRecursion shipmentVisitorWithoutRecursion);
}

public abstract class ShipmentItemGroup : ShipmentItem
{
    private List<ShipmentItem> _items = new List<ShipmentItem>();

    public void Add(ShipmentItem item)
    {
        _items.Add(item);
    }

    public void Remove(ShipmentItem item)
    {
        _items.Remove(item);
    }

    public IReadOnlyCollection<ShipmentItem> Items => _items;

    // public override int CountProducts()
    // {
    //     // return Items.Sum(x => x.CountProducts());
    //     int count = 0;
    //     foreach (var item in _items)
    //     {
    //         //recursion
    //         count += item.CountProducts();
    //     }
    //
    //     return count;
    // }

    // public override void DisplayProducts()
    // {
    //     foreach (var item in Items)
    //     {
    //         item.DisplayProducts();
    //     }
    // }
    
    public override void Accept(ShipmentVisitor visitor)
    {
        visitor.Visit(this);
    }

    
}

public class Shipment : ShipmentItemGroup
{
    // public override int Count()
    // {
    //     return Items.Sum(x => x.Count());
    // }

    // public override void DisplayProducts()
    // {
    //     Console.WriteLine("Shipment Contains: ");
    //     base.DisplayProducts();
    // }
    public override void Accept(ShipmentVisitor visitor)
    {
        visitor.Visit(this);
        base.Accept(visitor);
    }

    public override void Accept(ShipmentVisitorWithoutRecursion shipmentVisitorWithoutRecursion)
    {
        shipmentVisitorWithoutRecursion.Visit(this);
        
        //the traversal is done by the Composite pattern, this code can be moved to the base class
        //we should not forget to call the base class accept method in the accept method!
        //I leave it here to show the difference between the two visitors and make it easier to understand
        foreach (var shipmentItem in Items)
        {
            shipmentItem.Accept(shipmentVisitorWithoutRecursion);
        }
    }
}

public class Pallet : ShipmentItemGroup
{
    // public override int Count()
    // {
    //     return Items.Sum(x => x.Count());
    // }
    // public override void DisplayProducts()
    // {
    //     Console.WriteLine("Pallet Contains: ");
    //     base.DisplayProducts();
    // }
    public override void Accept(ShipmentVisitor visitor)
    {
        visitor.Visit(this);
        base.Accept(visitor);
    }
    
    public override void Accept(ShipmentVisitorWithoutRecursion shipmentVisitorWithoutRecursion)
    {
        shipmentVisitorWithoutRecursion.PreVisit(this);
        
        //the traversal is done by the Composite pattern, this code can be moved to the base class
        //we should not forget to call the base class accept method in the accept method!
        //I leave it here to show the difference between the two visitors and make it easier to understand
        foreach (var shipmentItem in Items)
        {
            shipmentItem.Accept(shipmentVisitorWithoutRecursion);
        }
        
        shipmentVisitorWithoutRecursion.PostVisit(this);
    }
    
    
}

public class Box : ShipmentItemGroup
{
    // public override int Count()
    // {
    //     return Items.Sum(x => x.Count());
    // }
    // public override void DisplayProducts()
    // {
    //     Console.WriteLine("Box Contains: ");
    //     foreach (var item in Items)
    //     {
    //         item.DisplayProducts();
    //     }
    // }
    public override void Accept(ShipmentVisitor visitor)
    {
        visitor.Visit(this);
        base.Accept(visitor);
    }
    
    public override void Accept(ShipmentVisitorWithoutRecursion shipmentVisitorWithoutRecursion)
    {
        shipmentVisitorWithoutRecursion.PreVisit(this);
        
        //the traversal is done by the Composite pattern, this code can be moved to the base class
        //we should not forget to call the base class accept method in the accept method!
        //I leave it here to show the difference between the two visitors and make it easier to understand
        foreach (var shipmentItem in Items)
        {
            shipmentItem.Accept(shipmentVisitorWithoutRecursion);
        }
        
        shipmentVisitorWithoutRecursion.PostVisit(this);
    }
}

public abstract class Product : ShipmentItem
{
    public string Name { get; set; } = null!;
    public double Price { get; set; }
    public int Weight { get; set; }
}

public class ProductWithSerialNumber : Product
{
    public string SerialNumber { get; set; } = null!;

    //violates the open closed principle
    // public override int CountProducts()
    // {
    //     return 1;
    // }

    // public override void DisplayProducts()
    // {
    //     Console.WriteLine($"Product: {Name}, Serial Number: {SerialNumber}, Price: {Price}, Quantity: {Quantity}, Weight: {Weight}");
    // }
    public override void Accept(ShipmentVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public override void Accept(ShipmentVisitorWithoutRecursion shipmentVisitorWithoutRecursion)
    {
        shipmentVisitorWithoutRecursion.Visit(this);
    }
}

public class ProductWithoutSerialNumber : Product
{
    // public override int CountProducts()
    // {
    //     return 1;
    // }

    // public override void DisplayProducts()
    // {
    //     Console.WriteLine($"Product: {Name}, Price: {Price}, Quantity: {Quantity}, Weight: {Weight}");
    // }


    public override void Accept(ShipmentVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public override void Accept(ShipmentVisitorWithoutRecursion shipmentVisitorWithoutRecursion)
    {
        shipmentVisitorWithoutRecursion.Visit(this);
    }
}

// public class Pallet : ShipmentItem
// {
//     private List<ShipmentItem> _items = new List<ShipmentItem>();
//
//     public void Add(ShipmentItem item)
//     {
//         _items.Add(item);
//     }
//
//     public void Remove(ShipmentItem item)
//     {
//         _items.Remove(item);
//     }
// }
//
// public class Box : ShipmentItem
// {
//     private List<ShipmentItem> _items = new List<ShipmentItem>();
//
//     public void Add(ShipmentItem item)
//     {
//         _items.Add(item);
//     }
//
//     public void Remove(ShipmentItem item)
//     {
//         _items.Remove(item);
//     }
// } 



// public class Product : ShipmentItem
// {
//     public string Name { get; set; }
//     public string SerialNumber { get; set; }
//     public double Price { get; set; }
//     public int Quantity { get; set; }
//     public int Weight { get; set; }
// }
//
// public class ProductWithSerialNumber : ShipmentItem
// {
//     public string Name { get; set; }
//     public double Price { get; set; }
//     public int Quantity { get; set; }
//     public int Weight { get; set; }
// }

