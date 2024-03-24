// See https://aka.ms/new-console-template for more information

using DesignPatterns;
using DesignPatterns.Composite;

var shipment = SeedShipment.CreateDummyShipment();

var countProductsVisitor = new CountProductsVisitor();
shipment.Accept(countProductsVisitor);
Console.WriteLine(countProductsVisitor.Count);

var printProductsVisitor = new DisplayProductsVisitor();
shipment.Accept(printProductsVisitor);
Console.WriteLine(printProductsVisitor.ToString());
