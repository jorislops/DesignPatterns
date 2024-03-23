namespace DesignPatterns.Composite;

public class SeedShipment
{
    public static Shipment CreateDummyShipment()
    {
        //maybe better to use a builder pattern
        //or a Faker for example: https://github.com/bchavez/Bogus
        
        var shipment = new Shipment();
            var pallet1 = new Pallet();
            shipment.Add(pallet1);
                var box1 = new Box();
                    var product1 = new ProductWithSerialNumber();
                    product1.Name = "Product 1";
                    product1.SerialNumber = "1234";
                    product1.Price = 10;
                    product1.Weight = 10;
                    product1.SerialNumber = "1234";
                    box1.Add(product1);
                    
                    var product2 = new ProductWithSerialNumber();
                    product2.Name = "Product 2";
                    product2.SerialNumber = "2345";
                    product2.Price = 20;
                    product2.Weight = 20;
                    box1.Add(product2);
                pallet1.Add(box1);
                
            var pallet2 = new Pallet();
            shipment.Add(pallet2);
                var box2 = new Box();
                    var product3 = new ProductWithoutSerialNumber();
                    product3.Name = "Product 3";
                    product3.Price = 30;
                    product3.Weight = 30;
                    box2.Add(product3);
                    
                    var product4 = new ProductWithoutSerialNumber();
                    product4.Name = "Product 4";
                    product4.Price = 40;
                    product4.Weight = 40;
                    box2.Add(product4);
                pallet2.Add(box2);
            

        return shipment;
    }

    public static Shipment CreateShipmentWithoutPallets()
    {
        var shipment = new Shipment();
            var box1 = new Box();
                var product1 = new ProductWithSerialNumber();
                    product1.Name = "Product 1";
                    product1.SerialNumber = "1234";
                    product1.Price = 10;
                    product1.Weight = 10;
                box1.Add(product1);
                var product2 = new ProductWithSerialNumber();
                    product2.Name = "Product 2";
                    product2.SerialNumber = "2345";
                    product2.Price = 20;
                    product2.Weight = 20;
                box1.Add(product2);
            shipment.Add(box1);
            var product3 = new ProductWithoutSerialNumber();
                product3.Name = "Product 3";
                product3.Price = 30;
                product3.Weight = 30;
            shipment.Add(product3);

        return shipment;
    }
}